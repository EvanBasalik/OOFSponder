# Adapted from https://janjones.me/posts/clickonce-installer-build-publish-github/.
# production location = https://evanbasalik.github.io/OOFSponder/production/OOFScheduling.application
# insider location = https://evanbasalik.github.io/OOFSponder/insider/OOFScheduling.application
# alpha location = https://evanbasalik.github.io/OOFSponder/alpha/OOFScheduling.application

[CmdletBinding(PositionalBinding=$false)]
param (
    [switch]$BuildOnly=$false,
    [switch]$NoOOF=$false,
    [parameter(mandatory)] [validateset("alpha","insider", "production")] [string] $ring 
)

Write-Output "Ring: $ring"

$appName = "OOFScheduling" # 👈 Replace with your application project name.
$projDir = "OOFSponderCore" # 👈 Replace with your project directory (where .csproj resides).

Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

# Load current Git tag.
$tag = $(git describe --tags)
Write-Output "Tag: $tag"

# Parse tag into a three-number version.
$version = $tag.Split('-')[0].TrimStart('v')

# Require major.minor.revision format
if ($version.Split(".").Count -ne 3) {
    Write-Error "Tag must have major.minor.revision format"
}

$version = "$version.0"
Write-Output "Version: $version"

# Clean output directory.
$publishDir = "$ring/bin/publish"
Write-Output "Publish directory: $publishDir"

$outDir = "$projDir/$publishDir"
Write-Output "Out directory: $outDir"

if (Test-Path $outDir) {
    Remove-Item -Path $outDir -Recurse
}

# Get the right Publish profile
$publishProfile = "ClickOnceProfile"
if($NoOOF) {
    $publishProfile = "ClickOnceProfileNoOOF"
}

Write-Output "Publish profile: $publishProfile"

Write-Output "Working directory: $pwd"

# Find MSBuild.
$msBuildPath = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" `
    -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe `
    -prerelease | select-object -first 1
Write-Output "MSBuild: $((Get-Command $msBuildPath).Path)"

# Publish the application.
Push-Location $projDir
try {
    Write-Output "Restoring:"
    dotnet restore -r win-x64
    Write-Output "Publishing:"
    $msBuildVerbosityArg = "/v:m"
    if ($env:CI) {
        $msBuildVerbosityArg = ""
    }

    # stick the ring inside the installUrl
    # do it externally to the command to make this easier to debug
    $installUrl = "https://evanbasalik.github.io/OOFSponder/" + $ring + "/"
    
    & $msBuildPath /target:publish /p:PublishProfile=$publishProfile `
        /p:ApplicationVersion=$version /p:Configuration=Release `
        /p:PublishDir=$publishDir /p:PublishUrl=$publishDir `
        /p:InstallUrl=$installUrl $msBuildVerbosityArg

    # Measure publish size.
    $publishSize = (Get-ChildItem -Path "$publishDir/Application Files" -Recurse |
        Measure-Object -Property Length -Sum).Sum / 1Mb
    Write-Output ("Published size: {0:N2} MB" -f $publishSize)
}
finally {
    Pop-Location
}

if ($BuildOnly) {
    Write-Output "Build finished."
    exit
}

# Clone `gh-pages` branch.
$ghPagesDir = "gh-pages"
if (-Not (Test-Path $ghPagesDir)) {
    git clone $(git config --get remote.origin.url) -b gh-pages `
        --depth 1 --single-branch $ghPagesDir
}

Push-Location $ghPagesDir
try {
    # Remove previous application files.
    Write-Output "Removing previous files if necessary..."
    Write-Output "Checking $ring/Application Files"
    if (Test-Path "$ring/Application Files") {
        Write-Output "Removing $ring/Application Files..."
        Remove-Item -Path "$ring/Application Files" -Recurse
    }
    else
    {
        "Don't need to remove $ring/Application Files"
    }

    Write-Output "Checking $ring/$appName.application"
    if (Test-Path "$ring/$appName.application") {
        Write-Output "Removing $ring/$appName.application..."
        Remove-Item -Path "$ring/$appName.application"
    }
    else
    {
        "Don't need to remove $ring/$appName.application"
    }

    # Copy new application files.
    $destination = "./$ring/ApplicationFiles"
    Write-Output "Copying new files..."
    Write-Output "From: ../$outDir/Application Files and ../$outDir/$appName.application"
    Write-Output "To: $destination"
    Copy-Item -Path "../$outDir/Application Files","../$outDir/$appName.application" `
        -Destination $destination -Recurse

    # Stage and commit.
    Write-Output "Staging..."
    git add -A
    Write-Output "Committing..."
    git commit -m "Update to v$version"

    # Push.
    git push
} finally {
    Pop-Location
}

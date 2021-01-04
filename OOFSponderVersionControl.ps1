<#
.Description
Set-OOFSponderVersion is a version manipulation script for OOFSponder releases.
It automatically updates version-related fields in dependent projects (both *.csproj and AssemblyInfo.cs), and switches between Deployment rings (Alpha/Insider/Production)
.Parameter Version
Target version that you want to update the build to. Should be in format of "1.1.1.12"
.Parameter Ring
Deployment ring. Can be Insider or Production
.Parameter Commit
Automatically commit changes with Git after file modification is made.
.Parameter NoCommit
Do not commit changes with Git, and don't ask for a confirmation.
.Synopsis
Sets OOFSponder version and deployment ring.
.Example
Set-OOFSponderVersion.ps1 1.1.1.30
Update version to 1.1.1.30, and autodetect deployment ring (will be Alpha). Ask for confirmation to commit change with Git.
.Example
Set-OOFSponderVersion.ps1 -Version 1.1.1.31 -NoCommit
Update version to 1.1.1.31. Odd build numbers can be Alpha or Insider, so ask to select a deployment ring. Do not commit the change with Git, and don't ask about it.
.Example
Set-OOFSponderVersion.ps1 -Version 1.1.1.31 -Ring Alpha -Commit
Update version to 1.1.1.31, and set deployment ring to Alpha. Automatically commit change with Git.
.Example
Set-OOFSponderVersion.ps1
Interactive mode. Will display current version, prompt for new one, ask for deployment ring if it is not Production, and ask for Git commit confirmation.
#> 

param (
    [string]$Version,
    [ValidateSet("Alpha", "Insider", "Production")]
    [string]$Ring,
    [switch]$Commit,
    [switch]$NoCommit
)
$OOFSponderLocalPath = "$($pwd)\OOFScheduling\OOFSponder.csproj"
[xml]$doc = Get-Content -Path $OOFSponderLocalPath
$currentVersion = ([string]$doc.Project.PropertyGroup.ApplicationVersion).Trim()
$currentRevision = ([string]$doc.Project.PropertyGroup.ApplicationRevision).Trim()
$installUrl = ([string]$doc.Project.PropertyGroup.InstallUrl).Trim()
$currentRing = (Get-Culture).TextInfo.ToTitleCase( (Select-String '([^\/]+\/?$)' -Input $installUrl).Matches.Value.TrimEnd('/') )

##only even numbered builds can be production - safety check
$expectedProd = $currentRevision % 2 -ne 0
$isProduction = $currentRing -eq "Production"
Write-Host "Current version: $currentVersion | r$currentRevision; ring: $currentRing"

if ($expectedProd -ne $isProduction) {
    if ($expectedProd -eq $true) {
        $expectedRing = "Production" 
    }
    else {
        $expectedRing = "Alpha/Insider"
    }
    Write-Warning "Revision/ring mismatch: expected $expectedRing build, this is $currentRing"
}

while ($Version -notmatch '(^[\d]+\.[\d]+\.[\d]+\.[\d]+$)') {
    [string]$Version = Read-Host "New Application version"
}

$Revision = (Select-String '([\d]+$)' -Input $Version).Matches.Value
Write-Host "Updating version to $Version"

if ($Revision % 2 -eq 0) {
    if ($Ring -eq "Alpha" -or $Ring -eq "Insider") {
        Write-Error "Alpha/Insider rings are not allowed for this revision"
        Break
    }
    $Ring = "Production"
}
elseif ($Ring -eq "Production") {
    Write-Error "Production ring is not allowed for this revision"
    Break
}
elseif ($Ring -eq "") {
    $Ring = "Alpha"
    if ($host.UI.PromptForChoice("Select deployment ring", "", [System.Management.Automation.Host.ChoiceDescription[]] @("&Alpha", "&Insider"), 0) -eq 1) {
        $Ring = "Insider"
    }
}
$lcRing = $Ring.ToLower()

##Publish URL
$doc.Project.PropertyGroup[0].PublishUrl = "C:\Users\Public\OOFSponder$Ring\"

##UpdateUrl and InstallUrl
switch ($lcRing) {
    { $lcRing -eq "production" }
        {
            $doc.Project.PropertyGroup[0].InstallUrl = "https://oofsponderdeploy.blob.core.windows.net/install/"
            $doc.Project.PropertyGroup[0].UpdateUrl = "https://oofsponderdeploy.blob.core.windows.net/install/"
            break
        }
    { $lcRing -eq "insider" }
        {
            $doc.Project.PropertyGroup[0].InstallUrl = "https://oofsponderdeploy.blob.core.windows.net/insider/"
            $doc.Project.PropertyGroup[0].UpdateUrl = "https://oofsponderdeploy.blob.core.windows.net/insider/"
            break
        }
   default  ##Default to Alpha
        {
            $doc.Project.PropertyGroup[0].InstallUrl = "https://oofsponderdeploy.blob.core.windows.net/alpha/"
            $doc.Project.PropertyGroup[0].UpdateUrl = "https://oofsponderdeploy.blob.core.windows.net/alpha/"
            break
        }
}

$doc.Save($OOFSponderLocalPath)
Write-Host -ForegroundColor Green "Deployment ring set to ""$Ring"""

Write-Host -NoNewline "Updating csproj files..."
$AssemblyFiles = Get-ChildItem . *.csproj -rec
foreach ($file in $AssemblyFiles) {
    $convPath = Convert-Path -Path $file.PSPath
    [xml]$doc = Get-Content -Path $file.PSPath
    [bool]$modified = $false
    $currentDepVersion = ([string]$doc.Project.PropertyGroup.ApplicationVersion).Trim()
    if ($currentDepVersion -ne $currentVersion) {
        $doc.Project.PropertyGroup[0].ApplicationVersion = $Version
        $modified = $true
    }
    $currentDepRevision = ([string]$doc.Project.PropertyGroup.ApplicationRevision).Trim()
    if ($currentDepRevision -ne $currentRevision) {
        $doc.Project.PropertyGroup[0].ApplicationRevision = $Revision
        $modified = $true
    }
    if ($modified) {
        $doc.Save("$convPath")
    }
}
Write-Host -ForegroundColor Green " Done."

Write-Host -NoNewline "Updating AssemblyInfo..."
$AssemblyFiles = Get-ChildItem . AssemblyInfo.cs -rec
foreach ($file in $AssemblyFiles) {
    (Get-Content $file.PSPath) | ForEach-Object {
        if ($_ -match "\[assembly: AssemblyVersion\(""$currentVersion""\)\]") {
            '[assembly: AssemblyVersion("{0}")]' -f $Version
        }
        elseif ($_ -match "\[assembly: AssemblyFileVersion\(""$currentVersion""\)\]") {
            '[assembly: AssemblyFileVersion("{0}")]' -f $Version
        }
        else {
            $_
        }
    } | Set-Content $file.PSPath -Encoding UTF8
}
Write-Host -ForegroundColor Green " Done."
if (!$NoCommit) {
    if ($Commit -or $host.UI.PromptForChoice("Would you like to commit the change now?", "", [System.Management.Automation.Host.ChoiceDescription[]] @("&Yes", "&No"), 0) -eq 0) {
        git commit -a -m "$Version $Ring release"
    }
}
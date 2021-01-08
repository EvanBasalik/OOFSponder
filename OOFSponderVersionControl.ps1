<#
.Description
Set-OOFSponderVersion is a version manipulation script for OOFSponder releases.
It automatically updates version-related fields in dependent projects (both *.csproj and AssemblyInfo.cs), and switches between Deployment rings (Alpha/Insider/Production)
.Parameter Version
Target version that you want to update the build to. Should be in format of "1.1"
.Parameter Ring
Deployment ring. Can be Insider or Production
.Parameter Commit
Automatically commit changes with Git after file modification is made.
.Parameter NoCommit
Do not commit changes with Git, and don't ask for a confirmation.
.Synopsis
Sets OOFSponder version and deployment ring.
.Example
Set-OOFSponderVersion.ps1 1.1
Update version to just a single increment on the Revision, autodetect deployment ring (will be Alpha). Ask for confirmation to commit change with Git.
.Example
Set-OOFSponderVersion.ps1 -Version 1.1 -NoCommit
Update version to 1.1.0.0, ask to select a deployment ring. Do not commit the change with Git, and don't ask about it.
.Example
Set-OOFSponderVersion.ps1
Interactive mode. Will display current version, prompt for new one, ask for deployment ring if it is not Production, and ask for Git commit confirmation.
#> 

param (
    [version]$Version,
    [ValidateSet("Alpha", "Insider", "Production")]
    [string]$Ring,
    [switch]$Commit,
    [switch]$NoCommit
)
$OOFSponderLocalPath = "$($pwd)\OOFScheduling\OOFSponder.csproj"
[xml]$doc = Get-Content -Path $OOFSponderLocalPath
[version]$currentVersion = ([string]$doc.Project.PropertyGroup.ApplicationVersion).Trim()
[int]$currentRevision = ([string]$doc.Project.PropertyGroup.ApplicationRevision).Trim()
$installUrl = ([string]$doc.Project.PropertyGroup.InstallUrl).Trim()
$currentRing = (Get-Culture).TextInfo.ToTitleCase( (Select-String '([^\/]+\/?$)' -Input $installUrl).Matches.Value.TrimEnd('/') )
Write-Host "Current version: $currentVersion | r$currentRevision; ring: $currentRing"

Write-Warning "New version not specified. Calculating new version based on ring..."
#get the Ring and based on the Ring, calculate the new version
if ($Ring -eq "") {
    $result = $host.UI.PromptForChoice("Select deployment ring", "", [System.Management.Automation.Host.ChoiceDescription[]] @("&Alpha", "&Insider", "&Production"),0)
}
switch ($result) 
{
    "1"
    {
        $Ring = "Insider"
        if (![string]::IsNullOrEmpty($Version)) {
            Write-Warning "Insider ring cannot have a specified version. Changing to nochange.nochange.increment.0"
        }

        #first, grab the existing version
        [version]$Version = $currentVersion

        #increment Minor version
        #set the Minor revision to 0
        $Version = [version]::new($Version.Major,$Version.Minor, $Version.Build+1, 0)
    }
    "2"
    {
        $Ring = "Production"
        if (($Version -notmatch '(^[\d]+\.[\d]+)') -and ($Ring -eq "Production")) 
        {
            [string]$Version = Read-Host "New Application version"
        }
        #can only specify the first two digits
        if (!($Version -match '(^[\d]+\.[\d]+$)')) 
        {
            Write-Error "Version must be in the form of x.y"
            Exit
        }
        $Version += ".0.0"
        [version]$Version = $Version
    }
    Default
    {
        $Ring = "Alpha"
        if (![string]::IsNullOrEmpty($Version)) {
            Write-Warning "Alpha ring cannot have a specified version. Changing to autoincrement of minor revision"
        }
        [version]$Version = $currentVersion
        $Version = [version]::new($Version.Major,$Version.Minor, $Version.Build, $Version.Revision + 1)
    }
}

Write-Host "Version will be set to $($Version.ToString())"

$lcRing = $Ring.ToLower()

##We only want to update OOFSponder's project - dependencies get modified independently
##Leave the logic in to grab everything in case the logic changes in the future
Write-Host -NoNewline "Updating OOFSponder.csproj..."
$AssemblyFiles = Get-ChildItem . *.csproj -rec
foreach ($file in $AssemblyFiles) {
    if ($file.Name -contains "OOFSponder.csproj")
    {
        $convPath = Convert-Path -Path $file.PSPath
        [xml]$doc = Get-Content -Path $file.PSPath
        [bool]$modified = $false
        $currentDepVersion = ([string]$doc.Project.PropertyGroup.ApplicationVersion).Trim()

        ##make sure the existing version is less than the new version
        ##if not, bail
        if (([version]$currentDepVersion) -lt [version]$Version) {
            $doc.Project.PropertyGroup[0].ApplicationVersion = $Version.ToString()
            $modified = $true
        }
        else 
        {
            Write-Host "New version less than or equal to old version. Please check the new version is correct" -ForegroundColor Red
            Exit
        }

        if ($modified) {
            $doc.Save($convPath)
        }
    }
}
Write-Host -ForegroundColor Green " Done."

Write-Host -NoNewline "Updating AssemblyInfo..."
$AssemblyFiles = Get-ChildItem . AssemblyInfo.cs -rec
foreach ($file in $AssemblyFiles) {
    if ($file.FullName.Contains("OOFScheduling"))
    {
        $fileContent=Get-Content $file.PSPath
        ($fileContent) | ForEach-Object {

            Write-Verbose "Evaluating $($_)"

            ##need to include a catch for super old versions where this value was either never set or set manually
            ##for the rest, just make sure existing = current, then bump to new
            if (($_ -match "\[assembly: AssemblyVersion\(""$currentVersion""\)\]") -or ($_ -match "\[assembly: AssemblyVersion\(""1.0.*""\)\]")) {
                if ($_ -notmatch "//") {
                    '[assembly: AssemblyVersion("{0}")]' -f $Version.ToString()
                }
                else {
                    $_
                }

            }
            elseif (($_ -match "\[assembly: AssemblyFileVersion\(""$currentVersion""\)\]") -or ($_ -match "\[assembly: AssemblyFileVersion\(""1.0.0.0""\)\]")) {
                if ($_ -notmatch "//") {
                    '[assembly: AssemblyFileVersion("{0}")]' -f $Version.ToString()
                }
                else {
                    $_
                }
            }
            else {
                $_
            }
        } | Set-Content $file.PSPath -Encoding UTF8
    }
}
Write-Host -ForegroundColor Green " Done."

Write-Host "Updating Publish/Update/Install URLs..." -NoNewline

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

if (!$NoCommit) {
    if ($Commit -or $host.UI.PromptForChoice("Would you like to commit the change now?", "", [System.Management.Automation.Host.ChoiceDescription[]] @("&Yes", "&No"), 0) -eq 0) {
        git commit -a -m "$Version $Ring release"
    }
}
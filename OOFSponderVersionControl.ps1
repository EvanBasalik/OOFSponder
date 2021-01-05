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

if ($Ring -eq "") {
    switch ($host.UI.PromptForChoice("Select deployment ring", "", [System.Management.Automation.Host.ChoiceDescription[]] @("&Alpha", "&Insider", "&Production"),0)) 
    {
        "1"
        {
            $Ring = "Insider"
        }
        "2"
        {
            Ring = "Production"
        }
        Default
        {
            Ring = "Alpha"
        }
    }
}

Write-Host "Updating version to $Version"
while ($Version -notmatch '(^[\d]+\.[\d]+$)') 
{
    [string]$Version = Read-Host "New Application version"

    #validate the inputted version against version rules
    switch ($Ring) {
        "Production" 
        {  
            #
            [version]$Version += ".0.0"
        }
        "Insider" 
        {  
            if ($Version -ne "") {
                Write-Error "Insider ring cannot have a specified version. Changing to nochange.nochange.increment.0"

                #first, grab the existing version
                $Version = $currentVersion

                #increment Minor version
                $Version.Minor = $currentVersion.Minor + 1

                #set the Minor revision to 0
                $Version.MinorRevision = 0
            }
        }
        Default ##Alpha
        {
            if ($Version -ne "") {
                Write-Error "Alpha ring cannot have a specified version. Changing to Revision increment only"
                $Version = $currentVersion
                $Revision = $currentRevision + 1
            }
        }
    }
    Write-Host "Version will be set to $($Version)"

    if ($Revision % 2 -eq 0) {
        if ($Ring -eq "Alpha" -or $Ring -eq "Insider") {
            Write-Error "Alpha/Insider rings are not allowed for this revision"
            Exit
        }
        $Ring = "Production"
    }
    elseif ($Ring -eq "Production") {
        Write-Error "Production ring is not allowed for this revision"
        Break
    }
    elseif ($Ring -eq "") {

    }
}
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
        if ([version]$currentDepVersion -lt [version]$Version) {
            $doc.Project.PropertyGroup[0].ApplicationVersion = $Version
            $modified = $true
        else {
            Write-Host "New version less than or equal to old version. Please check the new version is correct" -ForegroundColor Red
            Exit
            }
        }
        $currentDepRevision = ([string]$doc.Project.PropertyGroup.ApplicationRevision).Trim()
        if ($currentDepRevision -ne $currentRevision) {
            $doc.Project.PropertyGroup[0].ApplicationRevision = $Revision
            $modified = $true
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
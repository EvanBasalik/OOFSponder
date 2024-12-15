
<#
.Description
OOFSponderCoreVersionControl is a version manipulation script for OOFSponder releases.
It automatically update the Version property in the OOFSponderCore project
.Parameter Version
Target version that you want to update the build to. Should be in format of "v1.2.3"
.Synopsis
Sets OOFSponder version in the project file
.Example
OOFSponderCoreVersionControl.ps1 v1.2.3
Update version to 1.2.3
.Example
OOFSponderCoreVersionControl.ps1
Interactive mode. Will display current version, prompt for new one
#>

param (
    [Parameter(Mandatory=$true, Position=0, HelpMessage="Target version?")]
    [string]$Version
)

#force stop on error
Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"

$supportingFiles=$pwd.ToString().replace("\SupportingFiles","")
$OOFSponderLocalPath = "$($supportingFiles)\OOFSponderCore\"

#if "-ring" is on the end of version number, then drop it
if ($Version.Contains("-")) {
    Write-Output "Version number contained ""-xxxx"" - removed"
    $Version = $Version.Split("-")[0]
}


if (($Version.Split("v").Count -eq 2) -and ($Version.Split(".").Count -eq 3) -eq $false) {
    Write-Error "Version must be in the form of v1.2.3"
    Exit
}

#drop the leading "v"
$Version = $Version.Split("v")[1]

##We only want to update OOFSponder's project - dependencies get modified independently
##Leave the logic in to grab everything in case the logic changes in the future
Write-Host -NoNewline "Updating OOFSponderCore.csproj..."
$AssemblyFiles = Get-ChildItem -Path $OOFSponderLocalPath *.csproj -rec
foreach ($file in $AssemblyFiles) {
    if ($file.Name -contains "OOFSponderCore.csproj")
    {
        $convPath = Convert-Path -Path $file.PSPath
        [xml]$doc = Get-Content -Path $file.PSPath
        [bool]$modified = $false
        $currentDepVersion = ([string]$doc.Project.PropertyGroup[0].Version).Trim()
        Write-Output "Current version: $($currentDepVersion)"

        ##make sure the existing version is less than the new version
        ##if not, bail
        if (([version]$currentDepVersion) -lt [version]$Version) {
            $doc.Project.PropertyGroup[0].Version = $Version.ToString()
            $modified = $true
        }
        else 
        {
            Write-Error "New version less than or equal to old version. Please validate the new version is correct"
            Exit
        }

        if ($modified) {
            $doc.Save($convPath)
            Write-Output "Version updated to $($Version)"
        }
        else {
            Write-Error "Unable to update Version"
        }
    }
}

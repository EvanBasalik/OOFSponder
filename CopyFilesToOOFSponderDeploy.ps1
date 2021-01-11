#$localFolder = '$(buildPath)\app.publish\'
$localFolder = "C:\Users\evanba\source\repos\OOFSponder-1\OOFScheduling\bin\Release\app.publish\"
$StorageAccountName = 'oofsponderdeploy'
$ContainerName = 'deploy'
#$ring = '$(deploymentRing)'
$ring = "alpha"
$sasSecretKey = ""

###everything below here gets pasted into the Pipeline action
###everthing above here shouldn't get copied
Write-Host "Leading characters of sasSecretKey = $($sasSecretKey.Substring(0,6))"

Write-Host "Getting Storage Context..." -NoNewline
$ctx =New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $sasSecretKey
Write-Host "Done" -ForegroundColor Green

Write-Host "Getting container $($ContainerName)..." -NoNewline
$container = Get-AzStorageContainer -Name $ContainerName -Context $ctx
Write-Host "Done" -ForegroundColor Green

if ($container) {

        $filesToUpload = Get-ChildItem $localFolder -Recurse -File

        foreach($file in $filesToUpload)
        {
          Write-Host "Processing $($file.Name)"
          $directoryName = $file.Directory.Name
          $filename = $file.Name
        
          $blobName = $ring.ToLower() + "\" + $file.FullName.Replace($localFolder,"")
          Write-Host "New blob $($blobName)"

          write-host "copying $directoryName\$filename to $blobName" -ForegroundColor Yellow

          #$Properties = @{"CacheControl" = "max-age=$maxAge"; "ContentType" = $mimeType}
          #Set-AzStorageBlobContent -File $file.FullName -Container $container.Name -Blob $blobName -Context $ctx -Force -Properties $Properties
          Set-AzStorageBlobContent -Container $container.Name -File $file.FullName -Blob $blobName -Context $ctx -Force
        }
        write-host "All files in $localFolder uploaded to $($container.Name)!"
}
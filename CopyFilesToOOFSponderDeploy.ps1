
$localFolder = 'C:\Users\evanba\source\repos\OOFSponder-1\OOFScheduling\bin\Release\app.publish'
$StorageAccountName = 'oofsponderdeploy'
$ContainerName = 'deploy'
$ring = 'alpha'

##Create a storage context
$sasKey = ''
Write-Host "sasKey = $($sasKey)"

Write-Host "Getting Storage Context..." -NoNewline
$ctx =New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $sasKey
Write-Host "Done" -ForegroundColor Green

Write-Host "Getting container $($ContainerName)..." -NoNewline
$container = Get-AzStorageContainer -Name $ContainerName -Context $ctx
Write-Host "Done" -ForegroundColor Green

if ($container) {

        $filesToUpload = Get-ChildItem $localFolder -Recurse -File

        foreach($file in $filesToUpload)
        {
          Write-Host "Processing " + $file.Name
          $directoryName = $file.Directory.Name
          $filename = $file.Name
        
          $blobName = $ring + "/" + $destfolder + $file.FullName.Substring($file.FullName.IndexOf($localFolder) + $localFolder.Length+1)
    
          write-host "copying $directoryName\$filename to $blobName"

          #$Properties = @{"CacheControl" = "max-age=$maxAge"; "ContentType" = $mimeType}
          #Set-AzStorageBlobContent -File $file.FullName -Container $container.Name -Blob $blobName -Context $ctx -Force -Properties $Properties
          Set-AzStorageBlobContent -Container $container.Name -File $file.FullName -Blob $blobName -Context $ctx -Force
        }
        write-host "All files in $localFolder uploaded to $($container.Name)!"
}
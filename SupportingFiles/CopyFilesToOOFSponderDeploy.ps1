$localFolder = '$(buildPath)\app.publish\'
#$localFolder = "C:\Users\evanba\source\repos\EvanBasalik\OOFSponder\OOFScheduling\bin\Release\app.publish\"
$StorageAccountName = 'oofsponderdeploy'
$ring = '$(deploymentRing)'
#$ring = "alpha"

#if running in DevOps, use the Pipeline variable. Otherwise, use the local file
if ($TF_BUILD) 
    {$sasSecretKey = $(OOFSponderDeployKey)} 
else 
    {$sasSecretKey = Get-Content ".\SASKey.ignore"}

###everthing above here shouldn't get copied
###everything below here gets pasted into the Pipeline action
Write-Host "sasSecretKey = $($sasSecretKey.Substring(0,6))..."

Write-Host "Getting Storage Context..." -NoNewline
$ctx = New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $sasSecretKey
Write-Host "Done" -ForegroundColor Green

switch ($ring) {
  #Special case for Production since it uses a different URL scheme
  { $ring -eq "production" }  
    {
      $containerName = "install"
      break
    }
 default  ##Default to just using the ring name
    {
      $containerName = $ring.ToLower()
      break
    }
}

Write-Host "Getting container $($containerName)..." -NoNewline
$container = Get-AzStorageContainer -Name $containerName -Context $ctx
Write-Host "Done" -ForegroundColor Green

if ($container) {

  $filesToUpload = Get-ChildItem $localFolder -Recurse -File

  foreach($file in $filesToUpload)
  {
    Write-Host "Processing $($file.Name)"
    $directoryName = $file.Directory.Name
    $filename = $file.Name
  
    $blobName = $file.FullName.Replace($localFolder,"")
    Write-Host "New blob $($blobName)"

    write-host "copying $directoryName\$filename to $blobName" -ForegroundColor Yellow

    #$Properties = @{"CacheControl" = "max-age=$maxAge"; "ContentType" = $mimeType}
    #Set-AzStorageBlobContent -File $file.FullName -Container $container.Name -Blob $blobName -Context $ctx -Force -Properties $Properties
    Set-AzStorageBlobContent -Container $container.Name -File $file.FullName -Blob $blobName -Context $ctx -Force
  }
  write-host "All files in $localFolder uploaded to $($container.Name)!"
}
Write-Host "Install .NET Core 3 SDK Preview 7"

Get-ChildItem | Write-Host

$env:DOTNET_INSTALL_DIR = "$pwd\.dotnetsdk"

Get-ChildItem $env:DOTNET_INSTALL_DIR | Write-Host

$dotnetPath = "$env:DOTNET_INSTALL_DIR\dotnet.exe"
$fileExists = Test-Path $dotnetPath
Write-Host "Check SDK cache"

If ($fileExists -eq $False) {
    Write-Host "No cache. Loading SDK."
    $urlCurrent = "https://download.visualstudio.microsoft.com/download/pr/41e4c58f-3ac9-43f6-84b6-f57d2135331a/3691b61f15f1f5f844d687e542c4dc72/dotnet-sdk-3.0.100-preview7-012821-win-x64.zip"
    mkdir $env:DOTNET_INSTALL_DIR -Force | Out-Null
    $tempFileCurrent = [System.IO.Path]::GetTempFileName()
    (New-Object System.Net.WebClient).DownloadFile($urlCurrent, $tempFileCurrent)
    Add-Type -AssemblyName System.IO.Compression.FileSystem; [System.IO.Compression.ZipFile]::ExtractToDirectory($tempFileCurrent, $env:DOTNET_INSTALL_DIR)
}

$env:Path = "$env:DOTNET_INSTALL_DIR;$env:Path"
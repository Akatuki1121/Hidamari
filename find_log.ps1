$logPath = Join-Path $env:LOCALAPPDATA "Unity\Editor\Editor.log"

if (Test-Path $logPath) {
    Write-Host "FOUND: $logPath"
    Get-Item $logPath | Select-Object Length, LastWriteTime
} else {
    Write-Host "NOT FOUND: $logPath"
}

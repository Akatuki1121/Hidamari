Add-Type -AssemblyName System.Drawing

$images = @(
    "C:\Users\sou\Unity Program\Hidamari\Assets\Image\IMG_8600.png",
    "C:\Users\sou\Unity Program\Hidamari\Assets\Image\IMG_8601.png",
    "C:\Users\sou\Unity Program\Hidamari\Assets\Image\IMG_8605.png",
    "C:\Users\sou\Unity Program\Hidamari\Assets\Image\リザルト画面.png"
)

foreach ($path in $images) {
    $img = [System.Drawing.Image]::FromFile($path)
    Write-Host "$path : $($img.Width) x $($img.Height)"
    $img.Dispose()
}

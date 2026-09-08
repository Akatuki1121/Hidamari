Add-Type -AssemblyName System.Drawing
$src = [System.Drawing.Image]::FromFile('C:\Users\sou\Unity Program\Hidamari\Assets\Image\IMG_8592.png')
$thumb = New-Object System.Drawing.Bitmap($src, 480, 270)
$thumb.Save('C:\Users\sou\Unity Program\Hidamari\title_thumb.png', [System.Drawing.Imaging.ImageFormat]::Png)
$thumb.Dispose()
$src.Dispose()

Add-Type -AssemblyName System.Drawing
$img = [System.Drawing.Image]::FromFile('C:\Users\sou\Unity Program\Hidamari\Assets\Image\IMG_8592.png')
Write-Output ($img.Width.ToString() + 'x' + $img.Height.ToString())
$img.Dispose()

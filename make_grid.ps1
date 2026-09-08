Add-Type -AssemblyName System.Drawing
$src = [System.Drawing.Image]::FromFile('C:\Users\sou\Unity Program\Hidamari\Assets\Image\IMG_8592.png')
$w = 960
$h = 540
$thumb = New-Object System.Drawing.Bitmap($w, $h)
$g = [System.Drawing.Graphics]::FromImage($thumb)
$g.DrawImage($src, 0, 0, $w, $h)
$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::Red, 1)
$font = New-Object System.Drawing.Font('Arial', 10)
$brush = [System.Drawing.Brushes]::Red
for ($x = 0; $x -lt $w; $x += 100) {
  $g.DrawLine($pen, $x, 0, $x, $h)
  $g.DrawString(($x * 2).ToString(), $font, $brush, $x, 0)
}
for ($y = 0; $y -lt $h; $y += 100) {
  $g.DrawLine($pen, 0, $y, $w, $y)
  $g.DrawString(($y * 2).ToString(), $font, $brush, 0, $y)
}
$thumb.Save('C:\Users\sou\Unity Program\Hidamari\title_grid.png', [System.Drawing.Imaging.ImageFormat]::Png)
$g.Dispose()
$thumb.Dispose()
$src.Dispose()

<#
  Draws Tri-Stone's icon (owner choice 2026-11-18: white "TS" on dark navy with an orange bar) in 6 sizes into
  src\Company.Launcher\tristone.ico, used by TriStone.exe, every window and the Setup files. Run it again only to change
  the icon:  powershell -ExecutionPolicy Bypass -File installer\make_icon.ps1
#>
Add-Type -AssemblyName System.Drawing
$out = Join-Path $PSScriptRoot "..\src\Company.Launcher\tristone.ico"
$preview = Join-Path $env:TEMP "tristone_icon_sizes.png"   # all sizes side by side, to check by eye
$dark = [Drawing.Color]::FromArgb(29, 41, 57)          # the logo's dark text
$orange = [Drawing.ColorTranslator]::FromHtml("#F47A20")

# Icon B (owner choice 2026-11-18): white "TS" on dark navy with an orange bar under it
function Draw([int]$s) {
    $bmp = New-Object Drawing.Bitmap $s, $s, ([Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = "AntiAlias"; $g.PixelOffsetMode = "HighQuality"; $g.TextRenderingHint = "AntiAliasGridFit"
    $g.Clear([Drawing.Color]::Transparent)
    $k = $s / 256.0
    $r = [single](51 * $k); $m = [single]([math]::Max(0, 8 * $k)); $w = [single]($s - 2 * $m)
    $p = New-Object Drawing.Drawing2D.GraphicsPath
    $p.AddArc($m, $m, 2 * $r, 2 * $r, 180, 90); $p.AddArc($m + $w - 2 * $r, $m, 2 * $r, 2 * $r, 270, 90)
    $p.AddArc($m + $w - 2 * $r, $m + $w - 2 * $r, 2 * $r, 2 * $r, 0, 90); $p.AddArc($m, $m + $w - 2 * $r, 2 * $r, 2 * $r, 90, 90)
    $p.CloseFigure(); $g.FillPath((New-Object Drawing.SolidBrush $dark), $p)
    $fmt = New-Object Drawing.StringFormat; $fmt.Alignment = "Center"; $fmt.LineAlignment = "Center"
    $f = New-Object Drawing.Font "Segoe UI", ([single](104 * $k)), ([Drawing.FontStyle]::Bold), ([Drawing.GraphicsUnit]::Pixel)
    $g.DrawString("TS", $f, [Drawing.Brushes]::White, (New-Object Drawing.RectangleF 0, ([single](10 * $k)), $s, ([single](200 * $k))), $fmt)
    $barH = [math]::Max(1, [math]::Round(18 * $k))
    $g.FillRectangle((New-Object Drawing.SolidBrush $orange), [single](64 * $k), [single](182 * $k), [single](128 * $k), [single]$barH)
    $g.Dispose()
    return $bmp
}

$sizes = 256, 64, 48, 32, 24, 16
$sheet = New-Object Drawing.Bitmap 460, 260; $sg = [Drawing.Graphics]::FromImage($sheet); $sg.Clear([Drawing.Color]::White); $x = 0
$pngs = foreach ($s in $sizes) {
    $b = Draw $s
    $sg.DrawImage($b, $x, 0, $s, $s); $x += $s + 6
    $ms = New-Object IO.MemoryStream; $b.Save($ms, [Drawing.Imaging.ImageFormat]::Png); $b.Dispose(); , $ms.ToArray()
}
$sheet.Save($preview); $sg.Dispose(); $sheet.Dispose()

$fs = [IO.File]::Create($out); $bw = New-Object IO.BinaryWriter $fs
$bw.Write([uint16]0); $bw.Write([uint16]1); $bw.Write([uint16]$sizes.Count)
$offset = 6 + 16 * $sizes.Count
for ($i = 0; $i -lt $sizes.Count; $i++) {
    $s = $sizes[$i]; $len = $pngs[$i].Length
    $d = if ($s -ge 256) { 0 } else { $s }
    $bw.Write([byte]$d); $bw.Write([byte]$d); $bw.Write([byte]0); $bw.Write([byte]0)
    $bw.Write([uint16]1); $bw.Write([uint16]32); $bw.Write([uint32]$len); $bw.Write([uint32]$offset); $offset += $len
}
foreach ($p in $pngs) { $bw.Write($p) }
$bw.Close()
"icon: $((Get-Item $out).Length) bytes"

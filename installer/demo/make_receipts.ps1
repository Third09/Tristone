<#
  Test Setup only (owner request 2026-11-17): draws a made-up "scanned" receipt picture for every receipt and old
  delivery in the test copy that only has a paper copy, saves it in the files folder that ships with the Setup, and
  points the row at it, so testers can open and zoom receipts. Every picture says SAMPLE. Run by build.ps1 -FromThisPc
  on the scratch copy (never on the real database).
#>
param(
    [Parameter(Mandatory)][string]$Psql,
    [Parameter(Mandatory)][string]$Database,
    [Parameter(Mandatory)][string]$FilesDir
)
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing

$folder = Join-Path $FilesDir "receipts\sample"
New-Item -ItemType Directory -Force -Path $folder | Out-Null
$culture = [Globalization.CultureInfo]::GetCultureInfo("en-PH")

function Query([string]$sql) {
    $rows = & $Psql -h localhost -U postgres -d $Database -At -F "`t" -c $sql
    if ($LASTEXITCODE -ne 0) { throw "Query failed: $sql" }
    return @($rows | Where-Object { $_ -ne "" } | ForEach-Object { , ($_ -split "`t") })
}
function Exec([string]$sql) {
    & $Psql -h localhost -U postgres -d $Database -q -v ON_ERROR_STOP=1 -c $sql | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Update failed: $sql" }
}
function Peso([decimal]$v) { return "P " + $v.ToString("#,##0.00", $culture) }
function SafeName([string]$s) { return ($s -replace '[^A-Za-z0-9._-]', '_') }

# Draws one receipt. $lines = @(@(text, amount-or-$null), ...)
function Draw([string]$path, [string]$title, [string]$merchant, [string]$number, [string]$date, $lines, [decimal]$total) {
    $w = 560; $h = 430 + 26 * $lines.Count
    $bmp = New-Object Drawing.Bitmap $w, $h
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = "AntiAlias"; $g.TextRenderingHint = "AntiAliasGridFit"
    $g.Clear([Drawing.Color]::FromArgb(252, 250, 244))   # receipt paper
    $big = New-Object Drawing.Font "Consolas", 15, ([Drawing.FontStyle]::Bold)
    $mid = New-Object Drawing.Font "Consolas", 11, ([Drawing.FontStyle]::Bold)
    $txt = New-Object Drawing.Font "Consolas", 10.5
    $small = New-Object Drawing.Font "Consolas", 8.5
    $ink = New-Object Drawing.SolidBrush ([Drawing.Color]::FromArgb(40, 40, 40))
    $center = New-Object Drawing.StringFormat; $center.Alignment = "Center"
    $right = New-Object Drawing.StringFormat; $right.Alignment = "Far"
    $dash = New-Object Drawing.Pen ([Drawing.Color]::FromArgb(150, 150, 150)), 1; $dash.DashStyle = "Dash"

    $y = 24
    $g.DrawString($merchant.ToUpper(), $big, $ink, (New-Object Drawing.RectangleF 20, $y, ($w - 40), 50), $center); $y += 52
    $g.DrawString("Made-up address, Metro Manila", $small, $ink, ($w / 2), $y, $center); $y += 16
    $g.DrawString("TIN 000-000-000-000  (sample)", $small, $ink, ($w / 2), $y, $center); $y += 26
    $g.DrawString($title, $mid, $ink, ($w / 2), $y, $center); $y += 28
    $g.DrawString("No. $number", $txt, $ink, 30, $y); $g.DrawString("Date: $date", $txt, $ink, ($w - 30), $y, $right); $y += 28
    $g.DrawLine($dash, 30, $y, ($w - 30), $y); $y += 12
    foreach ($l in $lines) {
        $g.DrawString([string]$l[0], $txt, $ink, (New-Object Drawing.RectangleF 30, $y, ($w - 200), 24))
        if ($null -ne $l[1]) { $g.DrawString((Peso $l[1]), $txt, $ink, ($w - 30), $y, $right) }
        $y += 26
    }
    $g.DrawLine($dash, 30, $y + 4, ($w - 30), $y + 4); $y += 16
    $g.DrawString("TOTAL", $mid, $ink, 30, $y); $g.DrawString((Peso $total), $mid, $ink, ($w - 30), $y, $right); $y += 40
    $g.DrawString("Received the amount above. Thank you!", $txt, $ink, ($w / 2), $y, $center); $y += 22
    $g.DrawString("__________________________", $txt, $ink, ($w / 2), $y + 24, $center)
    $g.DrawString("Authorized signature", $small, $ink, ($w / 2), $y + 46, $center)

    # SAMPLE stamp across the receipt
    $g.TranslateTransform(($w / 2), ($h / 2)); $g.RotateTransform(-28)
    $stamp = New-Object Drawing.Font "Arial", 64, ([Drawing.FontStyle]::Bold)
    $red = New-Object Drawing.SolidBrush ([Drawing.Color]::FromArgb(45, 200, 30, 30))
    $both = New-Object Drawing.StringFormat; $both.Alignment = "Center"; $both.LineAlignment = "Center"
    $g.DrawString("SAMPLE", $stamp, $red, 0, 0, $both)
    $g.ResetTransform()

    $bmp.Save($path, [Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
}

$count = 0
# ----- receipts kept as paper only -----
foreach ($r in (Query "SELECT receipt_id, receipt_no, to_char(purchase_date,'Mon DD, YYYY'), merchant, amount, category FROM receipts WHERE physical_only AND NOT no_receipt AND receipt_no IS NOT NULL")) {
    $id, $no, $date, $merchant, $amount, $category = $r
    $amount = [decimal]::Parse($amount, [Globalization.CultureInfo]::InvariantCulture)
    $title = if ($no -match '^OR') { "OFFICIAL RECEIPT" } elseif ($no -match '^SI') { "SALES INVOICE" } elseif ($no -match '^BILL') { "STATEMENT OF ACCOUNT" } else { "ACKNOWLEDGEMENT RECEIPT" }
    $net = [math]::Round($amount / 1.12, 2)
    $lines = @(, @("$category", $net)) + @(, @("VAT 12%", ($amount - $net)))
    $file = "receipt_" + (SafeName $no) + ".png"
    Draw (Join-Path $folder $file) $title $merchant $no $date $lines $amount
    $rel = "receipts\sample\" + $file
    Exec "UPDATE receipts SET physical_only = FALSE, filed_location = NULL, file_path = '$rel', original_name = '$file' WHERE receipt_id = $id"
    $count++
}
# ----- old deliveries kept as paper only -----
foreach ($r in (Query "SELECT d.delivery_id, d.receipt_no, to_char(d.delivered_on,'Mon DD, YYYY'), v.name, m.name, m.unit, d.quantity, d.unit_price FROM deliveries d JOIN vendors v USING (vendor_id) JOIN materials m USING (material_id) WHERE d.physical_only")) {
    $id, $no, $date, $vendor, $material, $unit, $qty, $price = $r
    $qty = [decimal]::Parse($qty, [Globalization.CultureInfo]::InvariantCulture)
    $price = [decimal]::Parse($price, [Globalization.CultureInfo]::InvariantCulture)
    $total = [math]::Round($qty * $price, 2)
    $title = if ($no -match '^SI') { "SALES INVOICE" } else { "DELIVERY RECEIPT" }
    $lines = @(, @("$material", $null)) + @(, @(("  " + $qty.ToString("#,##0.##") + " $unit x " + (Peso $price)), $total))
    $file = "delivery_" + (SafeName $no) + ".png"
    Draw (Join-Path $folder $file) $title $vendor $no $date $lines $total
    $rel = "receipts\sample\" + $file
    Exec "UPDATE deliveries SET physical_only = FALSE, filed_location = NULL, receipt_file_path = '$rel' WHERE delivery_id = $id"
    $count++
}
Write-Host "Made-up receipt pictures: $count"

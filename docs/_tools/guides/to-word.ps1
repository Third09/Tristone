# Converts every HTML file made by build-guides.js into a .docx in TriStone\docs.
$html = Join-Path $PSScriptRoot "html"
$docs = Resolve-Path (Join-Path $PSScriptRoot "..\..")
$w = New-Object -ComObject Word.Application; $w.Visible = $false
try {
  foreach ($f in Get-ChildItem $html -Filter *.html) {
    $out = Join-Path $docs ($f.BaseName + ".docx")
    $doc = $w.Documents.Open($f.FullName, $false, $true)
    $doc.SaveAs2([string]$out, 16)   # 16 = Word document (.docx)
    $doc.Close($false)
    "docx: $out"
  }
} finally { $w.Quit() }

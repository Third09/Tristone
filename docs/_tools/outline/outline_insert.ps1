# Helpers for editing the Project Outline through Word.
# Dot-source it:  . .\outline_insert.ps1   then use Open-Outline / Insert-After / Replace-Para / Close-Outline.

function Open-Outline([string]$File) {
  $script:w = New-Object -ComObject Word.Application; $script:w.Visible = $false
  $script:doc = $script:w.Documents.Open($File); $script:doc.TrackRevisions = $false
}

function Find-Para([string]$Anchor) {
  foreach ($p in $script:doc.Paragraphs) { if ($p.Range.Text.Trim().StartsWith($Anchor)) { return $p } }
  throw "anchor not found: $Anchor"
}

# Adds new paragraphs right after the anchor paragraph (same style / list level). Never overwrites.
function Insert-After([string]$Anchor, [string[]]$Lines, [int[]]$Levels) {
  $p = Find-Para $Anchor
  # Typing paragraph breaks at the end of the anchor splits it, so every new paragraph keeps the anchor's bullet and style.
  # All lines go in with one insert; inserting them one by one shifts Word's paragraph references and scrambles the order.
  $r = $p.Range; $r.MoveEnd(1, -1) | Out-Null; $r.Collapse(0)
  $r.InsertAfter("`r" + ($Lines -join "`r"))
  # Optional bullet levels (1 = top level), one per line; 0 keeps the anchor's level.
  # ListLevelNumber can't be set from PowerShell, so step the bullet in or out until it matches.
  # Find each new paragraph by its text (Paragraph.Next() is unreliable from PowerShell).
  if ($Levels) {
    for ($k = 0; $k -lt $Lines.Count; $k++) { if ($Levels[$k]) {
      $lf = (Find-Para $Lines[$k]).Range.ListFormat
      for ($n = 0; $n -lt 9 -and $lf.ListLevelNumber -gt $Levels[$k]; $n++) { $lf.ListOutdent() }
      for ($n = 0; $n -lt 9 -and $lf.ListLevelNumber -lt $Levels[$k]; $n++) { $lf.ListIndent() }
    } }
  }
  "inserted $($Lines.Count) after: $Anchor"
}

# Replaces the text of the paragraph that starts with Anchor (keeps its formatting).
function Replace-Para([string]$Anchor, [string]$Text) {
  $r = (Find-Para $Anchor).Range; $r.MoveEnd(1, -1) | Out-Null; $r.Text = $Text
  "replaced: $Anchor"
}

function Close-Outline([switch]$NoSave) {
  try { if ($NoSave) { $script:doc.Close($false) } else { $script:doc.Save(); $script:doc.Close() } } finally { $script:w.Quit() }
}

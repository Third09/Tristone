param([string]$Docx, [string]$OutDir, [int[]]$Pages)
$w = New-Object -ComObject Word.Application; $w.Visible = $false
try { $doc = $w.Documents.Open($Docx, $false, $true); "pages: " + $doc.ComputeStatistics(2); $doc.ExportAsFixedFormat("$OutDir\guide.pdf", 17); $doc.Close($false) } finally { $w.Quit() }
Add-Type -AssemblyName System.Runtime.WindowsRuntime
$null = [Windows.Storage.StorageFile,Windows.Storage,ContentType=WindowsRuntime]
$null = [Windows.Data.Pdf.PdfDocument,Windows.Data.Pdf,ContentType=WindowsRuntime]
$null = [Windows.Storage.Streams.InMemoryRandomAccessStream,Windows.Storage.Streams,ContentType=WindowsRuntime]
$asTask = ([System.WindowsRuntimeSystemExtensions].GetMethods() | ? { $_.Name -eq 'AsTask' -and $_.GetParameters().Count -eq 1 -and $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncOperation`1' })[0]
$asTaskA = ([System.WindowsRuntimeSystemExtensions].GetMethods() | ? { $_.Name -eq 'AsTask' -and $_.GetParameters().Count -eq 1 -and $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncAction' })[0]
function Await($op, $t) { $task = $asTask.MakeGenericMethod($t).Invoke($null, @($op)); $task.Wait(); $task.Result }
$file = Await ([Windows.Storage.StorageFile]::GetFileFromPathAsync("$OutDir\guide.pdf")) ([Windows.Storage.StorageFile])
$pdf = Await ([Windows.Data.Pdf.PdfDocument]::LoadFromFileAsync($file)) ([Windows.Data.Pdf.PdfDocument])
foreach ($n in $Pages) { if ($n -gt $pdf.PageCount) { continue }
  $page = $pdf.GetPage($n - 1); $s = New-Object Windows.Storage.Streams.InMemoryRandomAccessStream
  $asTaskA.Invoke($null, @($page.RenderToStreamAsync($s))).Wait()
  $net = [System.IO.WindowsRuntimeStreamExtensions]::AsStreamForRead($s); $fs = [IO.File]::Create("$OutDir\p$n.png"); $net.CopyTo($fs); $fs.Close() }

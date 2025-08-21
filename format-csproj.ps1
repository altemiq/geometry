$ScriptDir = Split-Path $script:MyInvocation.MyCommand.Path

Get-ChildItem -Path $ScriptDir -Include *.*proj,*.props,*.targets -Exclude *_wpftmp.*,*.g.props,*.g.targets -Recurse | ForEach-Object {
    Write-Host "Processing $($_.Name)"
    $originalFileHash = Get-FileHash $_.FullName
    $temp = New-TemporaryFile

    Get-Content $_.FullName | ForEach-Object { $_.TrimEnd() } | Out-File -FilePath $temp -Encoding UTF8BOM

    $tempFileHash = Get-FileHash $temp

    if ($originalFileHash.Hash -ne $tempFileHash.Hash) {
      Copy-Item -Path $temp -Destination $_.FullName
    }

    Remove-Item $temp
}
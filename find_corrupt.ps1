$lines = Get-Content "d:\sora-hr-frontend\user\index.html"
for ($i = 0; $i -lt $lines.Length; $i++) {
    $line = $lines[$i]
    if ($line -match 'dY' -or $line -match '') {
        Write-Host "Line" ($i + 1) ": $line"
    }
}

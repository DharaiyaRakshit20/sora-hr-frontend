$content = Get-Content -Raw -Encoding UTF8 'd:\sora-hr-frontend\super-admin\index.html'

# Extract script blocks
$regex = [regex]'(?s)<script(?:\s+[^>]*)?>(.*?)</script>'
$matches = $regex.Matches($content)
Write-Host "Found $($matches.Count) script tags"

$i = 0
foreach ($m in $matches) {
    $i++
    $script = $m.Groups[1].Value
    # Count braces, parens, brackets outside strings
    $curly = 0; $paren = 0; $square = 0
    $inStr = $false; $strChar = ''
    $inBlockComment = $false; $inLineComment = $false
    
    $len = $script.Length
    for ($j = 0; $j -lt $len; $j++) {
        $ch = $script[$j]
        $next = if ($j + 1 -lt $len) { $script[$j+1] } else { '' }

        if ($inLineComment) {
            if ($ch -eq "`n") { $inLineComment = $false }
            continue
        }
        if ($inBlockComment) {
            if ($ch -eq '*' -and $next -eq '/') { $inBlockComment = $false; $j++ }
            continue
        }
        if ($inStr) {
            if ($ch -eq '\') { $j++; continue }
            if ($ch -eq $strChar) { $inStr = $false }
            continue
        }

        if ($ch -eq '/' -and $next -eq '/') { $inLineComment = $true; $j++; continue }
        if ($ch -eq '/' -and $next -eq '*') { $inBlockComment = $true; $j++; continue }
        if ($ch -eq '"' -or $ch -eq "'" -or $ch -eq '`') { $inStr = $true; $strChar = $ch; continue }

        if ($ch -eq '{') { $curly++ }
        elseif ($ch -eq '}') { $curly-- }
        elseif ($ch -eq '(') { $paren++ }
        elseif ($ch -eq ')') { $paren-- }
        elseif ($ch -eq '[') { $square++ }
        elseif ($ch -eq ']') { $square-- }
    }
    Write-Host "Script #$i -> Curly: $curly, Paren: $paren, Square: $square"
}

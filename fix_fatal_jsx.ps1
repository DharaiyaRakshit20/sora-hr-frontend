$filePath = "d:\sora-hr-frontend\user\index.html"
$content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)

# 1. Add error boundary script in <head> for visible debugging
$headMarker = "</head>"
$errorScript = @"
  <script>
    window.onerror = function(msg, url, line, col, error) {
      console.error("CRITICAL JS ERROR:", msg, "at line:", line, "col:", col, error);
      var errDiv = document.getElementById('debug-error-box');
      if (!errDiv) {
        errDiv = document.createElement('div');
        errDiv.id = 'debug-error-box';
        errDiv.style = 'position:fixed; top:10px; left:10px; right:10px; z-index:99999; background:#2d1215; color:#fca5a5; border:1px solid #f87171; border-radius:12px; padding:16px; font-family:monospace; font-size:12px; box-shadow:0 10px 25px rgba(0,0,0,0.5);';
        document.body.appendChild(errDiv);
      }
      errDiv.innerHTML = '<strong style="color:#ef4444; font-size:14px;">App Script Error:</strong><br/>' + msg + '<br/><span style="color:#f59e0b;">Line: ' + line + ', Col: ' + col + '</span>';
    };
  </script>
</head>
"@

if (-not $content.Contains("debug-error-box")) {
    $content = $content.Replace($headMarker, $errorScript)
    Write-Host "Added global error boundary script to head"
}

# 2. Fix the line with fatal broken `<` in DashboardTab
# Line: Hello, {currentUser.name}! dY`<
$lines = $content -split "`r`n"
for ($i = 0; $i -lt $lines.Length; $i++) {
    if ($lines[$i] -match 'Hello, \{currentUser\.name\}!') {
        $lines[$i] = '                <h2 className="text-xl sm:text-2xl font-black text-white">Hello, {currentUser.name}!</h2>'
        Write-Host "Fixed line ($i + 1): greeting h2"
    }
    if ($lines[$i] -match 'preview:.*BKC Campus') {
        $lines[$i] = '          preview: "Geofence: Within BKC Campus (42m) - Verified"'
        Write-Host "Fixed line ($i + 1): preview 1"
    }
    if ($lines[$i] -match 'preview:.*Casual') {
        $lines[$i] = '          preview: "Balances: 8 Casual - 5 Sick - 12 Privilege"'
        Write-Host "Fixed line ($i + 1): preview 2"
    }
    if ($lines[$i] -match 'preview:.*Salary') {
        $lines[$i] = '          preview: "Net Salary: Rs 98,540 / month"'
        Write-Host "Fixed line ($i + 1): preview 3"
    }
    if ($lines[$i] -match 'Enforced') {
        $lines[$i] = '              <span className="px-2 py-0.5 rounded-full bg-emerald-500/10 text-emerald-400 text-[10px] font-bold border border-emerald-500/20">Enforced</span>'
        Write-Host "Fixed line ($i + 1): Enforced badge"
    }
    if ($lines[$i] -match 'Active' -and $lines[$i] -match 'bg-emerald-500/15') {
        $lines[$i] = '              <span className="px-2 py-0.5 rounded-md bg-emerald-500/15 text-emerald-400 text-[10px] font-bold flex-shrink-0">Active</span>'
        Write-Host "Fixed line ($i + 1): Active badge"
    }
    if ($lines[$i] -match 'In Range \(28m\)') {
        $lines[$i] = '              <span className="px-2 py-0.5 rounded-md bg-emerald-500/15 text-emerald-400 text-[10px] font-bold flex-shrink-0">In Range (28m)</span>'
        Write-Host "Fixed line ($i + 1): In Range badge"
    }
}

$newContent = $lines -join "`r`n"
[System.IO.File]::WriteAllText($filePath, $newContent, [System.Text.Encoding]::UTF8)

# Sync to public/user/index.html
$publicPath = "d:\sora-hr-frontend\public\user\index.html"
if (Test-Path "d:\sora-hr-frontend\public\user") {
    [System.IO.File]::WriteAllText($publicPath, $newContent, [System.Text.Encoding]::UTF8)
    Write-Host "Synced to public/user/index.html"
}

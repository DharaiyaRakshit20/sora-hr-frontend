# SORA HR - Standalone Mobile Wi-Fi Server
$Port = 8080
$RootPath = $PSScriptRoot

# Detect Local IPv4
$LocalIP = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object { 
    $_.InterfaceAlias -notmatch "Loopback|vEthernet" -and $_.IPAddress -notmatch "^169\." 
} | Select-Object -First 1).IPAddress

if (-not $LocalIP) {
    $LocalIP = "192.168.31.192"
}

Write-Host "========================================================" -ForegroundColor Amber
Write-Host "   SORA HR PLATFORM — MOBILE PHONE SERVER" -ForegroundColor Yellow
Write-Host "========================================================" -ForegroundColor Amber
Write-Host ""
Write-Host "1. Apne phone aur computer ko SAME Wi-Fi ya Mobile Hotspot se connect karein." -ForegroundColor White
Write-Host "2. Apne Phone ke Chrome ya Safari browser me ye link kholein:" -ForegroundColor Cyan
Write-Host ""
Write-Host "   👉 http://${LocalIP}:${Port}/user/" -ForegroundColor Green -BackgroundColor Black
Write-Host ""
Write-Host "Admin Portal Link:" -ForegroundColor Gray
Write-Host "   👉 http://${LocalIP}:${Port}/admin/" -ForegroundColor Gray
Write-Host ""
Write-Host "App ko phone me install karne ke liye:" -ForegroundColor White
Write-Host "   • Chrome me: 3 dots menu -> 'Add to Home screen' / 'Install App'" -ForegroundColor Yellow
Write-Host "   • Safari (iPhone) me: Share icon -> 'Add to Home Screen'" -ForegroundColor Yellow
Write-Host "========================================================" -ForegroundColor Amber
Write-Host "Server listening on http://0.0.0.0:${Port}... (Press Ctrl+C to stop)" -ForegroundColor DarkGray
Write-Host ""

$listener = New-Object System.Net.Sockets.TcpListener([System.Net.IPAddress]::Any, $Port)
$listener.Start()

$mimeTypes = @{
    ".html" = "text/html; charset=utf-8"
    ".htm"  = "text/html; charset=utf-8"
    ".js"   = "application/javascript; charset=utf-8"
    ".css"  = "text/css; charset=utf-8"
    ".svg"  = "image/svg+xml"
    ".png"  = "image/png"
    ".jpg"  = "image/jpeg"
    ".jpeg" = "image/jpeg"
    ".json" = "application/json"
    ".ico"  = "image/x-icon"
}

try {
    while ($true) {
        $client = $listener.AcceptTcpClient()
        $stream = $client.GetStream()
        $reader = New-Object System.IO.StreamReader($stream)
        
        $requestLine = $reader.ReadLine()
        if ([string]::IsNullOrEmpty($requestLine)) {
            $client.Close()
            continue
        }

        $tokens = $requestLine -split ' '
        if ($tokens.Length -lt 2) {
            $client.Close()
            continue
        }

        $rawUrl = $tokens[1]
        $cleanPath = $rawUrl.Split('?')[0]
        $cleanPath = [System.Uri]::UnescapeDataString($cleanPath)

        if ($cleanPath -match '^/?user/?$') {
            $filePath = Join-Path $RootPath "user\index.html"
        } elseif ($cleanPath -match '^/?admin/?$') {
            $filePath = Join-Path $RootPath "admin\index.html"
        } elseif ($cleanPath -eq "/" -or $cleanPath -eq "") {
            $filePath = Join-Path $RootPath "index.html"
        } else {
            $relPath = $cleanPath.TrimStart('/') -replace '/', '\'
            $filePath = Join-Path $RootPath $relPath
            if ((Test-Path $filePath) -and (Get-Item $filePath).PSIsContainer) {
                $filePath = Join-Path $filePath "index.html"
            }
        }

        if (Test-Path $filePath -PathType Leaf) {
            $ext = [System.IO.Path]::GetExtension($filePath).ToLower()
            $mime = if ($mimeTypes.ContainsKey($ext)) { $mimeTypes[$ext] } else { "application/octet-stream" }
            $bytes = [System.IO.File]::ReadAllBytes($filePath)

            $header = "HTTP/1.1 200 OK`r`nContent-Type: $mime`r`nContent-Length: $($bytes.Length)`r`nAccess-Control-Allow-Origin: *`r`nConnection: close`r`n`r`n"
            $headerBytes = [System.Text.Encoding]::UTF8.GetBytes($header)
            
            $stream.Write($headerBytes, 0, $headerBytes.Length)
            $stream.Write($bytes, 0, $bytes.Length)
            $stream.Flush()
        } else {
            $body = "404 Not Found: $cleanPath"
            $bodyBytes = [System.Text.Encoding]::UTF8.GetBytes($body)
            $header = "HTTP/1.1 404 Not Found`r`nContent-Type: text/plain`r`nContent-Length: $($bodyBytes.Length)`r`nConnection: close`r`n`r`n"
            $headerBytes = [System.Text.Encoding]::UTF8.GetBytes($header)

            $stream.Write($headerBytes, 0, $headerBytes.Length)
            $stream.Write($bodyBytes, 0, $bodyBytes.Length)
            $stream.Flush()
        }

        $client.Close()
    }
} finally {
    $listener.Stop()
}

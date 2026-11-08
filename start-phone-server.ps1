# SORA HR - Standalone Mobile Wi-Fi Server
$Port = 8080
$RootPath = $PSScriptRoot

$LocalIP = "192.168.31.192"
try {
    $ipObj = Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue | Where-Object { 
        $_.InterfaceAlias -notmatch "Loopback|vEthernet" -and $_.IPAddress -notmatch "^169\." 
    } | Select-Object -First 1
    if ($ipObj -and $ipObj.IPAddress) {
        $LocalIP = $ipObj.IPAddress
    }
} catch {}

$userUrl = "http://" + $LocalIP + ":" + $Port + "/user/"

Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "   SORA HR PLATFORM -- MOBILE PHONE SERVER" -ForegroundColor Yellow
Write-Host "========================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Apne phone aur computer ko SAME Wi-Fi se connect rakhein." -ForegroundColor White
Write-Host "2. Apne Phone ke Chrome browser me ye link kholein:" -ForegroundColor Cyan
Write-Host ""
Write-Host "   $userUrl" -ForegroundColor Green
Write-Host ""
Write-Host "3. Ya phone me install kiye gaye APK ko open karein!" -ForegroundColor Magenta
Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "Server running on port $Port... (Press Ctrl+C to stop)" -ForegroundColor DarkGray
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

        $tokens = $requestLine.Split(" ")
        if ($tokens.Length -lt 2) {
            $client.Close()
            continue
        }

        $rawUrl = $tokens[1]
        $cleanPath = $rawUrl.Split("?")[0]
        $cleanPath = [System.Uri]::UnescapeDataString($cleanPath)

        if ($cleanPath -match "^/?user/?$") {
            $filePath = Join-Path $RootPath "user\index.html"
        } elseif ($cleanPath -match "^/?admin/?$") {
            $filePath = Join-Path $RootPath "admin\index.html"
        } elseif ($cleanPath -eq "/" -or $cleanPath -eq "") {
            $filePath = Join-Path $RootPath "index.html"
        } else {
            $relPath = $cleanPath.TrimStart("/").Replace("/", "\")
            $filePath = Join-Path $RootPath $relPath
            if ((Test-Path $filePath) -and (Get-Item $filePath).PSIsContainer) {
                $filePath = Join-Path $filePath "index.html"
            }
        }

        if (Test-Path $filePath -PathType Leaf) {
            $ext = [System.IO.Path]::GetExtension($filePath).ToLower()
            $mime = "application/octet-stream"
            if ($mimeTypes.ContainsKey($ext)) {
                $mime = $mimeTypes[$ext]
            }
            $bytes = [System.IO.File]::ReadAllBytes($filePath)

            $nl = "`r`n"
            $header = "HTTP/1.1 200 OK" + $nl + "Content-Type: " + $mime + $nl + "Content-Length: " + $bytes.Length + $nl + "Access-Control-Allow-Origin: *" + $nl + "Connection: close" + $nl + $nl
            $headerBytes = [System.Text.Encoding]::UTF8.GetBytes($header)

            $stream.Write($headerBytes, 0, $headerBytes.Length)
            $stream.Write($bytes, 0, $bytes.Length)
            $stream.Flush()
            Write-Host "[200 OK] $cleanPath" -ForegroundColor DarkGreen
        } else {
            $body = "404 Not Found: " + $cleanPath
            $bodyBytes = [System.Text.Encoding]::UTF8.GetBytes($body)

            $nl = "`r`n"
            $header = "HTTP/1.1 404 Not Found" + $nl + "Content-Type: text/plain" + $nl + "Content-Length: " + $bodyBytes.Length + $nl + "Connection: close" + $nl + $nl
            $headerBytes = [System.Text.Encoding]::UTF8.GetBytes($header)

            $stream.Write($headerBytes, 0, $headerBytes.Length)
            $stream.Write($bodyBytes, 0, $bodyBytes.Length)
            $stream.Flush()
            Write-Host "[404 Not Found] $cleanPath" -ForegroundColor DarkYellow
        }

        $client.Close()
    }
} finally {
    $listener.Stop()
}

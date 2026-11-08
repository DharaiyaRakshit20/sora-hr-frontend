using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class PhoneServer {
    static int port = 8080;
    static string rootDir = AppDomain.CurrentDomain.BaseDirectory;

    static string GetLocalIPAddress() {
        try {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                if (endPoint != null) {
                    return endPoint.Address.ToString();
                }
            }
        } catch { }

        try {
            string host = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(host);
            foreach (IPAddress ip in ipEntry.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork && !ip.ToString().StartsWith("127.") && !ip.ToString().StartsWith("169.")) {
                    return ip.ToString();
                }
            }
        } catch { }

        return "127.0.0.1";
    }

    static string GetMimeType(string path) {
        string ext = Path.GetExtension(path).ToLowerInvariant();
        switch (ext) {
            case ".html": case ".htm": return "text/html; charset=utf-8";
            case ".js": return "application/javascript; charset=utf-8";
            case ".css": return "text/css; charset=utf-8";
            case ".svg": return "image/svg+xml";
            case ".png": return "image/png";
            case ".jpg": case ".jpeg": return "image/jpeg";
            case ".gif": return "image/gif";
            case ".ico": return "image/x-icon";
            case ".json": return "application/json; charset=utf-8";
            case ".pdf": return "application/pdf";
            case ".apk": return "application/vnd.android.package-archive";
            default: return "application/octet-stream";
        }
    }

    static void Main(string[] args) {
        Console.Title = "SORA HR Platform - Mobile Phone Wi-Fi Server";
        string localIP = GetLocalIPAddress();
        string userUrl = "http://" + localIP + ":" + port + "/user/";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================================");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("   SORA HR PLATFORM -- MOBILE PHONE WI-FI SERVER");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================================");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("1. Apne phone aur computer ko SAME Wi-Fi se connect rakhein.");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("2. Apne Phone ke Chrome browser me ye link kholein:");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("   " + userUrl);
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("3. Ya phone me install kiye gaye APK ko open karein!");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================================");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Server running on port " + port + "... (Press Ctrl+C to stop)");
        Console.WriteLine();
        Console.ResetColor();

        TcpListener listener = null;
        try {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            listener.Start();
        } catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Warning on port " + port + ": " + ex.Message);
            port = 8081;
            Console.WriteLine("Switching to port " + port + "...");
            userUrl = "http://" + localIP + ":" + port + "/user/";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("New Link: " + userUrl);
            Console.ResetColor();
            try {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                listener.Start();
            } catch (Exception ex2) {
                Console.WriteLine("Failed to bind port: " + ex2.Message);
                return;
            }
        }

        while (true) {
            try {
                TcpClient client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ProcessClient, client);
            } catch {
                break;
            }
        }
    }

    static void ProcessClient(object obj) {
        TcpClient client = (TcpClient)obj;
        try {
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
                string requestLine = reader.ReadLine();
                if (string.IsNullOrEmpty(requestLine)) return;

                string[] tokens = requestLine.Split(' ');
                if (tokens.Length < 2) return;

                string rawUrl = tokens[1];
                string cleanPath = rawUrl.Split('?')[0];
                cleanPath = Uri.UnescapeDataString(cleanPath);

                string filePath;
                if (cleanPath == "/" || cleanPath == "") {
                    filePath = Path.Combine(rootDir, "index.html");
                } else if (cleanPath == "/user" || cleanPath == "/user/") {
                    filePath = Path.Combine(rootDir, "user", "index.html");
                } else if (cleanPath == "/admin" || cleanPath == "/admin/") {
                    filePath = Path.Combine(rootDir, "admin", "index.html");
                } else {
                    string relPath = cleanPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                    filePath = Path.Combine(rootDir, relPath);
                    if (Directory.Exists(filePath)) {
                        filePath = Path.Combine(filePath, "index.html");
                    }
                }

                if (File.Exists(filePath)) {
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    string mime = GetMimeType(filePath);
                    string headers = "HTTP/1.1 200 OK\r\n" +
                                     "Content-Type: " + mime + "\r\n" +
                                     "Content-Length: " + fileBytes.Length + "\r\n" +
                                     "Access-Control-Allow-Origin: *\r\n" +
                                     "Connection: close\r\n\r\n";
                    byte[] headerBytes = Encoding.UTF8.GetBytes(headers);
                    stream.Write(headerBytes, 0, headerBytes.Length);
                    stream.Write(fileBytes, 0, fileBytes.Length);
                    stream.Flush();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("[200 OK] " + cleanPath);
                    Console.ResetColor();
                } else if (cleanPath.StartsWith("/user", StringComparison.OrdinalIgnoreCase)) {
                    // SPA Fallback for User portal (prevents 404 on back/sub-routes)
                    string userHtml = Path.Combine(rootDir, "user", "index.html");
                    if (File.Exists(userHtml)) {
                        byte[] fileBytes = File.ReadAllBytes(userHtml);
                        string headers = "HTTP/1.1 200 OK\r\n" +
                                         "Content-Type: text/html; charset=utf-8\r\n" +
                                         "Content-Length: " + fileBytes.Length + "\r\n" +
                                         "Access-Control-Allow-Origin: *\r\n" +
                                         "Connection: close\r\n\r\n";
                        byte[] headerBytes = Encoding.UTF8.GetBytes(headers);
                        stream.Write(headerBytes, 0, headerBytes.Length);
                        stream.Write(fileBytes, 0, fileBytes.Length);
                        stream.Flush();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("[SPA Fallback 200 OK] " + cleanPath + " -> user/index.html");
                        Console.ResetColor();
                    }
                } else {
                    byte[] bodyBytes = Encoding.UTF8.GetBytes("404 Not Found: " + cleanPath);
                    string headers = "HTTP/1.1 404 Not Found\r\n" +
                                     "Content-Type: text/plain; charset=utf-8\r\n" +
                                     "Content-Length: " + bodyBytes.Length + "\r\n" +
                                     "Connection: close\r\n\r\n";
                    byte[] headerBytes = Encoding.UTF8.GetBytes(headers);
                    stream.Write(headerBytes, 0, headerBytes.Length);
                    stream.Write(bodyBytes, 0, bodyBytes.Length);
                    stream.Flush();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("[404 Not Found] " + cleanPath);
                    Console.ResetColor();
                }
            }
        } catch { }
        finally {
            try { client.Close(); } catch { }
        }
    }
}

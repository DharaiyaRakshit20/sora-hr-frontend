using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;

class TestLandingServer {
    static void Main() {
        int port = 8899;
        string rootDir = @"d:\sora-hr-frontend";
        TcpListener listener = new TcpListener(IPAddress.Loopback, port);
        listener.Start();
        Console.WriteLine("TcpListener started on http://127.0.0.1:" + port + "/");

        bool running = true;
        Thread serverThread = new Thread(() => {
            while (running) {
                try {
                    TcpClient client = listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem((_) => ProcessClient(client, rootDir));
                } catch {
                    break;
                }
            }
        });
        serverThread.IsBackground = true;
        serverThread.Start();

        Thread.Sleep(300);

        try {
            string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = chromePath,
                Arguments = "--headless=new --no-sandbox --disable-setuid-sandbox --disable-gpu --user-data-dir=\"d:\\sora-hr-frontend\\scratch\\temp-chrome\" --enable-logging=stderr --log-level=0 --virtual-time-budget=10000 --dump-dom http://127.0.0.1:" + port + "/",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = Process.Start(psi)) {
                string dom = proc.StandardOutput.ReadToEnd();
                string err = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                Console.WriteLine("Chrome exit code: " + proc.ExitCode);
                Console.WriteLine("DOM length: " + dom.Length);
                if (err.Contains("Uncaught") || err.Contains("Error")) {
                    Console.WriteLine("Chrome error:\n" + err);
                }

                File.WriteAllText(@"d:\sora-hr-frontend\scratch\rendered_test_dom.html", dom);

                MatchCollection sections = Regex.Matches(dom, @"<section[^>]*id=""([^""]+)""");
                Console.WriteLine("Rendered sections count: " + sections.Count);
                foreach (Match m in sections) {
                    Console.WriteLine(" - Section ID: " + m.Groups[1].Value);
                }
            }
        } catch (Exception ex) {
            Console.WriteLine("Exception: " + ex.Message);
        } finally {
            running = false;
            try { listener.Stop(); } catch { }
            Console.WriteLine("Server stopped.");
        }
    }

    static void ProcessClient(TcpClient client, string rootDir) {
        using (client)
        using (NetworkStream stream = client.GetStream()) {
            try {
                byte[] buffer = new byte[8192];
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0) return;

                string request = Encoding.ASCII.GetString(buffer, 0, read);
                string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
                if (lines.Length == 0) return;

                string[] reqParts = lines[0].Split(' ');
                if (reqParts.Length < 2) return;

                string urlPath = reqParts[1].Split('?')[0];
                if (urlPath == "/" || urlPath == "") urlPath = "/index.html";

                string relPath = urlPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                string filePath = Path.Combine(rootDir, relPath);

                if (File.Exists(filePath)) {
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    string ext = Path.GetExtension(filePath).ToLower();
                    string mime = "text/plain";
                    if (ext == ".html") mime = "text/html; charset=utf-8";
                    else if (ext == ".js") mime = "application/javascript; charset=utf-8";
                    else if (ext == ".css") mime = "text/css; charset=utf-8";
                    else if (ext == ".svg") mime = "image/svg+xml";
                    else if (ext == ".jpeg" || ext == ".jpg") mime = "image/jpeg";
                    else if (ext == ".png") mime = "image/png";

                    string header = "HTTP/1.1 200 OK\r\n" +
                                    "Content-Type: " + mime + "\r\n" +
                                    "Content-Length: " + fileBytes.Length + "\r\n" +
                                    "Access-Control-Allow-Origin: *\r\n" +
                                    "Connection: close\r\n\r\n";
                    byte[] headerBytes = Encoding.ASCII.GetBytes(header);
                    stream.Write(headerBytes, 0, headerBytes.Length);
                    stream.Write(fileBytes, 0, fileBytes.Length);
                } else {
                    string notFound = "HTTP/1.1 404 Not Found\r\nContent-Length: 0\r\nConnection: close\r\n\r\n";
                    byte[] notFoundBytes = Encoding.ASCII.GetBytes(notFound);
                    stream.Write(notFoundBytes, 0, notFoundBytes.Length);
                }
            } catch { }
        }
    }
}

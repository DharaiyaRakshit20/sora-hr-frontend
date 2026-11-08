using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class TestClient {
    static void Main() {
        try {
            using (TcpClient client = new TcpClient("127.0.0.1", 8080))
            using (NetworkStream ns = client.GetStream())
            using (StreamReader sr = new StreamReader(ns, Encoding.UTF8)) {
                byte[] req = Encoding.UTF8.GetBytes("GET /user/ HTTP/1.1\r\nHost: localhost:8080\r\nConnection: close\r\n\r\n");
                ns.Write(req, 0, req.Length);
                string line = sr.ReadLine();
                Console.WriteLine("Server response: " + line);
            }
        } catch (Exception ex) {
            Console.WriteLine("Server not running or error: " + ex.Message);
        }
    }
}

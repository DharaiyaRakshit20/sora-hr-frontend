using System;
using System.IO;

class Program {
    static void Main() {
        string text = File.ReadAllText(@"d:\sora-hr-frontend\start-phone-server.ps1");
        string[] lines = File.ReadAllLines(@"d:\sora-hr-frontend\start-phone-server.ps1");
        Console.WriteLine("Total lines: " + lines.Length);
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("Content-Length") || lines[i].Contains("Missing")) {
                Console.WriteLine((i + 1) + ": " + lines[i]);
            }
        }
    }
}

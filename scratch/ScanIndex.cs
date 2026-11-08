using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"d:\sora-hr-frontend\user\index.html");
        Console.WriteLine("Total lines in user/index.html: " + lines.Length);
        for (int i = 0; i < lines.Length; i++) {
            string l = lines[i];
            if (l.StartsWith("function ") || l.StartsWith("const ") && l.Contains("= (") || l.Contains("AttendanceViewTab") || l.Contains("LeaveManagementTab") || l.Contains("ApplyLeaveModal") || l.Contains("ProfileModal") || l.Contains("SelfiePreviewModal")) {
                Console.WriteLine((i + 1) + ": " + l.Trim());
            }
        }
    }
}

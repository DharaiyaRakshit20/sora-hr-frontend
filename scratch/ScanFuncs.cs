using System;
using System.IO;

class Program {
    static void Main() {
        string[] lines = File.ReadAllLines(@"d:\sora-hr-frontend\user\index.html");
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("function SelfieCameraModal")) {
                Console.WriteLine("SelfieCameraModal at line " + (i + 1));
                for (int j = i; j < i + 60; j++) {
                    Console.WriteLine((j + 1) + ": " + lines[j]);
                }
                break;
            }
        }
    }
}

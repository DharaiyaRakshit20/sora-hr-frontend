using System;
using System.IO;

class VerifyFile {
    static void Main() {
        string text = File.ReadAllText(@"d:\sora-hr-frontend\user\index.html");
        
        // Check for basic bracket matching or obvious issues
        int openBraces = 0, closeBraces = 0;
        int openParens = 0, closeParens = 0;
        foreach (char c in text) {
            if (c == '{') openBraces++;
            else if (c == '}') closeBraces++;
            else if (c == '(') openParens++;
            else if (c == ')') closeParens++;
        }
        Console.WriteLine(string.Format("Braces: {0} open, {1} close (diff: {2})", openBraces, closeBraces, openBraces - closeBraces));
        Console.WriteLine(string.Format("Parens: {0} open, {1} close (diff: {2})", openParens, closeParens, openParens - closeParens));
    }
}

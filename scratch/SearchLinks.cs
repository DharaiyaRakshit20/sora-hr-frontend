using System;
using System.IO;
using System.Text.RegularExpressions;

class SearchLinks {
    static void Main() {
        string text = File.ReadAllText(@"d:\sora-hr-frontend\user\index.html");
        var matches = Regex.Matches(text, @"href\s*=\s*\{?[^}\s>]+", RegexOptions.IgnoreCase);
        foreach (Match m in matches) {
            Console.WriteLine(m.Value);
        }
    }
}

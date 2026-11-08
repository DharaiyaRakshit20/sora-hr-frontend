using System;
using System.IO;

class ApplyCameraFix {
    static void Main() {
        string indexPath = @"d:\sora-hr-frontend\user\index.html";
        string publicPath = @"d:\sora-hr-frontend\public\user\index.html";

        string content = File.ReadAllText(indexPath);

        int idxSelfie = content.IndexOf("function SelfieCameraModal");
        int idxBreak = content.IndexOf("function BreakSelectorModal");

        if (idxSelfie > 0 && idxBreak > idxSelfie) {
            int commentBeforeSelfie = content.LastIndexOf("// =", idxSelfie);
            int startCut = (commentBeforeSelfie >= 0 && commentBeforeSelfie > idxSelfie - 200) ? commentBeforeSelfie : idxSelfie;

            int commentBeforeBreak = content.LastIndexOf("// =", idxBreak);
            int endCut = (commentBeforeBreak >= 0 && commentBeforeBreak > idxSelfie) ? commentBeforeBreak : idxBreak;

            string newModals = File.ReadAllText(@"d:\sora-hr-frontend\scratch\updated_camera_modals.txt");

            content = content.Substring(0, startCut) + newModals + "\r\n\r\n    " + content.Substring(endCut);
            Console.WriteLine("[Step 1] Replaced SelfieCameraModal and QrScannerModal with live video + native phone camera support!");

            File.WriteAllText(indexPath, content);
            Console.WriteLine("[Step 2] Saved updated " + indexPath);

            File.WriteAllText(publicPath, content);
            Console.WriteLine("[Step 3] Synced updated " + publicPath);

            Console.WriteLine("CAMERA FIX COMPLETE!");
        } else {
            Console.WriteLine("ERROR: Could not locate selfie/break modal markers!");
        }
    }
}

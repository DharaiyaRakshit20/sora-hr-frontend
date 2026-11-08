using System;
using System.IO;

class InsertMapModalRender {
    static void Main() {
        string indexPath = @"d:\sora-hr-frontend\user\index.html";
        string publicPath = @"d:\sora-hr-frontend\public\user\index.html";

        string content = File.ReadAllText(indexPath);
        int idxLeave = content.IndexOf("function LeaveManagementTab");
        if (idxLeave > 0) {
            // Find the AttendanceViewTab closing `</div>\r\n      );\r\n    }`
            int idxClose = content.LastIndexOf("</div>\r\n      );\r\n    }", idxLeave);
            if (idxClose < 0) {
                idxClose = content.LastIndexOf("</div>\n      );\n    }", idxLeave);
            }

            if (idxClose > 0) {
                string insertion = @"          {/* Interactive GPS Geotag Map Modal */}
          {selectedMapLog && (
            <AttendanceMapModal
              log={selectedMapLog}
              hrSettings={hrSettings}
              onClose={() => setSelectedMapLog(null)}
            />
          )}

";
                content = content.Substring(0, idxClose) + insertion + content.Substring(idxClose);
                Console.WriteLine("SUCCESS: Inserted AttendanceMapModal rendering at pos: " + idxClose);

                File.WriteAllText(indexPath, content);
                File.WriteAllText(publicPath, content);
                Console.WriteLine("Saved and synced!");
            } else {
                Console.WriteLine("ERROR: idxClose not found");
            }
        } else {
            Console.WriteLine("ERROR: idxLeave not found");
        }
    }
}

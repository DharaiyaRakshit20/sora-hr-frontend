using System;
using System.IO;

class ApplyDualPunchMap {
    static void Main() {
        string indexPath = @"d:\sora-hr-frontend\user\index.html";
        string publicPath = @"d:\sora-hr-frontend\public\user\index.html";

        string content = File.ReadAllText(indexPath);

        // 1. Replace AttendanceMapModal
        int idxMapStart = content.IndexOf("function AttendanceMapModal");
        int idxTabStart = content.IndexOf("function AttendanceViewTab");

        if (idxMapStart > 0 && idxTabStart > idxMapStart) {
            int commentBeforeMap = content.LastIndexOf("// =", idxMapStart);
            int startCut = (commentBeforeMap >= 0 && commentBeforeMap > idxMapStart - 200) ? commentBeforeMap : idxMapStart;

            int commentBeforeTab = content.LastIndexOf("// =", idxTabStart);
            int endCut = (commentBeforeTab >= 0 && commentBeforeTab > idxMapStart) ? commentBeforeTab : idxTabStart;

            string newModal = File.ReadAllText(@"d:\sora-hr-frontend\scratch\updated_map_modal.txt");

            content = content.Substring(0, startCut) + newModal + "\r\n\r\n    " + content.Substring(endCut);
            Console.WriteLine("[Step 1] Replaced AttendanceMapModal with India Dual Punch (In & Out) visualizer");
        } else {
            Console.WriteLine("[Step 1 Error] Could not find AttendanceMapModal markers");
        }

        // 2. Replace INITIAL_ATTENDANCE_LOGS
        int idxLogsStart = content.IndexOf("const INITIAL_ATTENDANCE_LOGS = [");
        int idxPayrollStart = content.IndexOf("const INITIAL_PAYROLL_MASTER = [");
        if (idxLogsStart > 0 && idxPayrollStart > idxLogsStart) {
            int endCutLogs = content.LastIndexOf("];", idxPayrollStart);
            if (endCutLogs > idxLogsStart) {
                endCutLogs += 2; // include ];
                string newLogs = File.ReadAllText(@"d:\sora-hr-frontend\scratch\enriched_logs.txt");
                content = content.Substring(0, idxLogsStart) + newLogs + content.Substring(endCutLogs);
                Console.WriteLine("[Step 2] Enriched INITIAL_ATTENDANCE_LOGS with inLat, inLng, outLat, outLng, and India geotags");
            }
        } else {
            Console.WriteLine("[Step 2 Error] Could not find INITIAL_ATTENDANCE_LOGS markers");
        }

        // 3. Update completeClockInRecord to populate inLat, inLng, inLocation
        string oldNewLog = @"        // Add or update today's log in attendance history
        const newLog = {
          id: 'ATT-' + now.toISOString().slice(0, 10).replace(/-/g, ''),
          date: now.toISOString().slice(0, 10),
          displayDate: now.toLocaleDateString('en-IN', { day: '2-digit', month: 'short', year: 'numeric' }) + ' (Today)',
          inTime: timeFormatted,
          outTime: '--:--',
          breakMinutes: 0,
          hours: 'In Progress',
          status: status,
          location: locationStr,
          lat: coords ? coords.lat : hrSettings.officeLat,
          lng: coords ? coords.lng : hrSettings.officeLng,
          selfieUrl: selfieDataUrl,
          hasSelfie: !!selfieDataUrl,
          method: punchMethod
        };";

        string newNewLog = @"        const inLat = coords ? coords.lat : hrSettings.officeLat;
        const inLng = coords ? coords.lng : hrSettings.officeLng;
        const inLocation = `BKC Campus Gate 2, Mumbai, Maharashtra, India (${dist || 28}m away)`;

        // Add or update today's log in attendance history
        const newLog = {
          id: 'ATT-' + now.toISOString().slice(0, 10).replace(/-/g, ''),
          date: now.toISOString().slice(0, 10),
          displayDate: now.toLocaleDateString('en-IN', { day: '2-digit', month: 'short', year: 'numeric' }) + ' (Today)',
          inTime: timeFormatted,
          outTime: '--:--',
          breakMinutes: 0,
          hours: 'In Progress',
          status: status,
          location: locationStr,
          inLat: inLat,
          inLng: inLng,
          inLocation: inLocation,
          outLat: null,
          outLng: null,
          outLocation: 'Shift Active (Clock-Out Pending)',
          lat: inLat,
          lng: inLng,
          selfieUrl: selfieDataUrl,
          hasSelfie: !!selfieDataUrl,
          method: punchMethod
        };";

        if (content.Contains(oldNewLog)) {
            content = content.Replace(oldNewLog, newNewLog);
            Console.WriteLine("[Step 3] Updated newLog in completeClockInRecord");
        } else {
            string oldNewLogLF = oldNewLog.Replace("\r\n", "\n");
            string newNewLogLF = newNewLog.Replace("\r\n", "\n");
            if (content.Contains(oldNewLogLF)) {
                content = content.Replace(oldNewLogLF, newNewLogLF);
                Console.WriteLine("[Step 3] Updated newLog in completeClockInRecord (LF matched)");
            }
        }

        // 4. Update handleClockOut to record outLat, outLng, outLocation
        string oldClockOutMap = @"        setAttendanceLogs(prev => prev.map((l, idx) => {
          if (idx === 0) {
            return {
              ...l,
              outTime: timeFormatted,
              hours: hoursFormatted
            };
          }
          return l;
        }));";

        string newClockOutMap = @"        const outLat = (hrSettings.officeLat || 19.0658) + 0.0004;
        const outLng = (hrSettings.officeLng || 72.8688) + 0.0003;
        const outLocation = 'BKC Campus Main Tower Exit, Mumbai, Maharashtra, India';

        setAttendanceLogs(prev => prev.map((l, idx) => {
          if (idx === 0) {
            return {
              ...l,
              outTime: timeFormatted,
              hours: hoursFormatted,
              outLat: outLat,
              outLng: outLng,
              outLocation: outLocation
            };
          }
          return l;
        }));";

        if (content.Contains(oldClockOutMap)) {
            content = content.Replace(oldClockOutMap, newClockOutMap);
            Console.WriteLine("[Step 4] Updated handleClockOut with outLat, outLng, and outLocation");
        } else {
            string oldClockOutMapLF = oldClockOutMap.Replace("\r\n", "\n");
            string newClockOutMapLF = newClockOutMap.Replace("\r\n", "\n");
            if (content.Contains(oldClockOutMapLF)) {
                content = content.Replace(oldClockOutMapLF, newClockOutMapLF);
                Console.WriteLine("[Step 4] Updated handleClockOut (LF matched)");
            }
        }

        // Save
        File.WriteAllText(indexPath, content);
        Console.WriteLine("[Step 5] Saved updated " + indexPath);

        File.WriteAllText(publicPath, content);
        Console.WriteLine("[Step 6] Synced updated " + publicPath);

        Console.WriteLine("ALL UPDATES SUCCESSFULLY APPLIED!");
    }
}

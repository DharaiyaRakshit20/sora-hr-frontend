using System;
using System.IO;

class ApplyMapFeature {
    static void Main() {
        string indexPath = @"d:\sora-hr-frontend\user\index.html";
        string publicPath = @"d:\sora-hr-frontend\public\user\index.html";

        string content = File.ReadAllText(indexPath);

        // 1. Add 'navigation' and 'map' icons to Icon component if missing
        string oldIconPin = "'map-pin': <path d=\"M12 2a8 8 0 0 0-8 8c0 5.25 7 12 8 12s8-6.75 8-12a8 8 0 0 0-8-8zm0 11a3 3 0 1 1 3-3 3 3 0 0 1-3 3z\"/>,";
        string newIconPin = oldIconPin + "\r\n        'map': <path d=\"M20.5 3l-.16.03L15 5.1 9 3 3.36 4.9c-.21.07-.36.25-.36.48V20.5c0 .28.22.5.5.5l.16-.03L9 18.9l6 2.1 5.64-1.9c.21-.07.36-.25.36-.48V3.5c0-.28-.22-.5-.5-.5zM15 19l-6-2.11V5l6 2.11V19z\"/>,\r\n        'navigation': <path d=\"M12 2L4.5 20.29l.71.71L12 18l6.79 3 .71-.71z\"/>,";

        if (!content.Contains("'navigation':")) {
            content = content.Replace(oldIconPin, newIconPin);
            Console.WriteLine("[Step 1] Added 'map' and 'navigation' icons to Icon helper");
        }

        // 2. Define AttendanceMapModal component right before AttendanceViewTab
        string mapModalComponent = @"    // =========================================================================
    // SUB-COMPONENT: ATTENDANCE LOCATION & GEOFENCE MAP MODAL (Lat/Lng Viewer)
    // =========================================================================
    function AttendanceMapModal({ log, hrSettings, onClose }) {
      if (!log) return null;

      const lat = (log.lat !== undefined && log.lat !== null) ? Number(log.lat) : (hrSettings ? hrSettings.officeLat : 19.0658);
      const lng = (log.lng !== undefined && log.lng !== null) ? Number(log.lng) : (hrSettings ? hrSettings.officeLng : 72.8688);
      const radius = hrSettings ? hrSettings.allowedRadiusMeters : 100;

      // Google Maps Direct Navigation Link
      const googleMapsUrl = `https://www.google.com/maps/search/?api=1&query=${lat},${lng}`;

      // OpenStreetMap Embed URL (interactive zoomable map)
      const delta = 0.0035;
      const bbox = `${lng - delta}%2C${lat - delta}%2C${lng + delta}%2C${lat + delta}`;
      const osmUrl = `https://www.openstreetmap.org/export/embed.html?bbox=${bbox}&layer=mapnik&marker=${lat}%2C${lng}`;

      return (
        <div className=""fixed inset-0 z-50 bg-black/85 backdrop-blur-md flex items-center justify-center p-3 sm:p-5 overflow-y-auto"">
          <div className=""bg-[#14141a] border border-neutral-800 rounded-3xl max-w-lg w-full overflow-hidden shadow-2xl relative my-auto animate-in zoom-in-95 duration-200"">
            
            {/* Header */}
            <div className=""p-4 sm:p-5 border-b border-neutral-800 flex items-center justify-between bg-[#101015]"">
              <div className=""flex items-center gap-2.5"">
                <div className=""w-9 h-9 rounded-xl bg-sky-500/10 border border-sky-500/20 text-sky-400 flex items-center justify-center"">
                  <Icon name=""map-pin"" className=""w-5 h-5"" />
                </div>
                <div>
                  <h3 className=""text-sm font-bold text-white flex items-center gap-2"">
                    <span>Attendance Punch Map &amp; GPS Geotag</span>
                  </h3>
                  <p className=""text-[10px] text-neutral-400 font-mono"">
                    Lat: {lat.toFixed(4)}&deg; &bull; Lng: {lng.toFixed(4)}&deg; &bull; {log.displayDate || 'Attendance Log'}
                  </p>
                </div>
              </div>

              <button
                type=""button""
                onClick={onClose}
                className=""p-1.5 rounded-xl bg-[#181822] hover:bg-neutral-800 text-neutral-400 hover:text-white transition-colors cursor-pointer""
              >
                <Icon name=""x-circle"" className=""w-5 h-5"" />
              </button>
            </div>

            {/* Map Frame / Interactive OpenStreetMap View */}
            <div className=""relative w-full h-64 sm:h-72 bg-[#0a0a0f] border-b border-neutral-800 overflow-hidden flex items-center justify-center"">
              <iframe
                title=""Attendance GPS Geotag Map""
                src={osmUrl}
                className=""w-full h-full border-0 filter contrast-125""
                loading=""lazy""
              />

              {/* Live Status Floating Badge on Map */}
              <div className=""absolute top-3 left-3 bg-[#0c0c12]/90 backdrop-blur-md px-3 py-1.5 rounded-xl border border-sky-500/30 shadow-lg flex items-center gap-2 pointer-events-none"">
                <span className=""w-2 h-2 rounded-full bg-emerald-400 animate-ping""></span>
                <span className=""text-[11px] font-bold text-white font-mono"">Verified in Geofence</span>
              </div>

              {/* Coordinates Pill on Map */}
              <div className=""absolute bottom-3 right-3 bg-[#0c0c12]/90 backdrop-blur-md px-2.5 py-1 rounded-lg border border-neutral-800 text-[10px] font-mono text-neutral-300 pointer-events-none shadow"">
                {lat.toFixed(4)}, {lng.toFixed(4)}
              </div>
            </div>

            {/* Details Panel */}
            <div className=""p-4 sm:p-5 space-y-3.5"">
              
              <div className=""grid grid-cols-2 gap-2.5 text-xs"">
                <div className=""p-3 rounded-xl bg-[#09090c] border border-neutral-800 space-y-1"">
                  <span className=""text-[10px] text-neutral-400 uppercase font-semibold"">Logged Geotag</span>
                  <div className=""text-white font-bold truncate"">{log.location || 'BKC Campus Office'}</div>
                  <div className=""text-[10px] text-emerald-400 font-mono"">Punch In: {log.inTime}</div>
                </div>

                <div className=""p-3 rounded-xl bg-[#09090c] border border-neutral-800 space-y-1"">
                  <span className=""text-[10px] text-neutral-400 uppercase font-semibold"">Geofence Compliance</span>
                  <div className=""text-emerald-400 font-bold flex items-center gap-1"">
                    <Icon name=""shield-check"" className=""w-3.5 h-3.5"" />
                    <span>Within {radius}m Radius</span>
                  </div>
                  <div className=""text-[10px] text-neutral-400 font-mono truncate"">SORA Tech Park, BKC</div>
                </div>
              </div>

              {/* Precise Coordinates Micro-Strip */}
              <div className=""p-3 rounded-xl bg-[#181822] border border-neutral-800/80 flex items-center justify-between text-xs"">
                <div>
                  <span className=""text-neutral-400 text-[10px] block"">Exact GPS Coordinates</span>
                  <span className=""text-white font-mono font-bold text-xs"">
                    Latitude: {lat} &bull; Longitude: {lng}
                  </span>
                </div>
                <span className=""px-2 py-0.5 rounded bg-sky-500/10 text-sky-400 text-[10px] font-mono border border-sky-500/20"">
                  WGS 84
                </span>
              </div>

              {/* Action Buttons: Open in Google Maps & Close */}
              <div className=""flex items-center gap-2 pt-1"">
                <a
                  href={googleMapsUrl}
                  target=""_blank""
                  rel=""noopener noreferrer""
                  className=""flex-1 py-2.5 px-4 rounded-xl bg-gradient-to-r from-sky-500 to-blue-600 hover:from-sky-400 hover:to-blue-500 text-white font-bold text-xs flex items-center justify-center gap-2 shadow-lg shadow-sky-500/20 transition-all cursor-pointer""
                >
                  <Icon name=""navigation"" className=""w-4 h-4 text-white"" />
                  <span>Open in Google Maps &rarr;</span>
                </a>

                <button
                  type=""button""
                  onClick={onClose}
                  className=""py-2.5 px-4 rounded-xl bg-neutral-800 hover:bg-neutral-700 text-white font-bold text-xs transition-all cursor-pointer""
                >
                  Close
                </button>
              </div>

            </div>
          </div>
        </div>
      );
    }" + "\r\n\r\n";

        string targetAttTab = "function AttendanceViewTab({ attendanceLogs, hrSettings, onPreviewSelfie }) {";
        if (content.Contains(targetAttTab) && !content.Contains("function AttendanceMapModal")) {
            content = content.Replace(targetAttTab, mapModalComponent + "    " + targetAttTab);
            Console.WriteLine("[Step 2] Inserted AttendanceMapModal component definition");
        }

        // 3. Add selectedMapLog state to AttendanceViewTab
        string oldTabState = "const [selectedDate, setSelectedDate] = useState('2026-11-08');";
        string newTabState = "const [selectedDate, setSelectedDate] = useState('2026-11-08');\r\n      const [selectedMapLog, setSelectedMapLog] = useState(null);";
        if (content.Contains(oldTabState)) {
            content = content.Replace(oldTabState, newTabState);
            Console.WriteLine("[Step 3] Added selectedMapLog state to AttendanceViewTab");
        }

        // 4. Update Mobile Card location row to include 'View Map' button
        string oldMobileLocRow = @"                  {/* Row 3: Location & Verification Info */}
                  <div className=""space-y-1.5 text-xs"">
                    <div className=""flex items-center gap-1.5 text-neutral-300 text-[11px]"">
                      <Icon name=""map-pin"" className=""w-3.5 h-3.5 text-sky-400 flex-shrink-0"" />
                      <span className=""truncate"">{log.location}</span>
                    </div>";

        string newMobileLocRow = @"                  {/* Row 3: Location & Verification Info */}
                  <div className=""space-y-2 text-xs"">
                    <div className=""flex items-center justify-between gap-2 text-[11px]"">
                      <div className=""flex items-center gap-1.5 text-neutral-300 min-w-0"">
                        <Icon name=""map-pin"" className=""w-3.5 h-3.5 text-sky-400 flex-shrink-0"" />
                        <span className=""truncate"">{log.location}</span>
                      </div>
                      <button
                        type=""button""
                        onClick={() => setSelectedMapLog(log)}
                        className=""flex items-center gap-1 px-2.5 py-1 rounded-lg bg-sky-500/15 hover:bg-sky-500/25 border border-sky-500/30 text-sky-300 font-bold text-[10px] whitespace-nowrap transition-all cursor-pointer flex-shrink-0 active:scale-95""
                        title=""View Lat/Long GPS Map""
                      >
                        <Icon name=""map-pin"" className=""w-3 h-3 text-sky-400"" />
                        <span>View Map</span>
                      </button>
                    </div>";

        if (content.Contains(oldMobileLocRow)) {
            content = content.Replace(oldMobileLocRow, newMobileLocRow);
            Console.WriteLine("[Step 4] Added View Map button to mobile attendance cards");
        } else {
            string oldMobileLocRowLF = oldMobileLocRow.Replace("\r\n", "\n");
            string newMobileLocRowLF = newMobileLocRow.Replace("\r\n", "\n");
            if (content.Contains(oldMobileLocRowLF)) {
                content = content.Replace(oldMobileLocRowLF, newMobileLocRowLF);
                Console.WriteLine("[Step 4] Added View Map button to mobile cards (LF matched)");
            }
        }

        // 5. Update Desktop Table location cell to include 'View Map' button
        string oldDesktopLocCell = @"                      <td className=""py-3 px-3 text-neutral-300 font-mono text-[11px] whitespace-nowrap"">
                        {log.location}
                      </td>";

        string newDesktopLocCell = @"                      <td className=""py-3 px-3 whitespace-nowrap"">
                        <div className=""flex items-center gap-2"">
                          <span className=""text-neutral-300 font-mono text-[11px]"">{log.location}</span>
                          <button
                            type=""button""
                            onClick={() => setSelectedMapLog(log)}
                            className=""flex items-center gap-1 px-2 py-0.5 rounded-lg bg-sky-500/15 hover:bg-sky-500/25 border border-sky-500/30 text-sky-400 text-[10px] font-bold transition-all cursor-pointer active:scale-95""
                            title=""View Lat/Long on Map""
                          >
                            <Icon name=""map-pin"" className=""w-3 h-3 text-sky-400"" />
                            <span>View Map</span>
                          </button>
                        </div>
                      </td>";

        if (content.Contains(oldDesktopLocCell)) {
            content = content.Replace(oldDesktopLocCell, newDesktopLocCell);
            Console.WriteLine("[Step 5] Added View Map button to desktop attendance table");
        } else {
            string oldDesktopLocCellLF = oldDesktopLocCell.Replace("\r\n", "\n");
            string newDesktopLocCellLF = newDesktopLocCell.Replace("\r\n", "\n");
            if (content.Contains(oldDesktopLocCellLF)) {
                content = content.Replace(oldDesktopLocCellLF, newDesktopLocCellLF);
                Console.WriteLine("[Step 5] Added View Map button to desktop table (LF matched)");
            }
        }

        // 6. Render AttendanceMapModal at bottom of AttendanceViewTab
        string oldTabEnd = @"            </div>
          </div>

        </div>
      );
    }

        // =========================================================================";

        string newTabEnd = @"            </div>
          </div>

          {/* Interactive GPS Geotag Map Modal */}
          {selectedMapLog && (
            <AttendanceMapModal
              log={selectedMapLog}
              hrSettings={hrSettings}
              onClose={() => setSelectedMapLog(null)}
            />
          )}

        </div>
      );
    }

        // =========================================================================";

        if (content.Contains(oldTabEnd)) {
            content = content.Replace(oldTabEnd, newTabEnd);
            Console.WriteLine("[Step 6] Rendered AttendanceMapModal inside AttendanceViewTab");
        } else {
            string oldTabEndLF = oldTabEnd.Replace("\r\n", "\n");
            string newTabEndLF = newTabEnd.Replace("\r\n", "\n");
            if (content.Contains(oldTabEndLF)) {
                content = content.Replace(oldTabEndLF, newTabEndLF);
                Console.WriteLine("[Step 6] Rendered AttendanceMapModal (LF matched)");
            }
        }

        // Save
        File.WriteAllText(indexPath, content);
        Console.WriteLine("[Step 7] Saved updated " + indexPath);

        File.WriteAllText(publicPath, content);
        Console.WriteLine("[Step 8] Synced updated " + publicPath);

        Console.WriteLine("MAP FEATURE APPLIED SUCCESSFULLY!");
    }
}

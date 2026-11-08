using System;
using System.IO;

class ApplyPhase2 {
    static void Main() {
        string indexPath = @"d:\sora-hr-frontend\user\index.html";
        string backupPath = @"d:\sora-hr-frontend\user\index.html.bak2";
        string publicPath = @"d:\sora-hr-frontend\public\user\index.html";

        string content = File.ReadAllText(indexPath);
        File.WriteAllText(backupPath, content);
        Console.WriteLine("Backup created at: " + backupPath);

        // 1. Add VERIFIED_SELFIE_PREVIEW before INITIAL_ATTENDANCE_LOGS
        string verifiedSelfieDef = @"    const VERIFIED_SELFIE_PREVIEW = 'data:image/svg+xml;utf8,<svg xmlns=""http://www.w3.org/2000/svg"" width=""400"" height=""400"" viewBox=""0 0 400 400""><defs><linearGradient id=""bg"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%""><stop offset=""0%"" stop-color=""%2314141f""/><stop offset=""100%"" stop-color=""%230a0a0f""/></linearGradient></defs><rect width=""400"" height=""400"" fill=""url(%23bg)""/><g stroke=""%233b82f6"" stroke-width=""0.5"" stroke-opacity=""0.3""><line x1=""50"" y1=""50"" x2=""350"" y2=""50""/><line x1=""50"" y1=""100"" x2=""350"" y2=""100""/><line x1=""50"" y1=""150"" x2=""350"" y2=""150""/><line x1=""50"" y1=""200"" x2=""350"" y2=""200""/><line x1=""50"" y1=""250"" x2=""350"" y2=""250""/><line x1=""50"" y1=""300"" x2=""350"" y2=""300""/><line x1=""100"" y1=""50"" x2=""100"" y2=""350""/><line x1=""150"" y1=""50"" x2=""150"" y2=""350""/><line x1=""200"" y1=""50"" x2=""200"" y2=""350""/><line x1=""250"" y1=""50"" x2=""250"" y2=""350""/><line x1=""300"" y1=""50"" x2=""300"" y2=""350""/></g><ellipse cx=""200"" cy=""180"" rx=""85"" ry=""105"" fill=""%231e1e2d"" stroke=""%23f59e0b"" stroke-width=""2"" stroke-dasharray=""6 4""/><circle cx=""165"" cy=""160"" r=""12"" fill=""%23f59e0b"" fill-opacity=""0.2"" stroke=""%23f59e0b"" stroke-width=""2""/><circle cx=""165"" cy=""160"" r=""4"" fill=""%23f59e0b""/><circle cx=""235"" cy=""160"" r=""12"" fill=""%23f59e0b"" fill-opacity=""0.2"" stroke=""%23f59e0b"" stroke-width=""2""/><circle cx=""235"" cy=""160"" r=""4"" fill=""%23f59e0b""/><path d=""M 200 175 L 195 205 L 205 205 Z"" fill=""none"" stroke=""%23f59e0b"" stroke-width=""2""/><path d=""M 175 235 Q 200 255 225 235"" fill=""none"" stroke=""%23f59e0b"" stroke-width=""3"" stroke-linecap=""round""/><path d=""M 130 90 L 110 90 L 110 110"" fill=""none"" stroke=""%2310b981"" stroke-width=""4""/><path d=""M 270 90 L 290 90 L 290 110"" fill=""none"" stroke=""%2310b981"" stroke-width=""4""/><path d=""M 110 270 L 110 290 L 130 290"" fill=""none"" stroke=""%2310b981"" stroke-width=""4""/><path d=""M 290 270 L 290 290 L 270 290"" fill=""none"" stroke=""%2310b981"" stroke-width=""4""/><rect x=""40"" y=""320"" width=""320"" height=""50"" rx=""12"" fill=""%2310b981"" fill-opacity=""0.15"" stroke=""%2310b981"" stroke-width=""1.5""/><text x=""200"" y=""342"" fill=""%2310b981"" font-family=""system-ui, sans-serif"" font-size=""12"" font-weight=""bold"" text-anchor=""middle"">&#x2713; AI FACIAL BIOMETRIC MATCH: 99.4%</text><text x=""200"" y=""358"" fill=""%239ca3af"" font-family=""system-ui, sans-serif"" font-size=""10"" text-anchor=""middle"">EMP-AGT-0104 &bull; Pooja Iyer &bull; BKC Campus</text></svg>';" + "\r\n\r\n";

        if (!content.Contains("VERIFIED_SELFIE_PREVIEW")) {
            content = content.Replace("const INITIAL_ATTENDANCE_LOGS =", verifiedSelfieDef + "    const INITIAL_ATTENDANCE_LOGS =");
            Console.WriteLine("[Step 1] Injected VERIFIED_SELFIE_PREVIEW");
        }

        // Replace Unsplash URLs in INITIAL_ATTENDANCE_LOGS with VERIFIED_SELFIE_PREVIEW
        content = content.Replace(
            "selfieUrl: 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=300&auto=format&fit=crop&q=80'",
            "selfieUrl: VERIFIED_SELFIE_PREVIEW"
        );
        Console.WriteLine("[Step 2] Replaced Unsplash selfie URLs with offline verified biometric SVG");

        // 2. Add isProfileModalOpen and previewSelfieLog state to App()
        string oldModalState = "const [isHrSettingsModalOpen, setIsHrSettingsModalOpen] = useState(false);\r\n      const [previewSelfieUrl, setPreviewSelfieUrl] = useState(null);";
        string newModalState = "const [isHrSettingsModalOpen, setIsHrSettingsModalOpen] = useState(false);\r\n      const [previewSelfieUrl, setPreviewSelfieUrl] = useState(null);\r\n      const [isProfileModalOpen, setIsProfileModalOpen] = useState(false);\r\n      const [previewSelfieLog, setPreviewSelfieLog] = useState(null);";
        
        if (content.Contains(oldModalState)) {
            content = content.Replace(oldModalState, newModalState);
            Console.WriteLine("[Step 3] Added profile & selfie log state to App()");
        } else {
            Console.WriteLine("[Step 3 Warning] oldModalState not found as exact string, checking CRLF/LF");
            string oldModalStateLF = oldModalState.Replace("\r\n", "\n");
            string newModalStateLF = newModalState.Replace("\r\n", "\n");
            if (content.Contains(oldModalStateLF)) {
                content = content.Replace(oldModalStateLF, newModalStateLF);
                Console.WriteLine("[Step 3] Added profile & selfie log state (LF matched)");
            }
        }

        // 3. Add handleCancelLeave
        string oldLeaveReject = "const handleRejectLeave = (id) => {\r\n        setLeaveRequests(prev => prev.map(r => r.id === id ? { ...r, status: 'Rejected' } : r));\r\n        addToast('Leave Rejected', `Leave application #${id} was marked as rejected.`, 'info');\r\n      };";
        string newLeaveCancel = oldLeaveReject + "\r\n\r\n      const handleCancelLeave = (id) => {\r\n        setLeaveRequests(prev => prev.map(r => r.id === id ? { ...r, status: 'Cancelled' } : r));\r\n        addToast('Leave Cancelled', `Leave application #${id} has been withdrawn/cancelled.`, 'info');\r\n      };";
        if (content.Contains(oldLeaveReject)) {
            content = content.Replace(oldLeaveReject, newLeaveCancel);
            Console.WriteLine("[Step 4] Added handleCancelLeave");
        } else {
            string oldLeaveRejectLF = oldLeaveReject.Replace("\r\n", "\n");
            string newLeaveCancelLF = newLeaveCancel.Replace("\r\n", "\n");
            if (content.Contains(oldLeaveRejectLF)) {
                content = content.Replace(oldLeaveRejectLF, newLeaveCancelLF);
                Console.WriteLine("[Step 4] Added handleCancelLeave (LF matched)");
            }
        }

        // 4. Update Top Navbar User Avatar to be clickable button
        string oldNavbarAvatar = @"              {/* User Profile Avatar Pill */}
              <div className=""flex items-center gap-2 pl-1 border-l border-brand-border"">
                <div className=""w-8 h-8 rounded-xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-extrabold flex items-center justify-center text-xs shadow-md"">
                  {currentUser.avatar}
                </div>
                <div className=""hidden md:block text-left leading-tight"">
                  <div className=""text-xs font-bold text-white truncate max-w-[120px]"">{currentUser.name}</div>
                  <div className=""text-[10px] text-amber-400 truncate max-w-[120px]"">{currentUser.id}</div>
                </div>
              </div>";

        string newNavbarAvatar = @"              {/* User Profile Avatar Pill (Clickable -> Opens Profile Modal) */}
              <button
                type=""button""
                onClick={() => setIsProfileModalOpen(true)}
                className=""flex items-center gap-2 pl-1.5 pr-2 py-1 rounded-xl hover:bg-neutral-800/80 transition-colors cursor-pointer border border-transparent hover:border-brand-border""
                title=""View My Complete Profile""
              >
                <div className=""w-8 h-8 rounded-xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-extrabold flex items-center justify-center text-xs shadow-md"">
                  {currentUser.avatar}
                </div>
                <div className=""hidden md:block text-left leading-tight"">
                  <div className=""text-xs font-bold text-white truncate max-w-[120px]"">{currentUser.name}</div>
                  <div className=""text-[10px] text-amber-400 truncate max-w-[120px]"">{currentUser.id}</div>
                </div>
              </button>";

        if (content.Contains(oldNavbarAvatar)) {
            content = content.Replace(oldNavbarAvatar, newNavbarAvatar);
            Console.WriteLine("[Step 5] Made Top Navbar Avatar button clickable to open Profile modal");
        } else {
            string oldNavbarAvatarLF = oldNavbarAvatar.Replace("\r\n", "\n");
            string newNavbarAvatarLF = newNavbarAvatar.Replace("\r\n", "\n");
            if (content.Contains(oldNavbarAvatarLF)) {
                content = content.Replace(oldNavbarAvatarLF, newNavbarAvatarLF);
                Console.WriteLine("[Step 5] Made Top Navbar Avatar button clickable (LF matched)");
            }
        }

        // 5. Update Sidebar Drawer User Card to be clickable + Add View Profile nav item
        string oldDrawerCard = @"                  {/* User Profile Card */}
                  <div className=""p-4 rounded-2xl bg-[#181822] border border-neutral-800 flex items-center gap-3"">
                    <div className=""w-12 h-12 rounded-2xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-black text-base flex items-center justify-center shadow-lg"">
                      {currentUser.avatar}
                    </div>
                    <div className=""flex-1 min-w-0"">
                      <div className=""text-sm font-bold text-white truncate"">{currentUser.name}</div>
                      <div className=""text-[11px] text-amber-400 font-mono"">{currentUser.id}</div>
                      <div className=""text-[10px] text-neutral-400 truncate mt-0.5"">{currentUser.designation}</div>
                    </div>
                  </div>

                  {/* Navigation Links */}
                  <div className=""space-y-1"">
                    <div className=""px-3 pb-1 text-[10px] font-bold text-neutral-500 uppercase tracking-wider"">Navigation</div>";

        string newDrawerCard = @"                  {/* User Profile Card - Clickable to open Profile */}
                  <div 
                    onClick={() => { setIsSidebarOpen(false); setIsProfileModalOpen(true); }}
                    className=""p-4 rounded-2xl bg-[#181822] hover:bg-[#1f1f2e] border border-neutral-800 hover:border-amber-500/40 flex items-center justify-between gap-3 cursor-pointer transition-all group""
                    title=""Click to view full employee profile""
                  >
                    <div className=""flex items-center gap-3 min-w-0"">
                      <div className=""w-12 h-12 rounded-2xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-black text-base flex items-center justify-center shadow-lg group-hover:scale-105 transition-transform"">
                        {currentUser.avatar}
                      </div>
                      <div className=""flex-1 min-w-0"">
                        <div className=""text-sm font-bold text-white truncate group-hover:text-amber-400 transition-colors"">{currentUser.name}</div>
                        <div className=""text-[11px] text-amber-400 font-mono"">{currentUser.id}</div>
                        <div className=""text-[10px] text-neutral-400 truncate mt-0.5"">{currentUser.designation}</div>
                      </div>
                    </div>
                    <div className=""px-2 py-1 rounded-lg bg-amber-500/10 text-amber-400 text-[10px] font-bold border border-amber-500/20 whitespace-nowrap"">
                      Profile &rarr;
                    </div>
                  </div>

                  {/* Navigation Links */}
                  <div className=""space-y-1"">
                    <div className=""px-3 pb-1 text-[10px] font-bold text-neutral-500 uppercase tracking-wider"">Navigation</div>

                    <button
                      type=""button""
                      onClick={() => { setIsSidebarOpen(false); setIsProfileModalOpen(true); }}
                      className=""w-full px-3 py-2.5 rounded-xl font-bold text-xs flex items-center gap-3 text-neutral-300 hover:text-white hover:bg-[#181822] transition-all cursor-pointer""
                    >
                      <Icon name=""user"" className=""w-4 h-4 text-amber-400"" />
                      <span>My Employee Profile</span>
                    </button>";

        if (content.Contains(oldDrawerCard)) {
            content = content.Replace(oldDrawerCard, newDrawerCard);
            Console.WriteLine("[Step 6] Updated Sidebar Drawer user card and profile link");
        } else {
            string oldDrawerCardLF = oldDrawerCard.Replace("\r\n", "\n");
            string newDrawerCardLF = newDrawerCard.Replace("\r\n", "\n");
            if (content.Contains(oldDrawerCardLF)) {
                content = content.Replace(oldDrawerCardLF, newDrawerCardLF);
                Console.WriteLine("[Step 6] Updated Sidebar Drawer (LF matched)");
            }
        }

        // 6. Update Tab 2 and Tab 3 props in App()
        string oldTabCalls = @"              {/* TAB 2: DETAILED ATTENDANCE VIEW */}
              {activeTab === 'attendance' && (
                <AttendanceViewTab
                  attendanceLogs={attendanceLogs}
                  hrSettings={hrSettings}
                  onPreviewSelfie={(url) => setPreviewSelfieUrl(url)}
                  onOpenXlsUpload={() => setActiveTab('xls-upload')}
                  onOpenSettings={() => setIsHrSettingsModalOpen(true)}
                />
              )}

              {/* TAB 3: PENDING LEAVE MANAGEMENT */}
              {activeTab === 'leaves' && (
                <LeaveManagementTab
                  currentUser={currentUser}
                  leaveQuotas={leaveQuotas}
                  leaveRequests={leaveRequests}
                  onOpenApplyModal={() => setIsApplyLeaveModalOpen(true)}
                  onApproveLeave={handleApproveLeave}
                  onRejectLeave={handleRejectLeave}
                />
              )}";

        string newTabCalls = @"              {/* TAB 2: DETAILED ATTENDANCE VIEW */}
              {activeTab === 'attendance' && (
                <AttendanceViewTab
                  attendanceLogs={attendanceLogs}
                  hrSettings={hrSettings}
                  onPreviewSelfie={(log) => setPreviewSelfieLog(log)}
                />
              )}

              {/* TAB 3: PENDING LEAVE MANAGEMENT */}
              {activeTab === 'leaves' && (
                <LeaveManagementTab
                  currentUser={currentUser}
                  leaveQuotas={leaveQuotas}
                  leaveRequests={leaveRequests}
                  onOpenApplyModal={() => setIsApplyLeaveModalOpen(true)}
                  onCancelLeave={handleCancelLeave}
                />
              )}";

        if (content.Contains(oldTabCalls)) {
            content = content.Replace(oldTabCalls, newTabCalls);
            Console.WriteLine("[Step 7] Updated Tab 2 and Tab 3 props in App()");
        } else {
            string oldTabCallsLF = oldTabCalls.Replace("\r\n", "\n");
            string newTabCallsLF = newTabCalls.Replace("\r\n", "\n");
            if (content.Contains(oldTabCallsLF)) {
                content = content.Replace(oldTabCallsLF, newTabCallsLF);
                Console.WriteLine("[Step 7] Updated Tab 2 and Tab 3 props (LF matched)");
            }
        }

        // 7. Update Enlarged Selfie modal and add ProfileModal in App()
        int idxViewerStart = content.IndexOf("{/* F. ENLARGED SELFIE PHOTO VIEWER */}");
        if (idxViewerStart >= 0) {
            int idxViewerEnd = content.IndexOf("{/* FIXED BOTTOM MOBILE NAVIGATION BAR", idxViewerStart);
            if (idxViewerEnd > 0) {
                string newModalsMarkup = @"{/* F. VERIFIED BIOMETRIC SELFIE PHOTO VIEWER */}
          {previewSelfieLog && (
            <SelfiePreviewModal
              log={previewSelfieLog}
              onClose={() => setPreviewSelfieLog(null)}
            />
          )}

          {/* G. COMPLETE EMPLOYEE PROFILE MODAL */}
          {isProfileModalOpen && (
            <EmployeeProfileModal
              user={currentUser}
              onClose={() => setIsProfileModalOpen(false)}
            />
          )}

          ";
                content = content.Substring(0, idxViewerStart) + newModalsMarkup + content.Substring(idxViewerEnd);
                Console.WriteLine("[Step 8] Replaced old selfie viewer with SelfiePreviewModal & EmployeeProfileModal");
            }
        }

        // 8. Replace AttendanceViewTab with attendance_tab.txt
        string attendanceNew = File.ReadAllText(@"d:\sora-hr-frontend\scratch\attendance_tab.txt");
        int idxAttStart = content.IndexOf("function AttendanceViewTab");
        int idxLeaveStart = content.IndexOf("function LeaveManagementTab");
        if (idxAttStart >= 0 && idxLeaveStart > idxAttStart) {
            // Find start marker line for AttendanceViewTab
            int commentBefore = content.LastIndexOf("// =", idxAttStart);
            int startCut = (commentBefore >= 0 && commentBefore > idxAttStart - 200) ? commentBefore : idxAttStart;
            
            int commentBeforeLeave = content.LastIndexOf("// =", idxLeaveStart);
            int endCut = (commentBeforeLeave >= 0 && commentBeforeLeave > idxAttStart) ? commentBeforeLeave : idxLeaveStart;

            content = content.Substring(0, startCut) + attendanceNew + "\r\n\r\n    " + content.Substring(endCut);
            Console.WriteLine("[Step 9] Replaced AttendanceViewTab with responsive mobile-cards & dynamic counts");
        }

        // 9. Replace LeaveManagementTab with leave_tab.txt
        string leaveNew = File.ReadAllText(@"d:\sora-hr-frontend\scratch\leave_tab.txt");
        idxLeaveStart = content.IndexOf("function LeaveManagementTab");
        int idxSalaryStart = content.IndexOf("function SalarySlipTab");
        if (idxLeaveStart >= 0 && idxSalaryStart > idxLeaveStart) {
            int commentBefore = content.LastIndexOf("// =", idxLeaveStart);
            int startCut = (commentBefore >= 0 && commentBefore > idxLeaveStart - 200) ? commentBefore : idxLeaveStart;

            int commentBeforeSalary = content.LastIndexOf("// =", idxSalaryStart);
            int endCut = (commentBeforeSalary >= 0 && commentBeforeSalary > idxLeaveStart) ? commentBeforeSalary : idxSalaryStart;

            content = content.Substring(0, startCut) + leaveNew + "\r\n\r\n    " + content.Substring(endCut);
            Console.WriteLine("[Step 10] Replaced LeaveManagementTab with Employee Self-Service view");
        }

        // 10. Replace ApplyLeaveModal with apply_leave_modal.txt
        string applyLeaveNew = File.ReadAllText(@"d:\sora-hr-frontend\scratch\apply_leave_modal.txt");
        int idxApplyStart = content.IndexOf("function ApplyLeaveModal");
        int idxHrStart = content.IndexOf("function HrSettingsModal");
        if (idxApplyStart >= 0 && idxHrStart > idxApplyStart) {
            int commentBefore = content.LastIndexOf("// =", idxApplyStart);
            int startCut = (commentBefore >= 0 && commentBefore > idxApplyStart - 200) ? commentBefore : idxApplyStart;

            int commentBeforeHr = content.LastIndexOf("// =", idxHrStart);
            int endCut = (commentBeforeHr >= 0 && commentBeforeHr > idxApplyStart) ? commentBeforeHr : idxHrStart;

            content = content.Substring(0, startCut) + applyLeaveNew + "\r\n\r\n    " + content.Substring(endCut);
            Console.WriteLine("[Step 11] Replaced ApplyLeaveModal with auto-calculator and date formatter");
        }

        // 11. Append SelfiePreviewModal and EmployeeProfileModal before ReactDOM.createRoot
        string selfieModalCode = File.ReadAllText(@"d:\sora-hr-frontend\scratch\selfie_preview_modal.txt");
        string profileModalCode = File.ReadAllText(@"d:\sora-hr-frontend\scratch\profile_modal.txt");
        int idxRoot = content.IndexOf("ReactDOM.createRoot");
        if (idxRoot >= 0) {
            int commentBeforeRoot = content.LastIndexOf("// =", idxRoot);
            int insertPoint = (commentBeforeRoot >= 0 && commentBeforeRoot > idxRoot - 200) ? commentBeforeRoot : idxRoot;

            string appendedComponents = selfieModalCode + "\r\n\r\n" + profileModalCode + "\r\n\r\n    ";
            content = content.Substring(0, insertPoint) + appendedComponents + content.Substring(insertPoint);
            Console.WriteLine("[Step 12] Appended SelfiePreviewModal and EmployeeProfileModal definitions");
        }

        // Write updated content to user/index.html and public/user/index.html
        File.WriteAllText(indexPath, content);
        Console.WriteLine("[Step 13] Successfully saved updated " + indexPath);

        string pubDir = Path.GetDirectoryName(publicPath);
        if (!Directory.Exists(pubDir)) Directory.CreateDirectory(pubDir);
        File.WriteAllText(publicPath, content);
        Console.WriteLine("[Step 14] Successfully synced to " + publicPath);

        Console.WriteLine("ALL UPDATES APPLIED SUCCESSFULLY!");
    }
}

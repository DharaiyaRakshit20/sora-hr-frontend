using System;
using System.IO;

class ApplyQrAndProfile {
    static void Main() {
        string indexPath = @"d:\sora-hr-frontend\user\index.html";
        string publicPath = @"d:\sora-hr-frontend\public\user\index.html";

        string content = File.ReadAllText(indexPath);

        // 1. QR Scanner onSuccess: strictly punch attendance via QR without opening selfie modal
        string oldQrBlock = @"{/* B. QR SCANNER MODAL */}
          {isQrModalOpen && (
            <QrScannerModal
              onSuccess={() => {
                setIsQrModalOpen(false);
                if (hrSettings.requireSelfie) {
                  setIsSelfieModalOpen(true);
                } else {
                  finalizeClockIn(null);
                }
              }}
              onClose={() => setIsQrModalOpen(false)}
            />
          )}";

        string newQrBlock = @"{/* B. QR SCANNER MODAL (QR Attendance only - NO selfie camera chaining) */}
          {isQrModalOpen && (
            <QrScannerModal
              onSuccess={() => {
                setIsQrModalOpen(false);
                finalizeClockIn(null, 'QR Code Scan');
                addToast('QR Verified', 'BKC Office QR verified! Clocked in successfully.', 'success');
              }}
              onClose={() => setIsQrModalOpen(false)}
            />
          )}";

        if (content.Contains(oldQrBlock)) {
            content = content.Replace(oldQrBlock, newQrBlock);
            Console.WriteLine("[Step 1] QR scan modal chained selfie removed! QR punches directly.");
        } else {
            string oldQrBlockLF = oldQrBlock.Replace("\r\n", "\n");
            string newQrBlockLF = newQrBlock.Replace("\r\n", "\n");
            if (content.Contains(oldQrBlockLF)) {
                content = content.Replace(oldQrBlockLF, newQrBlockLF);
                Console.WriteLine("[Step 1] QR scan modal chained selfie removed (LF matched).");
            } else {
                Console.WriteLine("[Step 1 Error] oldQrBlock not found");
            }
        }

        // 2. Update finalizeClockIn & completeClockInRecord to accept methodOverride
        string oldFinalizeDef = "const finalizeClockIn = (selfieDataUrl) => {";
        string newFinalizeDef = "const finalizeClockIn = (selfieDataUrl, methodOverride) => {";
        if (content.Contains(oldFinalizeDef)) {
            content = content.Replace(oldFinalizeDef, newFinalizeDef);
            Console.WriteLine("[Step 2A] Updated finalizeClockIn signature");
        }

        content = content.Replace(
            "completeClockInRecord(now, timeFormatted, selfieDataUrl, { lat, lng }, dist);",
            "completeClockInRecord(now, timeFormatted, selfieDataUrl, { lat, lng }, dist, methodOverride);"
        );
        content = content.Replace(
            "completeClockInRecord(now, timeFormatted, selfieDataUrl, { lat: mockLat, lng: mockLng }, dist);",
            "completeClockInRecord(now, timeFormatted, selfieDataUrl, { lat: mockLat, lng: mockLng }, dist, methodOverride);"
        );
        content = content.Replace(
            "completeClockInRecord(now, timeFormatted, selfieDataUrl, null, null);",
            "completeClockInRecord(now, timeFormatted, selfieDataUrl, null, null, methodOverride);"
        );

        string oldCompleteSig = "const completeClockInRecord = (now, timeFormatted, selfieDataUrl, coords, dist) => {";
        string newCompleteSig = "const completeClockInRecord = (now, timeFormatted, selfieDataUrl, coords, dist, methodOverride) => {\r\n        const punchMethod = methodOverride || (selfieDataUrl ? 'Geo + Live Selfie' : 'Touchless Geo');";
        
        if (content.Contains(oldCompleteSig)) {
            content = content.Replace(oldCompleteSig, newCompleteSig);
            Console.WriteLine("[Step 2B] Updated completeClockInRecord signature");
        } else {
            string oldCompleteSigLF = oldCompleteSig.Replace("\r\n", "\n");
            string newCompleteSigLF = newCompleteSig.Replace("\r\n", "\n");
            if (content.Contains(oldCompleteSigLF)) {
                content = content.Replace(oldCompleteSigLF, newCompleteSigLF);
                Console.WriteLine("[Step 2B] Updated completeClockInRecord signature (LF matched)");
            }
        }

        // Replace method string in newLog
        content = content.Replace(
            "method: selfieDataUrl ? 'Geo + Live Selfie' : 'Touchless Geo'",
            "method: punchMethod"
        );
        Console.WriteLine("[Step 2C] Set punchMethod in attendance record");

        // 3. Update Top Navbar Avatar to render currentUser.photoUrl if present
        string oldNavAvatar = @"{/* User Profile Avatar Pill (Clickable -> Opens Profile Modal) */}
              <button
                type=""button""
                onClick={() => setIsProfileModalOpen(true)}
                className=""flex items-center gap-2 pl-1.5 pr-2 py-1 rounded-xl hover:bg-neutral-800/80 transition-colors cursor-pointer border border-transparent hover:border-brand-border""
                title=""View My Complete Profile""
              >
                <div className=""w-8 h-8 rounded-xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-extrabold flex items-center justify-center text-xs shadow-md"">
                  {currentUser.avatar}
                </div>";

        string newNavAvatar = @"{/* User Profile Avatar Pill (Clickable -> Opens Profile Modal) */}
              <button
                type=""button""
                onClick={() => setIsProfileModalOpen(true)}
                className=""flex items-center gap-2 pl-1.5 pr-2 py-1 rounded-xl hover:bg-neutral-800/80 transition-colors cursor-pointer border border-transparent hover:border-brand-border""
                title=""View My Complete Profile""
              >
                <div className=""w-8 h-8 rounded-xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-extrabold flex items-center justify-center text-xs shadow-md overflow-hidden"">
                  {currentUser.photoUrl ? (
                    <img src={currentUser.photoUrl} alt={currentUser.name} className=""w-full h-full object-cover"" />
                  ) : (
                    currentUser.avatar
                  )}
                </div>";

        if (content.Contains(oldNavAvatar)) {
            content = content.Replace(oldNavAvatar, newNavAvatar);
            Console.WriteLine("[Step 3] Updated Top Navbar to support profile image");
        } else {
            string oldNavAvatarLF = oldNavAvatar.Replace("\r\n", "\n");
            string newNavAvatarLF = newNavAvatar.Replace("\r\n", "\n");
            if (content.Contains(oldNavAvatarLF)) {
                content = content.Replace(oldNavAvatarLF, newNavAvatarLF);
                Console.WriteLine("[Step 3] Updated Top Navbar to support profile image (LF matched)");
            }
        }

        // 4. Update Sidebar Drawer Avatar to render currentUser.photoUrl if present
        string oldDrawerAvatar = @"<div className=""w-12 h-12 rounded-2xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-black text-base flex items-center justify-center shadow-lg group-hover:scale-105 transition-transform"">
                        {currentUser.avatar}
                      </div>";

        string newDrawerAvatar = @"<div className=""w-12 h-12 rounded-2xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-black text-base flex items-center justify-center shadow-lg group-hover:scale-105 transition-transform overflow-hidden"">
                        {currentUser.photoUrl ? (
                          <img src={currentUser.photoUrl} alt={currentUser.name} className=""w-full h-full object-cover"" />
                        ) : (
                          currentUser.avatar
                        )}
                      </div>";

        if (content.Contains(oldDrawerAvatar)) {
            content = content.Replace(oldDrawerAvatar, newDrawerAvatar);
            Console.WriteLine("[Step 4] Updated Sidebar Drawer to support profile image");
        } else {
            string oldDrawerAvatarLF = oldDrawerAvatar.Replace("\r\n", "\n");
            string newDrawerAvatarLF = newDrawerAvatar.Replace("\r\n", "\n");
            if (content.Contains(oldDrawerAvatarLF)) {
                content = content.Replace(oldDrawerAvatarLF, newDrawerAvatarLF);
                Console.WriteLine("[Step 4] Updated Sidebar Drawer to support profile image (LF matched)");
            }
        }

        // 5. Update App() EmployeeProfileModal invocation to pass onUpdateUser
        string oldProfileInvoke = @"{/* G. COMPLETE EMPLOYEE PROFILE MODAL */}
          {isProfileModalOpen && (
            <EmployeeProfileModal
              user={currentUser}
              onClose={() => setIsProfileModalOpen(false)}
            />
          )}";

        string newProfileInvoke = @"{/* G. COMPLETE EMPLOYEE PROFILE MODAL */}
          {isProfileModalOpen && (
            <EmployeeProfileModal
              user={currentUser}
              onUpdateUser={(updated) => {
                setCurrentUser(updated);
                addToast('Profile Photo Updated', 'Your profile picture has been saved successfully!', 'success');
              }}
              onClose={() => setIsProfileModalOpen(false)}
            />
          )}";

        if (content.Contains(oldProfileInvoke)) {
            content = content.Replace(oldProfileInvoke, newProfileInvoke);
            Console.WriteLine("[Step 5] Passed onUpdateUser handler to EmployeeProfileModal");
        } else {
            string oldProfileInvokeLF = oldProfileInvoke.Replace("\r\n", "\n");
            string newProfileInvokeLF = newProfileInvoke.Replace("\r\n", "\n");
            if (content.Contains(oldProfileInvokeLF)) {
                content = content.Replace(oldProfileInvokeLF, newProfileInvokeLF);
                Console.WriteLine("[Step 5] Passed onUpdateUser handler (LF matched)");
            }
        }

        // 6. Update Dashboard banner to display user avatar
        string oldGreeting = @"          {/* Top Banner Greeting */}
          <div className=""p-5 sm:p-6 rounded-3xl bg-gradient-to-r from-[#17171d] via-[#101014] to-[#17171d] border border-neutral-800 flex flex-col sm:flex-row sm:items-center justify-between gap-4"">
            <div>
              <div className=""text-xs font-semibold text-amber-400 flex items-center gap-1.5 mb-1"">
                <span>Apex Global Technologies</span>
                <span>&bull;</span>
                <span>{currentDateStr}</span>
              </div>
              <h2 className=""text-xl sm:text-2xl font-black text-white"">
                Hello, {currentUser.name}!
              </h2>
              <p className=""text-xs text-neutral-400 mt-1"">
                {currentUser.designation} &bull; {currentUser.department}
              </p>
            </div>";

        string newGreeting = @"          {/* Top Banner Greeting */}
          <div className=""p-5 sm:p-6 rounded-3xl bg-gradient-to-r from-[#17171d] via-[#101014] to-[#17171d] border border-neutral-800 flex flex-col sm:flex-row sm:items-center justify-between gap-4"">
            <div className=""flex items-center gap-3.5"">
              <div 
                className=""w-12 h-12 sm:w-14 sm:h-14 rounded-2xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-black text-xl flex items-center justify-center shadow-lg flex-shrink-0 overflow-hidden border-2 border-neutral-800""
                title=""Your Profile Photo""
              >
                {currentUser.photoUrl ? (
                  <img src={currentUser.photoUrl} alt={currentUser.name} className=""w-full h-full object-cover"" />
                ) : (
                  currentUser.avatar || 'PI'
                )}
              </div>
              <div>
                <div className=""text-xs font-semibold text-amber-400 flex items-center gap-1.5 mb-1"">
                  <span>Apex Global Technologies</span>
                  <span>&bull;</span>
                  <span>{currentDateStr}</span>
                </div>
                <h2 className=""text-xl sm:text-2xl font-black text-white"">
                  Hello, {currentUser.name}!
                </h2>
                <p className=""text-xs text-neutral-400 mt-0.5"">
                  {currentUser.designation} &bull; {currentUser.department}
                </p>
              </div>
            </div>";

        if (content.Contains(oldGreeting)) {
            content = content.Replace(oldGreeting, newGreeting);
            Console.WriteLine("[Step 6] Updated Dashboard banner to show user photo");
        } else {
            string oldGreetingLF = oldGreeting.Replace("\r\n", "\n");
            string newGreetingLF = newGreeting.Replace("\r\n", "\n");
            if (content.Contains(oldGreetingLF)) {
                content = content.Replace(oldGreetingLF, newGreetingLF);
                Console.WriteLine("[Step 6] Updated Dashboard banner (LF matched)");
            }
        }

        // 7. Replace EmployeeProfileModal definition with updated version
        string updatedModalDef = File.ReadAllText(@"d:\sora-hr-frontend\scratch\profile_modal_updated.txt");
        int idxProfileStart = content.IndexOf("function EmployeeProfileModal");
        if (idxProfileStart > 0) {
            int commentBefore = content.LastIndexOf("// =", idxProfileStart);
            int startCut = (commentBefore >= 0 && commentBefore > idxProfileStart - 200) ? commentBefore : idxProfileStart;

            int idxRoot = content.IndexOf("ReactDOM.createRoot", idxProfileStart);
            int commentBeforeRoot = (idxRoot > 0) ? content.LastIndexOf("// =", idxRoot) : -1;
            int endCut = (commentBeforeRoot >= 0 && commentBeforeRoot > idxProfileStart) ? commentBeforeRoot : idxRoot;

            content = content.Substring(0, startCut) + updatedModalDef + "\r\n\r\n    " + content.Substring(endCut);
            Console.WriteLine("[Step 7] Replaced EmployeeProfileModal definition with photo upload support");
        }

        // Save
        File.WriteAllText(indexPath, content);
        Console.WriteLine("[Step 8] Saved updated " + indexPath);

        File.WriteAllText(publicPath, content);
        Console.WriteLine("[Step 9] Synced updated " + publicPath);

        Console.WriteLine("ALL UPDATES SUCCESSFULLY APPLIED!");
    }
}

using System;
using System.IO;
using System.Text;

class BackNavPatcher {
    static void Main() {
        string filePath = @"d:\sora-hr-frontend\user\index.html";
        string content = File.ReadAllText(filePath, Encoding.UTF8);

        // 1. Insert History & Back navigation manager into App component
        string targetAnchor = "      // Sync state to localStorage\r\n      useEffect(() => {";
        if (!content.Contains(targetAnchor)) {
            targetAnchor = "      // Sync state to localStorage\n      useEffect(() => {";
        }

        int targetIndex = content.IndexOf(targetAnchor);
        if (targetIndex == -1) {
            Console.WriteLine("Error: targetAnchor not found!");
            return;
        }

        string backManagerCode = @"      // =======================================================================
      // 11. SMART SPA BACK-BUTTON NAVIGATION MANAGER (Mobile & Browser History Sync)
      // =======================================================================
      const isPoppingRef = React.useRef(false);
      const prevStatesRef = React.useRef({
        activeTab: 'dashboard',
        currentScreen: 'welcome',
        isSidebarOpen: false,
        isModalOpen: false
      });

      // Initialize baseline history entry
      React.useEffect(() => {
        if (!window.history.state || !window.history.state.soraRoot) {
          window.history.replaceState({ soraRoot: true }, '');
        }
      }, []);

      // Popstate listener: Intercepts mobile phone hardware back button & swipe back
      React.useEffect(() => {
        const handlePopState = (e) => {
          isPoppingRef.current = true;
          setTimeout(() => { isPoppingRef.current = false; }, 120);

          // 1. Modals priority (Close open modal first)
          if (previewSelfieLog) { setPreviewSelfieLog(null); return; }
          if (previewSelfieUrl) { setPreviewSelfieUrl(null); return; }
          if (isProfileModalOpen) { setIsProfileModalOpen(false); return; }
          if (isSelfieModalOpen) { setIsSelfieModalOpen(false); return; }
          if (isQrModalOpen) { setIsQrModalOpen(false); return; }
          if (isBreakModalOpen) { setIsBreakModalOpen(false); return; }
          if (isApplyLeaveModalOpen) { setIsApplyLeaveModalOpen(false); return; }
          if (isHrSettingsModalOpen) { setIsHrSettingsModalOpen(false); return; }

          // 2. Sidebar drawer priority (Close drawer)
          if (isSidebarOpen) { setIsSidebarOpen(false); return; }

          // 3. Pre-login screen navigation (login screen -> welcome screen)
          if (!isLoggedIn && currentScreen === 'login') {
            setCurrentScreen('welcome');
            return;
          }

          // 4. In-App tabs navigation (sub-tab -> dashboard)
          if (isLoggedIn && activeTab !== 'dashboard') {
            setActiveTab('dashboard');
            return;
          }
        };

        window.addEventListener('popstate', handlePopState);
        return () => window.removeEventListener('popstate', handlePopState);
      }, [
        isLoggedIn,
        currentScreen,
        activeTab,
        isSidebarOpen,
        isSelfieModalOpen,
        isQrModalOpen,
        isBreakModalOpen,
        isApplyLeaveModalOpen,
        isHrSettingsModalOpen,
        previewSelfieUrl,
        isProfileModalOpen,
        previewSelfieLog
      ]);

      // Push history state whenever user moves into a sub-view (opens modal, drawer, or switches tab)
      React.useEffect(() => {
        if (isPoppingRef.current) return;

        const isModalOpen = Boolean(
          previewSelfieLog || previewSelfieUrl || isProfileModalOpen ||
          isSelfieModalOpen || isQrModalOpen || isBreakModalOpen ||
          isApplyLeaveModalOpen || isHrSettingsModalOpen
        );

        const currentActiveTab = activeTab;
        const currentScr = currentScreen;
        const currentSidebar = isSidebarOpen;

        const prev = prevStatesRef.current;

        const justOpenedModal = !prev.isModalOpen && isModalOpen;
        const justOpenedSidebar = !prev.isSidebarOpen && currentSidebar;
        const justChangedTab = prev.activeTab !== currentActiveTab && currentActiveTab !== 'dashboard';
        const justWentToLogin = prev.currentScreen !== currentScr && currentScr === 'login';

        if (justOpenedModal || justOpenedSidebar || justChangedTab || justWentToLogin) {
          window.history.pushState({ soraSub: true, timestamp: Date.now() }, '');
        }

        prevStatesRef.current = {
          activeTab: currentActiveTab,
          currentScreen: currentScr,
          isSidebarOpen: currentSidebar,
          isModalOpen: isModalOpen
        };
      }, [
        activeTab,
        currentScreen,
        isSidebarOpen,
        isSelfieModalOpen,
        isQrModalOpen,
        isBreakModalOpen,
        isApplyLeaveModalOpen,
        isHrSettingsModalOpen,
        previewSelfieUrl,
        isProfileModalOpen,
        previewSelfieLog
      ]);\r\n\r\n";

        content = content.Substring(0, targetIndex) + backManagerCode + content.Substring(targetIndex);

        // 2. Add visual Back button in Header when on sub-tab
        string headerLeftTarget = "{/* Left: Brand Logo that opens Sidebar Drawer */}";
        int headerIndex = content.IndexOf(headerLeftTarget);
        if (headerIndex != -1) {
            string headerReplacement = @"{/* Left: Back Button (if on sub-tab) + Brand Logo / Menu */}
            <div className=""flex items-center gap-2"">
              {activeTab !== 'dashboard' && (
                <button
                  type=""button""
                  onClick={() => setActiveTab('dashboard')}
                  className=""py-1 px-2.5 rounded-xl bg-[#1c1c24] hover:bg-neutral-800 text-amber-400 hover:text-white flex items-center gap-1.5 cursor-pointer transition-all border border-neutral-700/80 shadow text-xs font-bold active:scale-95""
                  title=""Back to Dashboard""
                >
                  <Icon name=""arrow-left"" className=""w-3.5 h-3.5 text-amber-400"" />
                  <span>Back</span>
                </button>
              )}

              <button
                type=""button""
                onClick={() => setIsSidebarOpen(true)}
                className=""p-1.5 -ml-1 rounded-xl hover:bg-neutral-800 text-neutral-300 hover:text-white flex items-center gap-2 cursor-pointer transition-colors""
                title=""Open Navigation Menu""
              >";

            int buttonTagEnd = content.IndexOf("<button\r\n                type=\"button\"\r\n                onClick={() => setIsSidebarOpen(true)}", headerIndex);
            if (buttonTagEnd == -1) {
                buttonTagEnd = content.IndexOf("<button\n                type=\"button\"\n                onClick={() => setIsSidebarOpen(true)}", headerIndex);
            }
            if (buttonTagEnd != -1) {
                int buttonTagClose = content.IndexOf(">", buttonTagEnd) + 1;
                content = content.Substring(0, headerIndex) + headerReplacement + content.Substring(buttonTagClose);
            }
        }

        File.WriteAllText(filePath, content, Encoding.UTF8);
        Console.WriteLine("SUCCESS: Updated user/index.html with Smart Back-Button Navigation!");
    }
}

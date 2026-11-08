# Build Clean SORA HR Mobile ESS user/index.html
$ErrorActionPreference = "Stop"

$sourcePath = "d:\sora-hr-frontend\user\index.html.bak"
$destPath   = "d:\sora-hr-frontend\user\index.html"
$publicPath = "d:\sora-hr-frontend\public\user\index.html"

$raw = [System.IO.File]::ReadAllText($sourcePath, [System.Text.Encoding]::UTF8)

# -----------------------------------------------------------------------------
# 1. Add missing Icons (menu, lock, mail, zap) to Icon component
# -----------------------------------------------------------------------------
$oldIconMarker = "'clock': <path d=""M12 2a10 10 0 1 0 10 10A10 10 0 0 0 12 2zm0 18a8 8 0 1 1 8-8 8 8 0 0 1-8 8zm1-13h-2v6l5.25 3.15.75-1.23-4-2.42z""/>,"
$newIconAddition = @"
        'menu': <path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"/>,
        'lock': <path d="M18 8h-1V6c0-2.76-2.24-5-5-5S7 3.24 7 6v2H6c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V10c0-1.1-.9-2-2-2zm-6 9c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2zm3.1-9H8.9V6c0-1.71 1.39-3.1 3.1-3.1 1.71 0 3.1 1.39 3.1 3.1v2z"/>,
        'mail': <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z"/>,
        'zap': <path d="M7 2v11h3v9l7-12h-4l4-8z"/>,
        'clock': <path d="M12 2a10 10 0 1 0 10 10A10 10 0 0 0 12 2zm0 18a8 8 0 1 1 8-8 8 8 0 0 1-8 8zm1-13h-2v6l5.25 3.15.75-1.23-4-2.42z"/>,
"@

if ($raw.Contains($oldIconMarker)) {
    $raw = $raw.Replace($oldIconMarker, $newIconAddition)
    Write-Host "1. Updated Icon component with menu, lock, mail, zap"
}

# -----------------------------------------------------------------------------
# 2. Fix calculateDistance (replace Greek symbols with rad1, rad2, dLat, dLon)
# -----------------------------------------------------------------------------
$distRegex = '(?s)function calculateDistance\(lat1, lon1, lat2, lon2\)\s*\{.*?return Math\.round\(R \* c\);\s*\}'
$newDistCode = @"
function calculateDistance(lat1, lon1, lat2, lon2) {
      const R = 6371e3; // Earth radius in meters
      const rad1 = lat1 * Math.PI / 180;
      const rad2 = lat2 * Math.PI / 180;
      const dLat = (lat2 - lat1) * Math.PI / 180;
      const dLon = (lon2 - lon1) * Math.PI / 180;

      const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                Math.cos(rad1) * Math.cos(rad2) *
                Math.sin(dLon / 2) * Math.sin(dLon / 2);
      const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
      return Math.round(R * c);
    }
"@
$raw = [System.Text.RegularExpressions.Regex]::Replace($raw, $distRegex, $newDistCode)
Write-Host "2. Fixed calculateDistance with pure ASCII variables"

# -----------------------------------------------------------------------------
# 3. Add isSidebarOpen state and body scroll locking in App()
# -----------------------------------------------------------------------------
$appStateMarker = "const [currentScreen, setCurrentScreen] = useState('welcome');"
$newAppState = @"
const [currentScreen, setCurrentScreen] = useState('welcome');
      const [isSidebarOpen, setIsSidebarOpen] = useState(false);

      // Lock background body scroll when sidebar drawer is open
      useEffect(() => {
        if (isSidebarOpen) {
          document.body.style.overflow = 'hidden';
        } else {
          document.body.style.overflow = '';
        }
        return () => {
          document.body.style.overflow = '';
        };
      }, [isSidebarOpen]);
"@
if ($raw.Contains($appStateMarker) -and -not $raw.Contains("isSidebarOpen")) {
    $raw = $raw.Replace($appStateMarker, $newAppState)
    Write-Host "3. Added isSidebarOpen state & scroll lock effect"
}

# -----------------------------------------------------------------------------
# 4. Replace WelcomeScreen (Clean SORA HR logo, remove square icon)
# -----------------------------------------------------------------------------
$welcomeRegex = '(?s)// SUB-COMPONENT: SCREEN 1 -- WELCOME / ONBOARDING SCREEN.*?function WelcomeScreen\(\{ onNext \}\)\s*\{.*?return\s*\(.*?\);\s*\}'
$newWelcome = @"
// SUB-COMPONENT: SCREEN 1 -- WELCOME / ONBOARDING SCREEN
    // =========================================================================
    function WelcomeScreen({ onNext }) {
      const [activeSlide, setActiveSlide] = useState(0);

      const slides = [
        {
          badge: "Geofence & Biometrics",
          icon: "camera",
          title: "Selfie Attendance & GPS Punch",
          desc: "Clock in securely with AI facial selfie validation and automated BKC office geofence verification.",
          preview: "Geofence: Within BKC Campus (42m) - Verified"
        },
        {
          badge: "Smart Leaves",
          icon: "calendar",
          title: "Instant Leave Management",
          desc: "Check real-time balances for Casual, Sick, and Privilege leaves. Apply in 2 taps with instant HR approval.",
          preview: "Balances: 8 Casual - 5 Sick - 12 Privilege"
        },
        {
          badge: "Verified Payroll",
          icon: "file-text",
          title: "Salary Slips & Tax Statements",
          desc: "View itemized earnings, HRA, PF deductions, and download tamper-proof Indian standard PDF payslips.",
          preview: "Net Salary: Rs 98,540 / month"
        }
      ];

      return (
        <div className="min-h-screen bg-[#09090c] text-white flex flex-col justify-between p-5 max-w-md mx-auto relative overflow-hidden">
          {/* Subtle Ambient Amber Glow */}
          <div className="absolute top-0 left-1/2 -translate-x-1/2 w-80 h-80 bg-amber-500/10 rounded-full blur-3xl pointer-events-none"></div>

          {/* Header Branding - Clean SORA HR logo without square icon beside it */}
          <div className="relative z-10 pt-3 flex items-center justify-between">
            <div className="flex items-center gap-3">
              <img 
                src="/sora-hr-logo.svg" 
                alt="SORA HR" 
                className="h-8 w-auto object-contain" 
                onError={(e) => { e.target.src = '/sora-hr-logo.png'; }} 
              />
              <div className="border-l border-neutral-800 pl-2.5">
                <p className="text-[10px] text-amber-400 font-bold uppercase tracking-wider">Employee Portal</p>
                <p className="text-[11px] text-neutral-300 font-semibold">Self-Service</p>
              </div>
            </div>

            <span className="px-2.5 py-0.5 rounded-full bg-amber-500/10 border border-amber-500/20 text-[10px] font-bold text-amber-400">
              v2.4 ESS
            </span>
          </div>

          {/* Center Showcase Card */}
          <div className="relative z-10 my-auto py-6 space-y-5">
            <div className="p-6 rounded-3xl bg-gradient-to-b from-[#181822] to-[#111116] border border-neutral-800 shadow-2xl relative overflow-hidden">
              <div className="absolute -top-10 -right-10 w-28 h-28 bg-amber-500/10 rounded-full blur-xl pointer-events-none"></div>

              <div className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-amber-500/15 border border-amber-500/30 text-[11px] font-bold text-amber-300 mb-3">
                <span className="w-2 h-2 rounded-full bg-amber-400 animate-ping"></span>
                <span>{slides[activeSlide].badge}</span>
              </div>

              <h2 className="text-xl sm:text-2xl font-black text-white leading-snug mb-2">
                {slides[activeSlide].title}
              </h2>

              <p className="text-neutral-400 text-xs sm:text-sm leading-relaxed mb-5 font-normal">
                {slides[activeSlide].desc}
              </p>

              <div className="p-3 rounded-2xl bg-[#09090c]/90 border border-neutral-800 flex items-center gap-3">
                <div className="w-8 h-8 rounded-xl bg-amber-400/10 text-amber-400 flex items-center justify-center flex-shrink-0">
                  <Icon name="shield-check" className="w-4 h-4 text-emerald-400" />
                </div>
                <div className="text-[11px] font-semibold text-neutral-200">
                  {slides[activeSlide].preview}
                </div>
              </div>
            </div>

            {/* Carousel Dots */}
            <div className="flex items-center justify-center gap-2">
              {slides.map((_, idx) => (
                <button
                  key={idx}
                  onClick={() => setActiveSlide(idx)}
                  className={`h-1.5 rounded-full transition-all cursor-pointer ${
                    activeSlide === idx ? 'w-6 bg-amber-400' : 'w-2 bg-neutral-700'
                  }`}
                />
              ))}
            </div>
          </div>

          {/* Bottom Action Section */}
          <div className="relative z-10 pb-4 space-y-3">
            <button
              onClick={onNext}
              className="w-full py-3.5 px-6 rounded-2xl bg-gradient-to-r from-amber-500 to-amber-600 hover:from-amber-400 hover:to-amber-500 text-neutral-950 font-black text-sm flex items-center justify-center gap-2 shadow-lg shadow-amber-500/25 active:scale-[0.98] transition-all cursor-pointer"
            >
              <span>Get Started - Next</span>
              <Icon name="arrow-right" className="w-4 h-4" />
            </button>

            <p className="text-center text-[10px] text-neutral-500">
              Apex Global Technologies Pvt Ltd - Secured ESS Portal
            </p>
          </div>
        </div>
      );
    }
"@
$raw = [System.Text.RegularExpressions.Regex]::Replace($raw, $welcomeRegex, $newWelcome)
Write-Host "4. Replaced WelcomeScreen"

# -----------------------------------------------------------------------------
# 5. Replace LoginScreen (Logo replaces Back, Strictly Employee, Forgot Pass)
# -----------------------------------------------------------------------------
$loginRegex = '(?s)// SUB-COMPONENT: SCREEN 2 -- DEDICATED MOBILE LOGIN SCREEN.*?function LoginScreen\(.*?return\s*\(.*?\);\s*\}'
$newLogin = @"
// SUB-COMPONENT: SCREEN 2 -- DEDICATED EMPLOYEE LOGIN SCREEN
    // =========================================================================
    function LoginScreen({ onLogin }) {
      const [email, setEmail] = useState('pooja.i@apextech.com');
      const [empCode, setEmpCode] = useState('EMP-AGT-0104');
      const [password, setPassword] = useState('sora123');
      const [showPassword, setShowPassword] = useState(false);
      const [rememberMe, setRememberMe] = useState(true);

      // Forgot Password Modal State
      const [isForgotModalOpen, setIsForgotModalOpen] = useState(false);
      const [forgotStep, setForgotStep] = useState(1);
      const [forgotInput, setForgotInput] = useState('pooja.i@apextech.com');
      const [forgotOtp, setForgotOtp] = useState('');
      const [newPass, setNewPass] = useState('');
      const [forgotSuccessMsg, setForgotSuccessMsg] = useState('');

      const handleLoginSubmit = (e) => {
        if (e) e.preventDefault();
        onLogin(DEFAULT_USERS.employee);
      };

      const handleForgotSubmit = (e) => {
        e.preventDefault();
        if (forgotStep === 1) {
          setForgotStep(2);
        } else if (forgotStep === 2) {
          if (forgotOtp === '1234' || forgotOtp.trim().length > 0) {
            setForgotStep(3);
          } else {
            alert('Please enter valid 4-digit OTP (Demo: 1234)');
          }
        } else if (forgotStep === 3) {
          setPassword(newPass || 'sora123');
          setForgotSuccessMsg('Password updated successfully! You can now sign in.');
          setTimeout(() => {
            setIsForgotModalOpen(false);
            setForgotStep(1);
            setForgotSuccessMsg('');
          }, 1500);
        }
      };

      return (
        <div className="min-h-screen bg-[#09090c] text-white flex flex-col justify-between p-5 max-w-md mx-auto relative overflow-hidden">
          {/* Subtle Ambient Glow */}
          <div className="absolute top-0 left-1/2 -translate-x-1/2 w-80 h-80 bg-amber-500/10 rounded-full blur-3xl pointer-events-none"></div>

          {/* Top Bar: SORA HR Logo on Left (No Back button or arrow) */}
          <div className="relative z-10 pt-2 flex items-center justify-between">
            <div className="flex items-center gap-2">
              <img 
                src="/sora-hr-logo.svg" 
                alt="SORA HR" 
                className="h-7 w-auto object-contain" 
                onError={(e) => { e.target.src = '/sora-hr-logo.png'; }} 
              />
            </div>

            <div className="flex items-center gap-2">
              <span className="w-2 h-2 rounded-full bg-emerald-400 animate-pulse"></span>
              <span className="text-[11px] font-bold text-neutral-300">System Ready</span>
            </div>
          </div>

          {/* Center Form Container */}
          <div className="relative z-10 my-auto py-4 space-y-4">
            <div>
              <div className="w-10 h-10 rounded-2xl bg-amber-500/10 border border-amber-500/20 text-amber-400 flex items-center justify-center mb-2.5">
                <Icon name="log-in" className="w-5 h-5" />
              </div>
              <h2 className="text-2xl font-black text-white">Employee Sign In</h2>
              <p className="text-xs text-neutral-400 mt-0.5">Enter your work email, employee ID, and password</p>
            </div>

            {/* User Quick Info Badge */}
            <div className="p-3 rounded-2xl bg-[#14141a] border border-amber-500/20 flex items-center gap-3">
              <div className="w-9 h-9 rounded-xl bg-amber-500/20 text-amber-400 font-extrabold flex items-center justify-center text-xs">
                PI
              </div>
              <div className="flex-1 min-w-0">
                <div className="text-xs font-bold text-white truncate">Pooja Iyer</div>
                <div className="text-[10px] text-amber-400 truncate">EMP-AGT-0104 - Senior Full Stack Engineer</div>
              </div>
              <span className="px-2 py-0.5 rounded-md bg-emerald-500/10 text-emerald-400 text-[9px] font-bold border border-emerald-500/20">
                Verified ESS
              </span>
            </div>

            {/* Form */}
            <form onSubmit={handleLoginSubmit} className="space-y-3">
              {/* Field 1: Work Email */}
              <div>
                <label className="block text-[10px] font-bold text-neutral-400 mb-1 uppercase tracking-wider">
                  Work Email
                </label>
                <div className="relative">
                  <Icon name="mail" className="w-4 h-4 text-neutral-500 absolute left-3 top-1/2 -translate-y-1/2" />
                  <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    className="w-full pl-9 pr-3 py-2.5 rounded-xl bg-[#14141a] border border-neutral-800 focus:border-amber-500 text-white text-xs outline-none transition-all"
                    placeholder="pooja.i@apextech.com"
                    required
                  />
                </div>
              </div>

              {/* Field 2: Employee Code / ID */}
              <div>
                <label className="block text-[10px] font-bold text-neutral-400 mb-1 uppercase tracking-wider">
                  Employee Code / ID
                </label>
                <div className="relative">
                  <Icon name="user" className="w-4 h-4 text-neutral-500 absolute left-3 top-1/2 -translate-y-1/2" />
                  <input
                    type="text"
                    value={empCode}
                    onChange={(e) => setEmpCode(e.target.value)}
                    className="w-full pl-9 pr-3 py-2.5 rounded-xl bg-[#14141a] border border-neutral-800 focus:border-amber-500 text-white text-xs outline-none transition-all"
                    placeholder="EMP-AGT-0104"
                    required
                  />
                </div>
              </div>

              {/* Field 3: Password */}
              <div>
                <label className="block text-[10px] font-bold text-neutral-400 mb-1 uppercase tracking-wider">
                  Password
                </label>
                <div className="relative">
                  <Icon name="lock" className="w-4 h-4 text-neutral-500 absolute left-3 top-1/2 -translate-y-1/2" />
                  <input
                    type={showPassword ? "text" : "password"}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    className="w-full pl-9 pr-9 py-2.5 rounded-xl bg-[#14141a] border border-neutral-800 focus:border-amber-500 text-white text-xs outline-none transition-all"
                    placeholder="Enter password"
                    required
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-neutral-500 hover:text-white cursor-pointer"
                  >
                    <Icon name={showPassword ? "eye-off" : "eye"} className="w-3.5 h-3.5" />
                  </button>
                </div>
              </div>

              {/* Remember Me & Forgot Password Row */}
              <div className="flex items-center justify-between text-xs pt-0.5">
                <label className="flex items-center gap-1.5 cursor-pointer text-neutral-400 hover:text-white text-[11px]">
                  <input
                    type="checkbox"
                    checked={rememberMe}
                    onChange={(e) => setRememberMe(e.target.checked)}
                    className="rounded accent-amber-500"
                  />
                  <span>Remember me</span>
                </label>
                <button
                  type="button"
                  onClick={() => setIsForgotModalOpen(true)}
                  className="text-amber-400 hover:text-amber-300 text-[11px] font-semibold hover:underline cursor-pointer"
                >
                  Forgot Password?
                </button>
              </div>

              {/* Submit CTA */}
              <button
                type="submit"
                className="w-full mt-2 py-3.5 px-4 rounded-xl bg-gradient-to-r from-amber-500 to-amber-600 hover:from-amber-400 hover:to-amber-500 text-neutral-950 font-bold text-xs flex items-center justify-center gap-2 shadow-lg shadow-amber-500/20 active:scale-[0.98] transition-all cursor-pointer"
              >
                <Icon name="shield-check" className="w-4 h-4 text-neutral-950" />
                <span>Sign In to SORA HR</span>
              </button>
            </form>
          </div>

          {/* Footer Security Footnote */}
          <div className="relative z-10 text-center text-[10px] text-neutral-500 pb-1">
            256-bit Encrypted Session - Apex Global Technologies
          </div>

          {/* FORGOT PASSWORD MODAL */}
          {isForgotModalOpen && (
            <div className="fixed inset-0 z-50 bg-black/80 backdrop-blur-md flex items-center justify-center p-4">
              <div className="bg-[#14141a] border border-neutral-800 rounded-3xl max-w-sm w-full p-6 space-y-4 shadow-2xl relative">
                <div className="flex items-center justify-between pb-2 border-b border-neutral-800">
                  <div className="flex items-center gap-2">
                    <Icon name="lock" className="w-4 h-4 text-amber-400" />
                    <h3 className="text-sm font-bold text-white">Reset Password</h3>
                  </div>
                  <button
                    type="button"
                    onClick={() => { setIsForgotModalOpen(false); setForgotStep(1); }}
                    className="text-neutral-400 hover:text-white p-1 cursor-pointer"
                  >
                    <Icon name="x-circle" className="w-4 h-4" />
                  </button>
                </div>

                {forgotSuccessMsg ? (
                  <div className="p-3 rounded-xl bg-emerald-500/10 border border-emerald-500/20 text-emerald-400 text-xs font-semibold text-center">
                    {forgotSuccessMsg}
                  </div>
                ) : (
                  <form onSubmit={handleForgotSubmit} className="space-y-3">
                    {forgotStep === 1 && (
                      <div>
                        <p className="text-xs text-neutral-300 mb-2">
                          Enter your registered Work Email or Employee ID to receive an OTP.
                        </p>
                        <input
                          type="text"
                          value={forgotInput}
                          onChange={(e) => setForgotInput(e.target.value)}
                          className="w-full px-3 py-2 rounded-xl bg-[#09090c] border border-neutral-800 text-xs text-white outline-none focus:border-amber-500"
                          placeholder="e.g. pooja.i@apextech.com"
                          required
                        />
                        <button
                          type="submit"
                          className="w-full mt-3 py-2.5 rounded-xl bg-amber-500 text-neutral-950 font-bold text-xs"
                        >
                          Send OTP
                        </button>
                      </div>
                    )}

                    {forgotStep === 2 && (
                      <div>
                        <p className="text-xs text-neutral-300 mb-2">
                          Enter the 4-digit verification code sent to your email. (Demo OTP: <strong className="text-amber-400">1234</strong>)
                        </p>
                        <input
                          type="text"
                          maxLength="4"
                          value={forgotOtp}
                          onChange={(e) => setForgotOtp(e.target.value)}
                          className="w-full px-3 py-2 rounded-xl bg-[#09090c] border border-neutral-800 text-center tracking-widest text-lg font-bold text-amber-400 outline-none focus:border-amber-500"
                          placeholder="1234"
                          required
                        />
                        <button
                          type="submit"
                          className="w-full mt-3 py-2.5 rounded-xl bg-amber-500 text-neutral-950 font-bold text-xs"
                        >
                          Verify OTP
                        </button>
                      </div>
                    )}

                    {forgotStep === 3 && (
                      <div>
                        <p className="text-xs text-neutral-300 mb-2">
                          Enter your new secure password.
                        </p>
                        <input
                          type="password"
                          value={newPass}
                          onChange={(e) => setNewPass(e.target.value)}
                          className="w-full px-3 py-2 rounded-xl bg-[#09090c] border border-neutral-800 text-xs text-white outline-none focus:border-amber-500"
                          placeholder="Enter new password"
                          required
                        />
                        <button
                          type="submit"
                          className="w-full mt-3 py-2.5 rounded-xl bg-amber-500 text-neutral-950 font-bold text-xs"
                        >
                          Update Password
                        </button>
                      </div>
                    )}
                  </form>
                )}
              </div>
            </div>
          )}
        </div>
      );
    }
"@
$raw = [System.Text.RegularExpressions.Regex]::Replace($raw, $loginRegex, $newLogin)
Write-Host "5. Replaced LoginScreen"

# -----------------------------------------------------------------------------
# 6. Update App() Header, Slide-out Sidebar Drawer & Bottom Navigation
# -----------------------------------------------------------------------------
# Search from <header to </nav> in the main return of App()
$mainLayoutRegex = '(?s)<header className="sticky top-0 z-40 bg-brand-surface/95.*?\{/\* FIXED BOTTOM MOBILE NAVIGATION BAR \*/\}.*?</nav>'

$newMainLayout = @"
{/* ================================================================= */}
          {/* 1. TOP GLOBAL NAVIGATION BAR */}
          {/* ================================================================= */}
          <header className="sticky top-0 z-40 bg-brand-surface/95 backdrop-blur-md border-b border-brand-border px-3 sm:px-6 py-2.5 flex items-center justify-between gap-3">
            
            {/* Left: Brand Logo that opens Sidebar Drawer */}
            <div className="flex items-center gap-2">
              <button
                type="button"
                onClick={() => setIsSidebarOpen(true)}
                className="p-1.5 -ml-1 rounded-xl hover:bg-neutral-800 text-neutral-300 hover:text-white flex items-center gap-2 cursor-pointer transition-colors"
                title="Open Navigation Menu"
              >
                <Icon name="menu" className="w-5 h-5 text-amber-400" />
                <img 
                  src="/sora-hr-logo.svg" 
                  alt="SORA HR" 
                  className="h-6 sm:h-7 w-auto object-contain" 
                  onError={(e) => { e.target.src = '/sora-hr-logo.png'; }} 
                />
              </button>
            </div>

            {/* Center / Right: Live Clock & Shift Badge & User Greeting */}
            <div className="flex items-center gap-2.5">
              
              {/* Shift Clock & Live Status Pill */}
              <div className="flex items-center gap-2 bg-brand-dark px-2.5 py-1.5 rounded-xl border border-brand-border shadow-inner">
                <div className="flex items-center gap-1 text-xs font-mono">
                  <Icon name="clock" className="w-3.5 h-3.5 text-amber-400" />
                  <span className="font-bold text-white text-[11px] sm:text-xs">{currentTimeStr}</span>
                </div>

                <div className="h-3 w-px bg-brand-border"></div>

                {/* Shift Status Indicator */}
                <div className={`px-2 py-0.5 rounded-full text-[10px] font-bold border flex items-center gap-1.5 whitespace-nowrap \${
                  shiftState.status === 'WORKING' 
                    ? 'bg-emerald-500/15 text-emerald-400 border-emerald-500/30'
                    : shiftState.status === 'ON_BREAK'
                    ? 'bg-amber-500/15 text-amber-300 border-amber-500/30 animate-pulse'
                    : shiftState.status === 'COMPLETED'
                    ? 'bg-sky-500/15 text-sky-300 border-sky-500/30'
                    : 'bg-neutral-800 text-neutral-400 border-neutral-700'
                }\`}>
                  <span className={`w-1.5 h-1.5 rounded-full \${
                    shiftState.status === 'WORKING' ? 'bg-emerald-400 animate-ping' :
                    shiftState.status === 'ON_BREAK' ? 'bg-amber-400' :
                    shiftState.status === 'COMPLETED' ? 'bg-sky-400' : 'bg-neutral-500'
                  }\`}></span>
                  <span className="hidden xs:inline">
                    \${shiftState.status === 'WORKING' ? 'Clocked In' :
                      shiftState.status === 'ON_BREAK' ? 'On Break' :
                      shiftState.status === 'COMPLETED' ? 'Completed' : 'Punched Out'}
                  </span>
                </div>
              </div>

              {/* User Profile Avatar Pill */}
              <div className="flex items-center gap-2 pl-1 border-l border-brand-border">
                <div className="w-8 h-8 rounded-xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-extrabold flex items-center justify-center text-xs shadow-md">
                  {currentUser.avatar}
                </div>
                <div className="hidden md:block text-left leading-tight">
                  <div className="text-xs font-bold text-white truncate max-w-[120px]">{currentUser.name}</div>
                  <div className="text-[10px] text-amber-400 truncate max-w-[120px]">{currentUser.id}</div>
                </div>
              </div>

            </div>
          </header>

          {/* ================================================================= */}
          {/* SLIDE-OUT SIDEBAR DRAWER WITH LOCKED BACKGROUND                   */}
          {/* ================================================================= */}
          {isSidebarOpen && (
            <>
              {/* Full-screen backdrop overlay: completely intercepts clicks & locks background */}
              <div
                className="fixed inset-0 z-50 bg-black/80 backdrop-blur-md transition-opacity"
                onClick={() => setIsSidebarOpen(false)}
              />

              {/* Drawer Container */}
              <div className="fixed top-0 left-0 bottom-0 z-50 w-80 max-w-[85vw] bg-[#121217] border-r border-neutral-800 shadow-2xl flex flex-col justify-between p-5 overflow-y-auto animate-in slide-in-from-left duration-200">
                <div className="space-y-6">
                  {/* Drawer Header */}
                  <div className="flex items-center justify-between pb-3 border-b border-neutral-800">
                    <div className="flex items-center gap-2">
                      <img 
                        src="/sora-hr-logo.svg" 
                        alt="SORA HR" 
                        className="h-7 w-auto object-contain" 
                        onError={(e) => { e.target.src = '/sora-hr-logo.png'; }} 
                      />
                      <span className="text-[10px] text-amber-400 font-bold uppercase">ESS Drawer</span>
                    </div>
                    <button
                      type="button"
                      onClick={() => setIsSidebarOpen(false)}
                      className="p-1.5 rounded-lg text-neutral-400 hover:text-white hover:bg-neutral-800 transition-colors cursor-pointer"
                    >
                      <Icon name="x-circle" className="w-5 h-5" />
                    </button>
                  </div>

                  {/* User Profile Card */}
                  <div className="p-4 rounded-2xl bg-[#181822] border border-neutral-800 flex items-center gap-3">
                    <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-amber-400 to-amber-600 text-neutral-950 font-black text-base flex items-center justify-center shadow-lg">
                      {currentUser.avatar}
                    </div>
                    <div className="flex-1 min-w-0">
                      <div className="text-sm font-bold text-white truncate">{currentUser.name}</div>
                      <div className="text-[11px] text-amber-400 font-mono">{currentUser.id}</div>
                      <div className="text-[10px] text-neutral-400 truncate mt-0.5">{currentUser.designation}</div>
                    </div>
                  </div>

                  {/* Navigation Links */}
                  <div className="space-y-1">
                    <div className="px-3 pb-1 text-[10px] font-bold text-neutral-500 uppercase tracking-wider">Navigation</div>

                    <button
                      type="button"
                      onClick={() => { setActiveTab('dashboard'); setIsSidebarOpen(false); }}
                      className={`w-full px-3 py-2.5 rounded-xl font-bold text-xs flex items-center gap-3 transition-all cursor-pointer \${
                        activeTab === 'dashboard' ? 'bg-amber-400 text-neutral-950 shadow-md' : 'text-neutral-300 hover:text-white hover:bg-[#181822]'
                      }\`}>
                      <Icon name="clock" className="w-4 h-4" />
                      <span>Home / Dashboard</span>
                    </button>

                    <button
                      type="button"
                      onClick={() => { setActiveTab('attendance'); setIsSidebarOpen(false); }}
                      className={`w-full px-3 py-2.5 rounded-xl font-bold text-xs flex items-center justify-between transition-all cursor-pointer \${
                        activeTab === 'attendance' ? 'bg-amber-400 text-neutral-950 shadow-md' : 'text-neutral-300 hover:text-white hover:bg-[#181822]'
                      }\`}>
                      <div className="flex items-center gap-3">
                        <Icon name="calendar" className="w-4 h-4" />
                        <span>Attendance History</span>
                      </div>
                      <span className="px-1.5 py-0.5 rounded text-[10px] font-mono bg-[#09090c] text-neutral-300">
                        {attendanceLogs.length}
                      </span>
                    </button>

                    <button
                      type="button"
                      onClick={() => { setActiveTab('leaves'); setIsSidebarOpen(false); }}
                      className={`w-full px-3 py-2.5 rounded-xl font-bold text-xs flex items-center justify-between transition-all cursor-pointer \${
                        activeTab === 'leaves' ? 'bg-amber-400 text-neutral-950 shadow-md' : 'text-neutral-300 hover:text-white hover:bg-[#181822]'
                      }\`}>
                      <div className="flex items-center gap-3">
                        <Icon name="coffee" className="w-4 h-4" />
                        <span>Leave Management</span>
                      </div>
                      <span className="px-1.5 py-0.5 rounded text-[10px] font-mono bg-emerald-500/20 text-emerald-400 font-bold">
                        25d Total
                      </span>
                    </button>

                    <button
                      type="button"
                      onClick={() => { setActiveTab('payslips'); setIsSidebarOpen(false); }}
                      className={`w-full px-3 py-2.5 rounded-xl font-bold text-xs flex items-center gap-3 transition-all cursor-pointer \${
                        activeTab === 'payslips' ? 'bg-amber-400 text-neutral-950 shadow-md' : 'text-neutral-300 hover:text-white hover:bg-[#181822]'
                      }\`}>
                      <Icon name="file-text" className="w-4 h-4" />
                      <span>Salary Slip &amp; PDF</span>
                    </button>
                  </div>

                  {/* App Preferences & Settings */}
                  <div className="space-y-2 pt-2 border-t border-neutral-800">
                    <div className="px-3 text-[10px] font-bold text-neutral-500 uppercase tracking-wider">Quick Settings</div>
                    
                    <div className="p-3 rounded-xl bg-[#14141a] border border-neutral-800/80 space-y-2.5 text-xs">
                      <div className="flex items-center justify-between">
                        <span className="text-neutral-300 text-[11px]">Shift Reminders</span>
                        <span className="text-[10px] font-bold text-emerald-400">Enabled</span>
                      </div>
                      <div className="flex items-center justify-between">
                        <span className="text-neutral-300 text-[11px]">Biometric Auth</span>
                        <span className="text-[10px] font-bold text-emerald-400">Active</span>
                      </div>
                      <div className="flex items-center justify-between">
                        <span className="text-neutral-300 text-[11px]">Geofence Alert (BKC)</span>
                        <span className="text-[10px] font-bold text-sky-400">In Range</span>
                      </div>
                    </div>
                  </div>
                </div>

                {/* Drawer Footer with Red Sign Out Button */}
                <div className="space-y-3 pt-4 border-t border-neutral-800">
                  <button
                    type="button"
                    onClick={() => {
                      setIsSidebarOpen(false);
                      setIsLoggedIn(false);
                      setCurrentScreen('welcome');
                      addToast('Signed Out', 'You have been safely signed out.', 'info');
                    }}
                    className="w-full py-3 px-4 rounded-xl bg-rose-500/10 hover:bg-rose-500/20 text-rose-400 font-bold text-xs flex items-center justify-center gap-2 border border-rose-500/20 active:scale-95 transition-all cursor-pointer"
                  >
                    <Icon name="log-out" className="w-4 h-4 text-rose-400" />
                    <span>Sign Out</span>
                  </button>

                  <p className="text-center text-[10px] text-neutral-500">
                    SORA HR v2.4 ESS &bull; Apex Global Technologies
                  </p>
                </div>
              </div>
            </>
          )}

          {/* ================================================================= */}
          {/* 2. BODY LAYOUT: MAIN CONTENT AREA WITH FULL BOTTOM CLEARANCE       */}
          {/* ================================================================= */}
          <div className="flex-1 flex overflow-hidden">
            
            {/* MAIN WORKSPACE CONTENT CONTAINER (pb-36 prevents bottom nav overlap) */}
            <main className="flex-1 overflow-y-auto p-4 sm:p-6 lg:p-8 pb-36 space-y-6">
              
              {/* TAB 1: OVERVIEW & CLOCK DASHBOARD */}
              {activeTab === 'dashboard' && (
                <DashboardTab
                  currentUser={currentUser}
                  currentDateStr={currentDateStr}
                  shiftState={shiftState}
                  elapsedShiftSeconds={elapsedShiftSeconds}
                  elapsedBreakSeconds={elapsedBreakSeconds}
                  formatDuration={formatDuration}
                  hrSettings={hrSettings}
                  onQuickClockIn={() => finalizeClockIn(null)}
                  onSelfieClockIn={() => setIsSelfieModalOpen(true)}
                  onOpenQrModal={() => setIsQrModalOpen(true)}
                  onQuickClockOut={handleClockOut}
                  onSelfieClockOut={() => setIsSelfieModalOpen(true)}
                  onOpenBreakModal={() => setIsBreakModalOpen(true)}
                  onEndBreak={endBreak}
                  attendanceLogs={attendanceLogs}
                  leaveQuotas={leaveQuotas}
                  onSwitchTab={(t) => setActiveTab(t)}
                  onOpenApplyLeave={() => setIsApplyLeaveModalOpen(true)}
                />
              )}

              {/* TAB 2: ATTENDANCE VIEW */}
              {activeTab === 'attendance' && (
                <AttendanceViewTab
                  attendanceLogs={attendanceLogs}
                  hrSettings={hrSettings}
                  onPreviewSelfie={(url) => setPreviewSelfieUrl(url)}
                />
              )}

              {/* TAB 3: LEAVE MANAGEMENT */}
              {activeTab === 'leaves' && (
                <LeaveManagementTab
                  currentUser={currentUser}
                  leaveQuotas={leaveQuotas}
                  leaveRequests={leaveRequests}
                  onOpenApplyLeave={() => setIsApplyLeaveModalOpen(true)}
                  onCancelLeave={(id) => {
                    setLeaveRequests(prev => prev.filter(r => r.id !== id));
                    addToast('Leave Withdrawn', 'Your leave request has been canceled.', 'info');
                  }}
                />
              )}

              {/* TAB 4: SALARY SLIP */}
              {activeTab === 'payslips' && (
                <SalarySlipTab
                  currentUser={currentUser}
                  attendanceLogs={attendanceLogs}
                />
              )}

            </main>
          </div>

          {/* ================================================================= */}
          {/* FIXED BOTTOM MOBILE NAVIGATION BAR (Strictly 4 Employee Tabs)    */}
          {/* ================================================================= */}
          <nav className="fixed bottom-0 left-0 right-0 z-40 bg-[#121217]/95 backdrop-blur-xl border-t border-brand-border px-3 py-2 flex items-center justify-around shadow-2xl">
            <button
              onClick={() => setActiveTab('dashboard')}
              className={`flex flex-col items-center gap-1 py-1 px-3 rounded-xl transition-all cursor-pointer \${
                activeTab === 'dashboard' ? 'text-amber-400 font-bold' : 'text-neutral-400 hover:text-white'
              }\`}>
              <div className={`p-1.5 rounded-xl transition-all \${activeTab === 'dashboard' ? 'bg-amber-400/15 text-amber-400 scale-105' : ''}\`}>
                <Icon name="clock" className="w-5 h-5" />
              </div>
              <span className="text-[10px] tracking-tight">Home</span>
            </button>

            <button
              onClick={() => setActiveTab('attendance')}
              className={`flex flex-col items-center gap-1 py-1 px-3 rounded-xl transition-all cursor-pointer \${
                activeTab === 'attendance' ? 'text-amber-400 font-bold' : 'text-neutral-400 hover:text-white'
              }\`}>
              <div className={`p-1.5 rounded-xl transition-all \${activeTab === 'attendance' ? 'bg-amber-400/15 text-amber-400 scale-105' : ''}\`}>
                <Icon name="calendar" className="w-5 h-5" />
              </div>
              <span className="text-[10px] tracking-tight">Attendance</span>
            </button>

            <button
              onClick={() => setActiveTab('leaves')}
              className={`flex flex-col items-center gap-1 py-1 px-3 rounded-xl transition-all cursor-pointer \${
                activeTab === 'leaves' ? 'text-amber-400 font-bold' : 'text-neutral-400 hover:text-white'
              }\`}>
              <div className={`p-1.5 rounded-xl transition-all \${activeTab === 'leaves' ? 'bg-amber-400/15 text-amber-400 scale-105' : ''}\`}>
                <Icon name="coffee" className="w-5 h-5" />
              </div>
              <span className="text-[10px] tracking-tight">Leaves</span>
            </button>

            <button
              onClick={() => setActiveTab('payslips')}
              className={`flex flex-col items-center gap-1 py-1 px-3 rounded-xl transition-all cursor-pointer \${
                activeTab === 'payslips' ? 'text-amber-400 font-bold' : 'text-neutral-400 hover:text-white'
              }\`}>
              <div className={`p-1.5 rounded-xl transition-all \${activeTab === 'payslips' ? 'bg-amber-400/15 text-amber-400 scale-105' : ''}\`}>
                <Icon name="file-text" className="w-5 h-5" />
              </div>
              <span className="text-[10px] tracking-tight">Payslip</span>
            </button>
          </nav>
"@

$raw = [System.Text.RegularExpressions.Regex]::Replace($raw, $mainLayoutRegex, $newMainLayout)
Write-Host "6. Replaced main App layout with Header, Drawer, pb-36 and 4 tabs bottom nav"

# -----------------------------------------------------------------------------
# 7. Replace DashboardTab (Vertical stacked policy, 3 clock in buttons)
# -----------------------------------------------------------------------------
$dashRegex = '(?s)// SUB-COMPONENT: TAB 1 -- DASHBOARD.*?function DashboardTab\(.*?return\s*\(.*?\);\s*\}'
$newDash = @"
// SUB-COMPONENT: TAB 1 -- DASHBOARD
    // =========================================================================
    function DashboardTab({
      currentUser,
      currentDateStr,
      shiftState,
      elapsedShiftSeconds,
      elapsedBreakSeconds,
      formatDuration,
      hrSettings,
      onQuickClockIn,
      onSelfieClockIn,
      onOpenQrModal,
      onQuickClockOut,
      onSelfieClockOut,
      onOpenBreakModal,
      onEndBreak,
      attendanceLogs,
      leaveQuotas,
      onSwitchTab,
      onOpenApplyLeave
    }) {
      return (
        <div className="space-y-5">
          {/* Top Banner Greeting */}
          <div className="p-5 sm:p-6 rounded-3xl bg-gradient-to-r from-[#17171d] via-[#101014] to-[#17171d] border border-neutral-800 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
            <div>
              <div className="text-xs font-semibold text-amber-400 flex items-center gap-1.5 mb-1">
                <span>Apex Global Technologies</span>
                <span>&bull;</span>
                <span>{currentDateStr}</span>
              </div>
              <h2 className="text-xl sm:text-2xl font-black text-white">
                Hello, {currentUser.name}!
              </h2>
              <p className="text-xs text-neutral-400 mt-1">
                {currentUser.designation} &bull; {currentUser.department}
              </p>
            </div>

            <div className="px-3 py-1.5 rounded-xl bg-[#14141a] border border-neutral-800 text-xs font-mono text-neutral-300 self-start sm:self-auto">
              {currentUser.shift}
            </div>
          </div>

          {/* HR Punch Policy Card - VERTICALLY STACKED (uper-niche) as requested */}
          <div className="p-4 sm:p-5 rounded-2xl bg-[#14141a] border border-neutral-800 space-y-3">
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-2 text-xs font-bold text-neutral-200">
                <Icon name="shield-check" className="w-4 h-4 text-amber-400" />
                <span>Attendance Verification Policy</span>
              </div>
              <span className="text-[10px] text-neutral-400 font-mono">BKC Campus Guidelines</span>
            </div>

            <div className="space-y-2.5">
              {/* Policy Micro-Card 1 (Top): Live Facial Selfie */}
              <div className="p-3 rounded-xl bg-[#1a1a24] border border-neutral-800 flex items-center justify-between gap-3">
                <div className="flex items-center gap-3">
                  <div className="w-9 h-9 rounded-xl bg-amber-500/10 border border-amber-500/20 text-amber-400 flex items-center justify-center flex-shrink-0">
                    <Icon name="camera" className="w-4 h-4" />
                  </div>
                  <div>
                    <div className="text-xs font-bold text-white">Live Facial Selfie Validation</div>
                    <div className="text-[10px] text-neutral-400">Anti-spoofing camera check required during punch</div>
                  </div>
                </div>
                <span className="px-2.5 py-1 rounded-full bg-emerald-500/15 border border-emerald-500/30 text-emerald-400 text-[10px] font-bold whitespace-nowrap">
                  Active
                </span>
              </div>

              {/* Policy Micro-Card 2 (Bottom): Geofencing */}
              <div className="p-3 rounded-xl bg-[#1a1a24] border border-neutral-800 flex items-center justify-between gap-3">
                <div className="flex items-center gap-3">
                  <div className="w-9 h-9 rounded-xl bg-sky-500/10 border border-sky-500/20 text-sky-400 flex items-center justify-center flex-shrink-0">
                    <Icon name="map-pin" className="w-4 h-4" />
                  </div>
                  <div>
                    <div className="text-xs font-bold text-white">BKC Campus GPS Geofencing (100m)</div>
                    <div className="text-[10px] text-neutral-400">SORA Tech Park, G-Block &bull; Distance: 28m away</div>
                  </div>
                </div>
                <span className="px-2.5 py-1 rounded-full bg-emerald-500/15 border border-emerald-500/30 text-emerald-400 text-[10px] font-bold whitespace-nowrap">
                  In Range (28m)
                </span>
              </div>
            </div>
          </div>

          {/* Core Interactive Shift Attendance Station */}
          <div className="p-5 sm:p-7 rounded-3xl bg-[#14141a] border border-neutral-800 shadow-xl space-y-5 relative overflow-hidden">
            <div className="flex items-center justify-between pb-3 border-b border-neutral-800">
              <div>
                <span className="text-[10px] font-bold text-amber-400 uppercase tracking-wider">Live Shift &amp; Punch Desk</span>
                <h3 className="text-base font-bold text-white flex items-center gap-2 mt-0.5">
                  <Icon name="clock" className="w-4 h-4 text-amber-400" />
                  <span>Real-Time Shift Attendance Controller</span>
                </h3>
              </div>
            </div>

            {/* Stopwatch Tickers */}
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
              <div className="p-4 rounded-2xl bg-[#0c0c10] border border-neutral-800 flex flex-col justify-between">
                <div className="text-[10px] text-neutral-400 font-semibold uppercase flex items-center justify-between">
                  <span>Productive Work Hours</span>
                  <span className={`w-2 h-2 rounded-full \${shiftState.status === 'WORKING' ? 'bg-emerald-400 animate-pulse' : 'bg-neutral-600'}\`}></span>
                </div>
                <div className="text-2xl sm:text-3xl font-extrabold font-mono text-emerald-400 mt-2">
                  \${shiftState.status === 'WORKING' ? formatDuration(elapsedShiftSeconds) :
                   shiftState.status === 'COMPLETED' ? (attendanceLogs[0]?.hours || '08h 15m') : '00:00:00'}
                </div>
                <div className="text-[9px] text-neutral-500 font-mono mt-1">
                  Target Shift: 09h 00m &bull; General Shift
                </div>
              </div>

              <div className="p-4 rounded-2xl bg-[#0c0c10] border border-neutral-800 flex flex-col justify-between">
                <div className="text-[10px] text-neutral-400 font-semibold uppercase flex items-center justify-between">
                  <span>Break Stopwatch</span>
                  <Icon name="coffee" className="w-3.5 h-3.5 text-amber-400" />
                </div>
                <div className="text-2xl sm:text-3xl font-extrabold font-mono text-amber-300 mt-2">
                  \${shiftState.status === 'ON_BREAK' ? formatDuration(elapsedBreakSeconds) :
                   formatDuration(shiftState.totalBreakSeconds)}
                </div>
                <div className="text-[9px] text-neutral-500 font-mono mt-1">
                  \${shiftState.status === 'ON_BREAK' ? `Currently on: \${shiftState.currentBreakName}` : 'Permitted lunch/tea: 60m'}
                </div>
              </div>

              <div className="p-4 rounded-2xl bg-[#0c0c10] border border-neutral-800 flex flex-col justify-between">
                <div className="text-[10px] text-neutral-400 font-semibold uppercase flex items-center justify-between">
                  <span>Geofence Proximity</span>
                  <Icon name="map-pin" className="w-3.5 h-3.5 text-sky-400" />
                </div>
                <div className="text-xl font-extrabold text-white mt-2">
                  BKC Office
                </div>
                <div className="text-[10px] text-emerald-400 font-mono mt-1 flex items-center gap-1.5">
                  <span className="w-1.5 h-1.5 rounded-full bg-emerald-400"></span>
                  <span>Within 28m of 100m Office Radius</span>
                </div>
              </div>
            </div>

            {/* FLEXIBLE CLOCK IN / OUT ACTIONS (Selfie, Touchless Quick, and QR) */}
            <div className="space-y-3 pt-2">
              {/* When NOT Clocked In or Shift Completed */}
              {(shiftState.status === 'NOT_CLOCKED_IN' || shiftState.status === 'COMPLETED') && (
                <div className="space-y-2.5">
                  <div className="text-[11px] font-bold text-neutral-400 uppercase tracking-wider">Choose Clock-In Method:</div>
                  <div className="grid grid-cols-1 sm:grid-cols-3 gap-2.5">
                    {/* Method 1: Quick 1-Tap Clock In (Direct touchless, no camera) */}
                    <button
                      type="button"
                      onClick={onQuickClockIn}
                      className="py-3.5 px-4 rounded-2xl bg-gradient-to-r from-emerald-500 to-emerald-600 hover:from-emerald-400 hover:to-emerald-500 text-neutral-950 font-black text-xs flex items-center justify-center gap-2 shadow-lg shadow-emerald-500/20 active:scale-95 transition-all cursor-pointer"
                    >
                      <Icon name="zap" className="w-4 h-4" />
                      <span>⚡ Quick 1-Tap Clock In</span>
                    </button>

                    {/* Method 2: Selfie Clock In (Live camera photo) */}
                    <button
                      type="button"
                      onClick={onSelfieClockIn}
                      className="py-3.5 px-4 rounded-2xl bg-[#1e1e28] hover:bg-[#252532] text-amber-400 font-black text-xs flex items-center justify-center gap-2 border border-amber-500/30 active:scale-95 transition-all cursor-pointer"
                    >
                      <Icon name="camera" className="w-4 h-4" />
                      <span>📸 Selfie Clock In</span>
                    </button>

                    {/* Method 3: Scan Office QR */}
                    <button
                      type="button"
                      onClick={onOpenQrModal}
                      className="py-3.5 px-4 rounded-2xl bg-[#1e1e28] hover:bg-[#252532] text-neutral-200 font-black text-xs flex items-center justify-center gap-2 border border-neutral-800 active:scale-95 transition-all cursor-pointer"
                    >
                      <Icon name="qr-code" className="w-4 h-4 text-sky-400" />
                      <span>📱 Scan Office QR</span>
                    </button>
                  </div>
                </div>
              )}

              {/* When WORKING */}
              {shiftState.status === 'WORKING' && (
                <div className="grid grid-cols-1 sm:grid-cols-3 gap-2.5">
                  {/* Break Button */}
                  <button
                    type="button"
                    onClick={onOpenBreakModal}
                    className="py-3.5 px-4 rounded-2xl bg-amber-500 hover:bg-amber-400 text-neutral-950 font-black text-xs flex items-center justify-center gap-2 shadow-lg shadow-amber-500/20 active:scale-95 transition-all cursor-pointer"
                  >
                    <Icon name="coffee" className="w-4 h-4" />
                    <span>☕ Take a Break</span>
                  </button>

                  {/* Quick Clock Out */}
                  <button
                    type="button"
                    onClick={onQuickClockOut}
                    className="py-3.5 px-4 rounded-2xl bg-rose-500/15 hover:bg-rose-500/25 text-rose-400 font-black text-xs flex items-center justify-center gap-2 border border-rose-500/30 active:scale-95 transition-all cursor-pointer"
                  >
                    <Icon name="log-out" className="w-4 h-4" />
                    <span>⚡ 1-Tap Clock Out</span>
                  </button>

                  {/* Selfie Clock Out */}
                  <button
                    type="button"
                    onClick={onSelfieClockOut}
                    className="py-3.5 px-4 rounded-2xl bg-[#1e1e28] hover:bg-[#252532] text-amber-400 font-black text-xs flex items-center justify-center gap-2 border border-amber-500/30 active:scale-95 transition-all cursor-pointer"
                  >
                    <Icon name="camera" className="w-4 h-4" />
                    <span>📸 Selfie Clock Out</span>
                  </button>
                </div>
              )}

              {/* When ON BREAK */}
              {shiftState.status === 'ON_BREAK' && (
                <button
                  type="button"
                  onClick={onEndBreak}
                  className="w-full py-3.5 px-4 rounded-2xl bg-emerald-500 hover:bg-emerald-400 text-neutral-950 font-black text-xs flex items-center justify-center gap-2 shadow-lg shadow-emerald-500/20 active:scale-95 transition-all cursor-pointer animate-pulse"
                >
                  <Icon name="clock" className="w-4 h-4" />
                  <span>▶️ End Break &amp; Resume Shift</span>
                </button>
              )}
            </div>
          </div>

          {/* Quick Action Tiles */}
          <div className="grid grid-cols-3 gap-3">
            <button
              type="button"
              onClick={() => onSwitchTab('attendance')}
              className="p-3.5 rounded-2xl bg-[#14141a] hover:bg-[#1a1a24] border border-neutral-800 text-left space-y-1 transition-all cursor-pointer"
            >
              <div className="w-8 h-8 rounded-xl bg-amber-500/10 text-amber-400 flex items-center justify-center">
                <Icon name="calendar" className="w-4 h-4" />
              </div>
              <div className="text-xs font-bold text-white pt-1">Register</div>
              <div className="text-[10px] text-neutral-400">View Month Log</div>
            </button>

            <button
              type="button"
              onClick={onOpenApplyLeave}
              className="p-3.5 rounded-2xl bg-[#14141a] hover:bg-[#1a1a24] border border-neutral-800 text-left space-y-1 transition-all cursor-pointer"
            >
              <div className="w-8 h-8 rounded-xl bg-emerald-500/10 text-emerald-400 flex items-center justify-center">
                <Icon name="plus" className="w-4 h-4" />
              </div>
              <div className="text-xs font-bold text-white pt-1">Apply Leave</div>
              <div className="text-[10px] text-neutral-400">CL / SL / PL</div>
            </button>

            <button
              type="button"
              onClick={() => onSwitchTab('payslips')}
              className="p-3.5 rounded-2xl bg-[#14141a] hover:bg-[#1a1a24] border border-neutral-800 text-left space-y-1 transition-all cursor-pointer"
            >
              <div className="w-8 h-8 rounded-xl bg-sky-500/10 text-sky-400 flex items-center justify-center">
                <Icon name="file-text" className="w-4 h-4" />
              </div>
              <div className="text-xs font-bold text-white pt-1">Salary Slip</div>
              <div className="text-[10px] text-neutral-400">PDF Download</div>
            </button>
          </div>

          {/* Leave Quotas Summary Card */}
          <div className="p-4 sm:p-5 rounded-2xl bg-[#14141a] border border-neutral-800 space-y-3">
            <div className="flex items-center justify-between">
              <span className="text-xs font-bold text-white flex items-center gap-1.5">
                <Icon name="coffee" className="w-4 h-4 text-amber-400" />
                <span>Available Leave Balances</span>
              </span>
              <button
                type="button"
                onClick={onOpenApplyLeave}
                className="text-[10px] font-bold text-amber-400 hover:underline cursor-pointer"
              >
                + Request Leave
              </button>
            </div>

            <div className="grid grid-cols-3 gap-2.5">
              <div className="p-3 rounded-xl bg-[#0c0c10] border border-neutral-800 text-center">
                <div className="text-[10px] text-neutral-400">Casual (CL)</div>
                <div className="text-lg font-black text-amber-400 mt-0.5">8</div>
                <div className="text-[9px] text-neutral-500">of 12 Days</div>
              </div>
              <div className="p-3 rounded-xl bg-[#0c0c10] border border-neutral-800 text-center">
                <div className="text-[10px] text-neutral-400">Sick (SL)</div>
                <div className="text-lg font-black text-rose-400 mt-0.5">5</div>
                <div className="text-[9px] text-neutral-500">of 8 Days</div>
              </div>
              <div className="p-3 rounded-xl bg-[#0c0c10] border border-neutral-800 text-center">
                <div className="text-[10px] text-neutral-400">Privilege (PL)</div>
                <div className="text-lg font-black text-emerald-400 mt-0.5">12</div>
                <div className="text-[9px] text-neutral-500">of 18 Days</div>
              </div>
            </div>
          </div>

          {/* Bottom clearance spacer so nothing gets blocked by bottom nav */}
          <div className="h-28 w-full pointer-events-none"></div>
        </div>
      );
    }
"@
$raw = [System.Text.RegularExpressions.Regex]::Replace($raw, $dashRegex, $newDash)
Write-Host "7. Replaced DashboardTab with stacked policy cards & flexible clock in"

# -----------------------------------------------------------------------------
# 8. Clean up all non-ASCII / broken bytes in the file
# -----------------------------------------------------------------------------
$raw = $raw.Replace("👋", "")
$raw = $raw.Replace("✓", "")
$raw = $raw.Replace("•", "-")
$raw = $raw.Replace("—", "-")
$raw = $raw.Replace("₹", "Rs ")
$raw = $raw.Replace("âœ“", "")
$raw = $raw.Replace("â€¢", "-")
$raw = $raw.Replace("â‚¹", "Rs ")
$raw = $raw.Replace("", "")

# -----------------------------------------------------------------------------
# 9. Save file in UTF-8
# -----------------------------------------------------------------------------
[System.IO.File]::WriteAllText($destPath, $raw, [System.Text.Encoding]::UTF8)
Write-Host "Successfully written $destPath (Size: $((Get-Item $destPath).Length) bytes)"

if (Test-Path "d:\sora-hr-frontend\public\user") {
    [System.IO.File]::WriteAllText($publicPath, $raw, [System.Text.Encoding]::UTF8)
    Write-Host "Successfully synced to $publicPath"
}

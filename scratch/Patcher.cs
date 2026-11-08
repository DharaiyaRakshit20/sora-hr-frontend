using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Patcher
{
    static int Main()
    {
        try
        {
            string basePath = @"d:\sora-hr-frontend";
            string sourcePath = Path.Combine(basePath, @"user\index.html.bak");
            string destPath = Path.Combine(basePath, @"user\index.html");
            string publicPath = Path.Combine(basePath, @"public\user\index.html");
            string scratchPath = Path.Combine(basePath, "scratch");

            Console.WriteLine("Reading " + sourcePath);
            string text = File.ReadAllText(sourcePath, Encoding.UTF8);

            // 0. Error boundary in <head>
            if (!text.Contains("debug-error-box"))
            {
                string errorBoxScript = "  <script>\n" +
                    "    window.onerror = function(msg, url, line, col, error) {\n" +
                    "      console.error('JS ERROR:', msg, 'line:', line, error);\n" +
                    "      var box = document.getElementById('debug-error-box');\n" +
                    "      if (!box) {\n" +
                    "        box = document.createElement('div');\n" +
                    "        box.id = 'debug-error-box';\n" +
                    "        box.style = 'position:fixed;top:10px;left:10px;right:10px;z-index:99999;background:#2d1215;color:#fca5a5;border:1px solid #f87171;border-radius:12px;padding:14px;font-family:sans-serif;font-size:12px;box-shadow:0 10px 25px rgba(0,0,0,0.5);';\n" +
                    "        document.body.appendChild(box);\n" +
                    "      }\n" +
                    "      box.innerHTML = '<strong style=\"color:#ef4444;\">Runtime Script Error:</strong> ' + msg + ' (Line ' + line + ')';\n" +
                    "    };\n" +
                    "  </script>\n</head>";
                text = text.Replace("</head>", errorBoxScript);
                Console.WriteLine("0. Added global error boundary to <head>");
            }

            // 1. Icon additions
            string iconAdditions = File.ReadAllText(Path.Combine(scratchPath, "icon_additions.txt"), Encoding.UTF8);
            string oldIcon = "'clock': <path d=\"M12 2a10 10 0 1 0 10 10A10 10 0 0 0 12 2zm0 18a8 8 0 1 1 8-8 8 8 0 0 1-8 8zm1-13h-2v6l5.25 3.15.75-1.23-4-2.42z\"/>,";
            if (text.Contains(oldIcon))
            {
                text = text.Replace(oldIcon, iconAdditions.Trim());
                Console.WriteLine("1. Added icons (menu, lock, mail, zap)");
            }

            // 2. calculateDistance
            string distCode = File.ReadAllText(Path.Combine(scratchPath, "dist.txt"), Encoding.UTF8);
            Regex distRegex = new Regex(@"(?s)function calculateDistance\(lat1, lon1, lat2, lon2\)\s*\{.*?return Math\.round\(R \* c\);\s*\}");
            text = distRegex.Replace(text, distCode.Trim());
            Console.WriteLine("2. Fixed calculateDistance");

            // 3. isSidebarOpen & scroll lock in App()
            string oldAppState = "const [currentScreen, setCurrentScreen] = useState('welcome');";
            string newAppState = "const [currentScreen, setCurrentScreen] = useState('welcome');\n" +
                                 "      const [isSidebarOpen, setIsSidebarOpen] = useState(false);\n\n" +
                                 "      // Lock background body scroll when sidebar drawer is open\n" +
                                 "      useEffect(() => {\n" +
                                 "        if (isSidebarOpen) {\n" +
                                 "          document.body.style.overflow = 'hidden';\n" +
                                 "        } else {\n" +
                                 "          document.body.style.overflow = '';\n" +
                                 "        }\n" +
                                 "        return () => {\n" +
                                 "          document.body.style.overflow = '';\n" +
                                 "        };\n" +
                                 "      }, [isSidebarOpen]);";
            if (text.Contains(oldAppState) && !text.Contains("isSidebarOpen"))
            {
                text = text.Replace(oldAppState, newAppState);
                Console.WriteLine("3. Added isSidebarOpen state & scroll lock");
            }

            // 4. WelcomeScreen
            string welcomeCode = File.ReadAllText(Path.Combine(scratchPath, "welcome.txt"), Encoding.UTF8);
            Regex welcomeRegex = new Regex(@"(?s)// SUB-COMPONENT: SCREEN 1 -- WELCOME / ONBOARDING SCREEN.*?function WelcomeScreen\(\{ onNext \}\)\s*\{.*?return\s*\(.*?\);\s*\}");
            text = welcomeRegex.Replace(text, welcomeCode.Trim());
            Console.WriteLine("4. Replaced WelcomeScreen");

            // 5. LoginScreen
            string loginCode = File.ReadAllText(Path.Combine(scratchPath, "login.txt"), Encoding.UTF8);
            Regex loginRegex = new Regex(@"(?s)// SUB-COMPONENT: SCREEN 2 -- DEDICATED MOBILE LOGIN SCREEN.*?function LoginScreen\(.*?return\s*\(.*?\);\s*\}");
            text = loginRegex.Replace(text, loginCode.Trim());
            Console.WriteLine("5. Replaced LoginScreen");

            // 6a. Replace Header & Desktop Aside with Mobile Header & Slide-out Drawer
            string headerStart = "<header className=\"sticky top-0 z-40 bg-brand-surface/95 backdrop-blur-md border-b border-brand-border px-4 sm:px-6 py-2.5 flex items-center justify-between gap-4\">";
            string mainStart = "<main className=\"flex-1 overflow-y-auto p-4 sm:p-6 lg:p-8 space-y-6\">";
            int idxHeader = text.IndexOf(headerStart);
            int idxMain = text.IndexOf(mainStart);
            if (idxHeader != -1 && idxMain != -1)
            {
                string newTop = File.ReadAllText(Path.Combine(scratchPath, "top_header_and_drawer.txt"), Encoding.UTF8);
                text = text.Substring(0, idxHeader) + newTop.Trim() + "\n" + text.Substring(idxMain + mainStart.Length);
                Console.WriteLine("6a. Replaced Header & Aside with Mobile Header, Drawer, and main pb-36");
            }
            else
            {
                Console.WriteLine("WARNING: Could not find headerStart or mainStart (" + idxHeader + ", " + idxMain + ")");
            }

            // 6b. Replace Bottom Nav with 4 tabs
            string navStart = "<nav className=\"fixed bottom-0 left-0 right-0 z-40 bg-[#121217]/95 backdrop-blur-xl border-t border-brand-border px-2 py-1.5 flex items-center justify-around shadow-2xl\">";
            int idxNav = text.IndexOf(navStart);
            if (idxNav != -1)
            {
                int idxNavEnd = text.IndexOf("</nav>", idxNav);
                if (idxNavEnd != -1)
                {
                    string newBottomNav = File.ReadAllText(Path.Combine(scratchPath, "bottom_nav_4tabs.txt"), Encoding.UTF8);
                    text = text.Substring(0, idxNav) + newBottomNav.Trim() + text.Substring(idxNavEnd + 6);
                    Console.WriteLine("6b. Replaced Bottom Nav with strictly 4 tabs");
                }
            }
            else
            {
                Console.WriteLine("WARNING: Could not find navStart (" + idxNav + ")");
            }

            // 7. DashboardTab
            string dashCode = File.ReadAllText(Path.Combine(scratchPath, "dashboard.txt"), Encoding.UTF8);
            Regex dashRegex = new Regex(@"(?s)// SUB-COMPONENT: TAB 1 -- DASHBOARD.*?function DashboardTab\(.*?return\s*\(.*?\);\s*\}");
            text = dashRegex.Replace(text, dashCode.Trim());
            Console.WriteLine("7. Replaced DashboardTab");

            // 8. Clean non-ASCII / mangled characters
            text = text.Replace("\uD83D\uDC4B", ""); // waving hand
            text = text.Replace("\u2713", ""); // checkmark
            text = text.Replace("\u2022", "-"); // bullet
            text = text.Replace("\u2014", "-"); // em-dash
            text = text.Replace("\u2013", "-"); // en-dash
            text = text.Replace("\u20B9", "Rs "); // Indian rupee symbol
            text = text.Replace("âœ“", "");
            text = text.Replace("â€¢", "-");
            text = text.Replace("â‚¹", "Rs ");

            // 9. Write destination
            File.WriteAllText(destPath, text, new UTF8Encoding(false));
            Console.WriteLine("Successfully wrote " + destPath + " (Bytes: " + new FileInfo(destPath).Length + ")");

            if (Directory.Exists(Path.GetDirectoryName(publicPath)))
            {
                File.WriteAllText(publicPath, text, new UTF8Encoding(false));
                Console.WriteLine("Successfully synced " + publicPath);
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("FATAL ERROR: " + ex.ToString());
            return 1;
        }
    }
}

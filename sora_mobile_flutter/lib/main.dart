import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:webview_flutter/webview_flutter.dart';
import 'package:webview_flutter_android/webview_flutter_android.dart';
import 'package:permission_handler/permission_handler.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  SystemChrome.setSystemUIOverlayStyle(
    const SystemUiOverlayStyle(
      statusBarColor: Colors.transparent,
      statusBarIconBrightness: Brightness.light,
      systemNavigationBarColor: Color(0xFF09090C),
      systemNavigationBarIconBrightness: Brightness.light,
    ),
  );
  runApp(const SoraHrApp());
}

class SoraHrApp extends StatelessWidget {
  const SoraHrApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'SORA HR',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        brightness: Brightness.dark,
        scaffoldBackgroundColor: const Color(0xFF09090C),
        colorScheme: const ColorScheme.dark(
          primary: Color(0xFFF59E0B),
          secondary: Color(0xFFFBBF24),
          surface: Color(0xFF17171D),
        ),
      ),
      home: const SoraWebViewScreen(),
    );
  }
}

class SoraWebViewScreen extends StatefulWidget {
  const SoraWebViewScreen({super.key});

  @override
  State<SoraWebViewScreen> createState() => _SoraWebViewScreenState();
}

class _SoraWebViewScreenState extends State<SoraWebViewScreen> {
  late WebViewController _controller;
  bool _isLoading = true;
  bool _hasError = false;
  String _errorMessage = '';
  double _loadingProgress = 0.0;

  // Default server address (can be updated via in-app settings if IP changes)
  String appUrl = 'http://192.168.31.192:8080/user/';

  @override
  void initState() {
    super.initState();
    _requestAppPermissions();
    _initWebView();
  }

  // Request Camera & GPS Permissions for Geofencing & Selfie attendance
  Future<void> _requestAppPermissions() async {
    try {
      await [
        Permission.camera,
        Permission.locationWhenInUse,
        Permission.microphone,
      ].request();
    } catch (e) {
      debugPrint("Permission request error: $e");
    }
  }

  void _initWebView() {
    _controller = WebViewController()
      ..setJavaScriptMode(JavaScriptMode.unrestricted)
      ..setBackgroundColor(const Color(0xFF09090C))
      ..setNavigationDelegate(
        NavigationDelegate(
          onProgress: (progress) {
            setState(() {
              _loadingProgress = progress / 100;
            });
          },
          onPageStarted: (url) {
            setState(() {
              _isLoading = true;
              _hasError = false;
            });
          },
          onPageFinished: (url) {
            setState(() {
              _isLoading = false;
            });
          },
          onWebResourceError: (error) {
            // Only trigger full-screen error if the MAIN FRAME itself failed to load
            // Sub-resource failures (fonts, map tiles, images, aborted requests) must not crash the app
            if (error.isForMainFrame == true && error.errorCode != -3) {
              setState(() {
                _isLoading = false;
                _hasError = true;
                _errorMessage = error.description;
              });
            } else {
              debugPrint("Ignored non-main frame resource error: ${error.description}");
            }
          },
        ),
      );

    // Auto-grant Camera and WebRTC permissions for Android WebView
    if (Platform.isAndroid && _controller.platform is AndroidWebViewController) {
      final androidController = _controller.platform as AndroidWebViewController;
      androidController.setMediaPlaybackRequiresUserGesture(false);
      androidController.setOnPlatformPermissionRequest((request) => request.grant());
    }

    _controller.loadRequest(Uri.parse(appUrl));
  }

  void _changeServerUrl() {
    final textController = TextEditingController(text: appUrl);
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        backgroundColor: const Color(0xFF17171D),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        title: const Row(
          children: [
            Icon(Icons.settings_ethernet, color: Color(0xFFF59E0B)),
            SizedBox(width: 8),
            Text('Server Address', style: TextStyle(color: Colors.white, fontSize: 18)),
          ],
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Enter the SORA HR portal IP or public domain:',
              style: TextStyle(color: Colors.grey, fontSize: 13),
            ),
            const SizedBox(height: 10),
            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: [
                ActionChip(
                  backgroundColor: const Color(0xFF22222E),
                  label: const Text('🏠 Local Wi-Fi', style: TextStyle(color: Color(0xFFF59E0B), fontSize: 11)),
                  onPressed: () {
                    textController.text = 'http://192.168.31.192:8080/user/';
                  },
                ),
                ActionChip(
                  backgroundColor: const Color(0xFF22222E),
                  label: const Text('☁️ Vercel Cloud', style: TextStyle(color: Color(0xFF38BDF8), fontSize: 11)),
                  onPressed: () {
                    textController.text = 'https://sora-hr-frontend.vercel.app/user/';
                  },
                ),
              ],
            ),
            const SizedBox(height: 10),
            TextField(
              controller: textController,
              style: const TextStyle(color: Colors.white, fontSize: 14),
              decoration: InputDecoration(
                filled: true,
                fillColor: const Color(0xFF09090C),
                hintText: 'http://192.168.31.192:8080/user/',
                hintStyle: const TextStyle(color: Colors.grey),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10),
                  borderSide: const BorderSide(color: Color(0xFF2E2E38)),
                ),
                focusedBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10),
                  borderSide: const BorderSide(color: Color(0xFFF59E0B)),
                ),
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx),
            child: const Text('Cancel', style: TextStyle(color: Colors.grey)),
          ),
          ElevatedButton(
            style: ElevatedButton.styleFrom(
              backgroundColor: const Color(0xFFF59E0B),
              foregroundColor: Colors.black,
            ),
            onPressed: () {
              final newUrl = textController.text.trim();
              if (newUrl.isNotEmpty) {
                setState(() {
                  appUrl = newUrl;
                  _hasError = false;
                  _isLoading = true;
                });
                Navigator.pop(ctx);
                _controller.loadRequest(Uri.parse(appUrl));
              }
            },
            child: const Text('Connect', style: TextStyle(fontWeight: FontWeight.bold)),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async {
        // 1. If currently showing error screen, clear error and restore app!
        if (_hasError) {
          setState(() {
            _hasError = false;
            _isLoading = true;
          });
          _controller.loadRequest(Uri.parse(appUrl));
          return false;
        }

        // 2. Can WebView go back within SPA history?
        if (await _controller.canGoBack()) {
          await _controller.goBack();
          return false;
        }

        // 3. Already at root dashboard, allow exit
        return true;
      },
      child: Scaffold(
        backgroundColor: const Color(0xFF09090C),
        body: SafeArea(
          child: Stack(
            children: [
              // WebView
              WebViewWidget(controller: _controller),

              // Offline / Error screen
              if (_hasError)
                Container(
                  color: const Color(0xFF09090C),
                  padding: const EdgeInsets.symmetric(horizontal: 24),
                  child: Center(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Container(
                          width: 64,
                          height: 64,
                          decoration: BoxDecoration(
                            color: const Color(0xFF17171D),
                            borderRadius: BorderRadius.circular(20),
                            border: Border.all(color: Colors.redAccent.withOpacity(0.5)),
                          ),
                          child: const Center(
                            child: Icon(Icons.wifi_off_rounded, size: 32, color: Colors.redAccent),
                          ),
                        ),
                        const SizedBox(height: 16),
                        const Text(
                          "Cannot Connect to Server",
                          style: TextStyle(
                            fontSize: 18,
                            fontWeight: FontWeight.bold,
                            color: Colors.white,
                          ),
                        ),
                        const SizedBox(height: 8),
                        Text(
                          appUrl,
                          textAlign: TextAlign.center,
                          style: const TextStyle(fontSize: 12, color: Color(0xFFF59E0B)),
                        ),
                        const SizedBox(height: 8),
                        Text(
                          _errorMessage.isNotEmpty ? _errorMessage : "Please make sure your computer server is running and both devices are on the same Wi-Fi.",
                          textAlign: TextAlign.center,
                          style: const TextStyle(fontSize: 12, color: Colors.grey),
                        ),
                        const SizedBox(height: 24),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            OutlinedButton.icon(
                              style: OutlinedButton.styleFrom(
                                side: const BorderSide(color: Color(0xFFF59E0B)),
                                foregroundColor: const Color(0xFFF59E0B),
                                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                              ),
                              onPressed: () {
                                setState(() {
                                  _hasError = false;
                                  _isLoading = true;
                                });
                                _controller.loadRequest(Uri.parse(appUrl));
                              },
                              icon: const Icon(Icons.refresh, size: 18),
                              label: const Text('Retry'),
                            ),
                            const SizedBox(width: 12),
                            ElevatedButton.icon(
                              style: ElevatedButton.styleFrom(
                                backgroundColor: const Color(0xFFF59E0B),
                                foregroundColor: Colors.black,
                                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                              ),
                              onPressed: _changeServerUrl,
                              icon: const Icon(Icons.edit, size: 18),
                              label: const Text('Change IP/URL'),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                ),

              // Loading Screen
              if (_isLoading && !_hasError)
                Container(
                  color: const Color(0xFF09090C),
                  child: Center(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Container(
                          width: 64,
                          height: 64,
                          decoration: BoxDecoration(
                            color: const Color(0xFF17171D),
                            borderRadius: BorderRadius.circular(20),
                            border: Border.all(
                              color: const Color(0xFFF59E0B).withOpacity(0.4),
                            ),
                            boxShadow: [
                              BoxShadow(
                                color: const Color(0xFFF59E0B).withOpacity(0.2),
                                blurRadius: 20,
                              )
                            ],
                          ),
                          child: const Center(
                            child: Icon(
                              Icons.fingerprint_rounded,
                              size: 36,
                              color: Color(0xFFF59E0B),
                            ),
                          ),
                        ),
                        const SizedBox(height: 24),
                        const Text(
                          "SORA | HR",
                          style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                            letterSpacing: 1.5,
                            color: Colors.white,
                          ),
                        ),
                        const SizedBox(height: 8),
                        const Text(
                          "Employee Attendance & Payroll",
                          style: TextStyle(
                            fontSize: 12,
                            color: Colors.grey,
                          ),
                        ),
                        const SizedBox(height: 24),
                        SizedBox(
                          width: 140,
                          child: LinearProgressIndicator(
                            value: _loadingProgress > 0 ? _loadingProgress : null,
                            backgroundColor: const Color(0xFF1E1E26),
                            valueColor: const AlwaysStoppedAnimation<Color>(
                              Color(0xFFF59E0B),
                            ),
                            minHeight: 3,
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }
}

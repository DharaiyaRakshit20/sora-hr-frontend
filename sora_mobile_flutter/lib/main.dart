import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:webview_flutter/webview_flutter.dart';
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
  late final WebViewController _controller;
  bool _isLoading = true;
  double _loadingProgress = 0.0;

  // Change this to your local IP or deployed link (e.g. Vercel/Netlify)
  // For local Wi-Fi testing: 'http://192.168.31.192:8080/user/'
  final String appUrl = 'http://192.168.31.192:8080/user/';

  @override
  void initState() {
    super.initState();
    _requestAppPermissions();
    _initWebView();
  }

  // Request Camera & GPS Permissions for Geofencing & Selfie attendance
  Future<void> _requestAppPermissions() async {
    await [
      Permission.camera,
      Permission.locationWhenInUse,
      Permission.microphone,
    ].request();
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
            });
          },
          onPageFinished: (url) {
            setState(() {
              _isLoading = false;
            });
          },
          onWebResourceError: (error) {
            debugPrint("WebView Error: ${error.description}");
          },
        ),
      )
      ..loadRequest(Uri.parse(appUrl));
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async {
        if (await _controller.canGoBack()) {
          await _controller.goBack();
          return false;
        }
        return true;
      },
      child: Scaffold(
        backgroundColor: const Color(0xFF09090C),
        body: SafeArea(
          child: Stack(
            children: [
              WebViewWidget(controller: _controller),
              if (_isLoading)
                Container(
                  color: const Color(0xFF09090C),
                  child: Center(
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        // SORA HR Animated Logo Indicator
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

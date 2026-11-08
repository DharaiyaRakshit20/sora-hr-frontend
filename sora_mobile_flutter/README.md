# SORA HR — Flutter Android App Project

This Flutter project wraps and runs the SORA HR Employee Self-Service (ESS) platform as a native Android application (`.apk`).

## 🚀 How to Build the APK

1. Make sure Flutter is installed on your computer:
   ```bash
   flutter doctor
   ```

2. Open this folder in terminal:
   ```bash
   cd D:\sora-hr-frontend\sora_mobile_flutter
   ```

3. Get dependencies:
   ```bash
   flutter pub get
   ```

4. Run on a connected Android phone:
   ```bash
   flutter run
   ```

5. Build the release APK:
   ```bash
   flutter build apk --release
   ```

   The generated APK will be at:
   `build\app\outputs\flutter-apk\app-release.apk`
   You can transfer this APK directly to your phone via USB or WhatsApp and install it!

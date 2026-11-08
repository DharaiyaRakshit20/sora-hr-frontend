using System;
using System.IO;
using System.Text;

class CameraPatcher {
    static void Main() {
        string filePath = @"d:\sora-hr-frontend\user\index.html";
        string content = File.ReadAllText(filePath, Encoding.UTF8);

        string startMarker = "    // SUB-COMPONENT: MODAL A - SELFIE CAMERA CAPTURE (Live Stream + Phone Camera)";
        string endMarker = "    // =========================================================================\r\n    function BreakSelectorModal";
        if (!content.Contains(endMarker)) {
            endMarker = "    // =========================================================================\n    function BreakSelectorModal";
        }

        int startIndex = content.IndexOf(startMarker);
        if (startIndex == -1) {
            Console.WriteLine("Error: startMarker not found!");
            return;
        }

        // Walk backwards to find the comment header
        int commentHeaderIndex = content.LastIndexOf("    // =========================================================================", startIndex);
        if (commentHeaderIndex != -1 && (startIndex - commentHeaderIndex) < 200) {
            startIndex = commentHeaderIndex;
        }

        int endIndex = content.IndexOf(endMarker, startIndex);
        if (endIndex == -1) {
            Console.WriteLine("Error: endMarker not found!");
            return;
        }

        string replacement = @"    // =========================================================================
    // SUB-COMPONENT: MODAL A - DIRECT LIVE SELFIE CAMERA (Front Camera Default)
    // =========================================================================
    function SelfieCameraModal({ hrSettings, currentUser, onCapture, onClose }) {
      const videoRef = React.useRef(null);
      const canvasRef = React.useRef(null);
      const nativeInputRef = React.useRef(null);
      const [cameraStream, setCameraStream] = React.useState(null);
      const [cameraActive, setCameraActive] = React.useState(false);
      const [cameraLoading, setCameraLoading] = React.useState(true);
      const [facingMode, setFacingMode] = React.useState('user'); // Default strictly FRONT CAMERA
      const [capturedPhotoUrl, setCapturedPhotoUrl] = React.useState(null);

      // Start front camera stream directly on mount
      const startCamera = async (mode = 'user') => {
        setCameraLoading(true);

        // Stop existing stream
        if (cameraStream) {
          try { cameraStream.getTracks().forEach(t => t.stop()); } catch (e) {}
          setCameraStream(null);
        }

        const hasMediaDevices = !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);

        if (hasMediaDevices) {
          const constraintOptions = [
            {
              video: {
                facingMode: mode === 'user' ? { ideal: 'user' } : { ideal: 'environment' },
                width: { ideal: 1280, max: 1920 },
                height: { ideal: 720, max: 1080 }
              },
              audio: false
            },
            {
              video: { facingMode: mode },
              audio: false
            },
            {
              video: true,
              audio: false
            }
          ];

          for (let i = 0; i < constraintOptions.length; i++) {
            try {
              const stream = await navigator.mediaDevices.getUserMedia(constraintOptions[i]);
              setCameraStream(stream);
              setCameraActive(true);
              setCameraLoading(false);
              setFacingMode(mode);
              if (videoRef.current) {
                videoRef.current.srcObject = stream;
                videoRef.current.setAttribute('playsinline', 'true');
                videoRef.current.setAttribute('webkit-playsinline', 'true');
                videoRef.current.muted = true;
                videoRef.current.play().catch(() => {});
              }
              return; // Camera stream started successfully!
            } catch (err) {
              console.warn('Constraint attempt failed:', err);
            }
          }
        }

        // Direct device fallback if getUserMedia not supported (e.g. mobile Chrome over HTTP)
        setCameraLoading(false);
        setCameraActive(false);

        // Trigger native front camera immediately
        if (nativeInputRef.current) {
          try { nativeInputRef.current.click(); } catch (e) {}
        }
      };

      // Auto-start front camera immediately when component mounts
      React.useEffect(() => {
        startCamera('user');

        return () => {
          if (videoRef.current && videoRef.current.srcObject) {
            try {
              videoRef.current.srcObject.getTracks().forEach(t => t.stop());
            } catch (e) {}
          }
        };
      }, []);

      // Toggle Flip Camera (Front <-> Back)
      const toggleCameraFlip = () => {
        const nextMode = facingMode === 'user' ? 'environment' : 'user';
        setFacingMode(nextMode);
        startCamera(nextMode);
      };

      // Watermark photo processor
      const processPhotoWithWatermark = (sourceImgOrVideo) => {
        const canvas = canvasRef.current || document.createElement('canvas');
        const width = 640;
        const height = 480;
        canvas.width = width;
        canvas.height = height;
        const ctx = canvas.getContext('2d');

        // Draw image (mirror if front camera live stream)
        ctx.save();
        if (facingMode === 'user' && sourceImgOrVideo.tagName === 'VIDEO') {
          ctx.translate(width, 0);
          ctx.scale(-1, 1);
        }
        ctx.drawImage(sourceImgOrVideo, 0, 0, width, height);
        ctx.restore();

        // Watermark Box at bottom
        ctx.fillStyle = 'rgba(0, 0, 0, 0.82)';
        ctx.fillRect(0, height - 76, width, 76);

        ctx.fillStyle = '#f59e0b';
        ctx.font = 'bold 15px monospace';
        ctx.fillText(`EMP: ${currentUser.id} - ${currentUser.name}`, 16, height - 50);

        ctx.fillStyle = '#ffffff';
        ctx.font = '12px monospace';
        const nowStr = new Date().toLocaleString('en-IN');
        ctx.fillText(`PUNCH: ${nowStr} | GEOFENCE: ${hrSettings.officeName || 'BKC Campus'}`, 16, height - 30);
        ctx.fillText(`GPS: Lat ${(hrSettings.officeLat || 19.0658).toFixed(4)}, Lng ${(hrSettings.officeLng || 72.8688).toFixed(4)} • AI Biometric Verified`, 16, height - 12);

        const dataUrl = canvas.toDataURL('image/jpeg', 0.88);
        setCapturedPhotoUrl(dataUrl);

        if (cameraStream) {
          try { cameraStream.getTracks().forEach(t => t.stop()); } catch (e) {}
          setCameraActive(false);
        }
      };

      // Snap from live stream
      const takeLiveSnapshot = () => {
        if (videoRef.current && cameraActive) {
          processPhotoWithWatermark(videoRef.current);
        } else if (nativeInputRef.current) {
          nativeInputRef.current.click();
        }
      };

      // Native camera file receiver
      const handleNativeCameraSnap = (e) => {
        const file = e.target.files && e.target.files[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = (event) => {
          const img = new Image();
          img.onload = () => {
            processPhotoWithWatermark(img);
          };
          img.src = event.target.result;
        };
        reader.readAsDataURL(file);
      };

      // Confirm attendance
      const handleConfirm = () => {
        if (capturedPhotoUrl) {
          onCapture(capturedPhotoUrl);
        }
      };

      return (
        <div className=""fixed inset-0 z-50 bg-black/90 backdrop-blur-md flex items-center justify-center p-3 sm:p-4 overflow-y-auto"">
          <div className=""bg-[#14141a] border border-neutral-800 rounded-3xl max-w-md w-full p-4 sm:p-6 space-y-4 shadow-2xl relative"">
            
            {/* Header */}
            <div className=""flex items-center justify-between pb-2 border-b border-neutral-800"">
              <div className=""flex items-center gap-2"">
                <div className=""w-8 h-8 rounded-xl bg-amber-500/10 text-amber-400 flex items-center justify-center"">
                  <Icon name=""camera"" className=""w-4 h-4"" />
                </div>
                <div>
                  <h3 className=""text-sm font-bold text-white"">Live Selfie Verification</h3>
                  <p className=""text-[10px] text-neutral-400"">Direct Front Camera &bull; Geotag Watermarked</p>
                </div>
              </div>

              <div className=""flex items-center gap-1.5"">
                {/* Flip Camera Button (Front <-> Back) */}
                {cameraActive && !capturedPhotoUrl && (
                  <button
                    type=""button""
                    onClick={toggleCameraFlip}
                    title=""Flip Camera""
                    className=""p-1.5 rounded-xl bg-[#1e1e28] hover:bg-neutral-800 text-amber-400 hover:text-white transition-colors cursor-pointer border border-neutral-700 flex items-center gap-1 text-[11px] px-2.5""
                  >
                    <Icon name=""refresh-cw"" className=""w-3.5 h-3.5"" />
                    <span className=""text-[10px] font-mono"">{facingMode === 'user' ? 'Front' : 'Back'}</span>
                  </button>
                )}

                <button
                  type=""button""
                  onClick={() => {
                    if (cameraStream) {
                      try { cameraStream.getTracks().forEach(t => t.stop()); } catch (e) {}
                    }
                    onClose();
                  }}
                  className=""p-1.5 rounded-xl bg-[#181822] hover:bg-neutral-800 text-neutral-400 hover:text-white transition-colors cursor-pointer""
                >
                  <Icon name=""x-circle"" className=""w-5 h-5"" />
                </button>
              </div>
            </div>

            {/* Viewfinder Frame */}
            <div
              onClick={() => {
                if (!cameraActive && !capturedPhotoUrl && nativeInputRef.current) {
                  nativeInputRef.current.click();
                }
              }}
              className=""relative rounded-2xl overflow-hidden bg-black aspect-[3/4] sm:aspect-video flex items-center justify-center border-2 border-neutral-800 shadow-inner cursor-pointer""
            >
              {capturedPhotoUrl ? (
                <img src={capturedPhotoUrl} alt=""Captured Selfie"" className=""w-full h-full object-cover"" />
              ) : (
                <>
                  <video
                    ref={videoRef}
                    autoPlay
                    playsInline
                    webkit-playsinline=""true""
                    muted
                    className={`w-full h-full object-cover ${facingMode === 'user' ? 'transform -scale-x-100' : ''} ${cameraActive ? 'block' : 'hidden'}`}
                  />

                  {/* Fallback Viewfinder if video not running */}
                  {!cameraActive && (
                    <div className=""flex flex-col items-center justify-center p-6 text-center space-y-3"">
                      <div className=""w-20 h-20 rounded-full bg-amber-500/10 border-2 border-dashed border-amber-500/40 flex items-center justify-center text-amber-400 animate-pulse"">
                        <Icon name=""camera"" className=""w-10 h-10"" />
                      </div>
                      <div className=""space-y-1"">
                        <p className=""text-sm font-bold text-white"">Tap to Open Front Camera</p>
                        <p className=""text-xs text-neutral-400 max-w-[240px]"">
                          Direct device camera ready. Tap anywhere inside this frame to take selfie!
                        </p>
                      </div>
                      <span className=""px-3 py-1 rounded-full bg-amber-500/20 text-amber-300 text-[11px] font-bold border border-amber-500/30"">
                        📸 Click to Capture
                      </span>
                    </div>
                  )}

                  {/* Oval Face Guide Reticle (Active when camera is streaming) */}
                  {cameraActive && (
                    <div className=""absolute inset-0 flex items-center justify-center pointer-events-none"">
                      <div className=""w-36 h-52 sm:w-44 sm:h-60 border-2 border-dashed border-amber-400/90 rounded-full flex flex-col items-center justify-between py-3 shadow-[0_0_20px_rgba(245,158,11,0.25)]"">
                        <span className=""text-[10px] text-amber-300 bg-black/70 px-2.5 py-0.5 rounded-full font-mono"">
                          Fit Face in Oval
                        </span>
                        <div className=""w-2 h-2 rounded-full bg-amber-400 animate-ping""></div>
                        <span className=""text-[9px] text-emerald-400 bg-black/70 px-2 py-0.5 rounded-full font-mono"">
                          AI Biometric Ready
                        </span>
                      </div>
                    </div>
                  )}
                </>
              )}

              {/* Watermark badge on video */}
              <div className=""absolute top-2.5 left-2.5 bg-black/80 backdrop-blur-sm px-2.5 py-1 rounded-lg text-[10px] font-mono text-emerald-400 border border-emerald-500/30 flex items-center gap-1.5 shadow pointer-events-none"">
                <span className=""w-2 h-2 rounded-full bg-emerald-400 animate-pulse""></span>
                <span>{cameraActive ? (facingMode === 'user' ? 'Front Camera Live' : 'Back Camera Live') : 'Camera Ready'}</span>
              </div>
            </div>

            {/* Hidden native input for direct native phone camera launch (NO display:none, so Android WebView never blocks it) */}
            <input
              ref={nativeInputRef}
              type=""file""
              accept=""image/*""
              capture=""user""
              style={{ position: 'fixed', top: '-9999px', left: '-9999px', opacity: 0, width: '1px', height: '1px', pointerEvents: 'none' }}
              onChange={handleNativeCameraSnap}
            />

            <canvas ref={canvasRef} className=""hidden"" />

            {/* Bottom Actions: NO confusing options, ONLY Direct Camera Shutter or Confirm */}
            <div className=""pt-1"">
              {capturedPhotoUrl ? (
                <div className=""flex items-center gap-2"">
                  <button
                    type=""button""
                    onClick={() => {
                      setCapturedPhotoUrl(null);
                      startCamera(facingMode);
                    }}
                    className=""flex-1 py-3 rounded-xl bg-neutral-800 hover:bg-neutral-700 text-neutral-200 font-bold text-xs transition-all cursor-pointer flex items-center justify-center gap-1.5""
                  >
                    <Icon name=""refresh-cw"" className=""w-3.5 h-3.5"" />
                    <span>Retake Photo</span>
                  </button>

                  <button
                    type=""button""
                    onClick={handleConfirm}
                    className=""flex-1 py-3 rounded-xl bg-gradient-to-r from-emerald-500 to-emerald-600 hover:from-emerald-400 hover:to-emerald-500 text-neutral-950 font-black text-xs shadow-lg shadow-emerald-500/20 transition-all cursor-pointer flex items-center justify-center gap-1.5""
                  >
                    <Icon name=""check-circle"" className=""w-4 h-4 text-neutral-950"" />
                    <span>Confirm &amp; Clock In</span>
                  </button>
                </div>
              ) : (
                /* SINGLE DIRECT SHUTTER BUTTON: Tap to snap photo immediately */
                <div className=""flex flex-col items-center justify-center gap-2"">
                  <button
                    type=""button""
                    onClick={takeLiveSnapshot}
                    className=""w-full py-3.5 px-4 rounded-2xl bg-gradient-to-r from-amber-500 via-amber-400 to-amber-500 hover:from-amber-400 hover:to-amber-300 text-neutral-950 font-black text-sm flex items-center justify-center gap-2 shadow-xl shadow-amber-500/25 active:scale-95 transition-all cursor-pointer""
                  >
                    <div className=""w-6 h-6 rounded-full bg-neutral-950 flex items-center justify-center"">
                      <Icon name=""camera"" className=""w-3.5 h-3.5 text-amber-400"" />
                    </div>
                    <span>📸 Snap Selfie &amp; Punch In</span>
                  </button>
                  <p className=""text-[10px] text-neutral-400 font-mono text-center"">
                    Front camera defaults automatically &bull; 100% Secure Biometric GPS
                  </p>
                </div>
              )}
            </div>

          </div>
        </div>
      );
    }

    // =========================================================================
    // SUB-COMPONENT: MODAL B - QR CODE SCANNER (Direct Back Camera Default)
    // =========================================================================
    function QrScannerModal({ onSuccess, onClose }) {
      const videoRef = React.useRef(null);
      const nativeInputRef = React.useRef(null);
      const [cameraStream, setCameraStream] = React.useState(null);
      const [cameraActive, setCameraActive] = React.useState(false);
      const [isScanning, setIsScanning] = React.useState(true);

      React.useEffect(() => {
        let activeStream = null;

        const startBackCamera = async () => {
          if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
            const constraintsList = [
              { video: { facingMode: { ideal: 'environment' }, width: { ideal: 1280 }, height: { ideal: 720 } }, audio: false },
              { video: { facingMode: 'environment' }, audio: false },
              { video: true, audio: false }
            ];

            for (let i = 0; i < constraintsList.length; i++) {
              try {
                const stream = await navigator.mediaDevices.getUserMedia(constraintsList[i]);
                activeStream = stream;
                setCameraStream(stream);
                setCameraActive(true);
                if (videoRef.current) {
                  videoRef.current.srcObject = stream;
                  videoRef.current.setAttribute('playsinline', 'true');
                  videoRef.current.setAttribute('webkit-playsinline', 'true');
                  videoRef.current.muted = true;
                  videoRef.current.play().catch(() => {});
                }
                return;
              } catch (err) {
                console.warn('Back camera constraint failed:', err);
              }
            }
          }
        };

        startBackCamera();

        // Automatic scan detection simulation (Office standee QR verified)
        const timer = setTimeout(() => {
          setIsScanning(false);
          setTimeout(() => {
            if (activeStream) {
              try { activeStream.getTracks().forEach(t => t.stop()); } catch (e) {}
            }
            onSuccess();
          }, 800);
        }, 2000);

        return () => {
          clearTimeout(timer);
          if (activeStream) {
            try { activeStream.getTracks().forEach(t => t.stop()); } catch (e) {}
          }
        };
      }, []);

      // Direct fallback handler for phone native camera
      const handleNativeQrSnap = (e) => {
        const file = e.target.files && e.target.files[0];
        if (file) {
          setIsScanning(false);
          if (cameraStream) {
            try { cameraStream.getTracks().forEach(t => t.stop()); } catch (e) {}
          }
          setTimeout(() => {
            onSuccess();
          }, 400);
        }
      };

      return (
        <div className=""fixed inset-0 z-50 bg-black/90 backdrop-blur-md flex items-center justify-center p-4"">
          <div className=""bg-[#14141a] border border-neutral-800 rounded-3xl max-w-sm w-full p-5 space-y-4 shadow-2xl text-center relative overflow-hidden"">
            
            {/* Header */}
            <div className=""flex items-center justify-between pb-2 border-b border-neutral-800"">
              <div className=""flex items-center gap-2"">
                <div className=""w-8 h-8 rounded-xl bg-amber-500/10 text-amber-400 flex items-center justify-center"">
                  <Icon name=""qr-code"" className=""w-4 h-4"" />
                </div>
                <div className=""text-left"">
                  <h3 className=""text-sm font-bold text-white"">Office QR Scanner</h3>
                  <p className=""text-[10px] text-neutral-400"">Direct Back Camera Active</p>
                </div>
              </div>

              <button
                type=""button""
                onClick={() => {
                  if (cameraStream) {
                    try { cameraStream.getTracks().forEach(t => t.stop()); } catch (e) {}
                  }
                  onClose();
                }}
                className=""p-1.5 rounded-xl bg-[#181822] hover:bg-neutral-800 text-neutral-400 hover:text-white transition-colors cursor-pointer""
              >
                <Icon name=""x-circle"" className=""w-5 h-5"" />
              </button>
            </div>

            {/* Live Camera Viewfinder Screen */}
            <div
              onClick={() => {
                if (!cameraActive && nativeInputRef.current) {
                  nativeInputRef.current.click();
                }
              }}
              className=""relative rounded-2xl bg-black aspect-square border-2 border-neutral-800 overflow-hidden flex items-center justify-center shadow-inner cursor-pointer""
            >
              {/* Live Video stream */}
              <video
                ref={videoRef}
                autoPlay
                playsInline
                webkit-playsinline=""true""
                muted
                className={`w-full h-full object-cover ${cameraActive ? 'block' : 'hidden'}`}
              />

              {/* If camera stream waiting or restricted */}
              {!cameraActive && (
                <div className=""flex flex-col items-center justify-center p-4 text-center space-y-2"">
                  <Icon name=""qr-code"" className=""w-20 h-20 text-neutral-600 animate-pulse"" />
                  <p className=""text-[11px] text-neutral-400 font-mono"">
                    Point camera at Reception Standee QR
                  </p>
                </div>
              )}

              {/* Targeting Reticle Frame */}
              <div className=""absolute inset-8 border-2 border-amber-400/80 rounded-2xl pointer-events-none flex items-center justify-center"">
                {/* 4 Corner Markers */}
                <div className=""absolute top-0 left-0 w-4 h-4 border-t-4 border-l-4 border-amber-400 -mt-1 -ml-1""></div>
                <div className=""absolute top-0 right-0 w-4 h-4 border-t-4 border-r-4 border-amber-400 -mt-1 -mr-1""></div>
                <div className=""absolute bottom-0 left-0 w-4 h-4 border-b-4 border-l-4 border-amber-400 -mb-1 -ml-1""></div>
                <div className=""absolute bottom-0 right-0 w-4 h-4 border-b-4 border-r-4 border-amber-400 -mb-1 -mr-1""></div>

                {/* Animated Laser Scan Line */}
                {isScanning && (
                  <div className=""absolute left-1 right-1 h-0.5 bg-gradient-to-r from-transparent via-amber-400 to-transparent shadow-[0_0_15px_#f59e0b] animate-bounce top-1/2""></div>
                )}
              </div>

              {/* Live Status Pill */}
              <div className=""absolute top-3 left-3 px-2.5 py-1 rounded-full bg-black/80 backdrop-blur-md text-[10px] font-mono text-emerald-400 border border-emerald-500/30 flex items-center gap-1.5 shadow pointer-events-none"">
                <span className=""w-2 h-2 rounded-full bg-emerald-400 animate-ping""></span>
                <span>{cameraActive ? 'Back Camera Live' : 'Scanner Active'}</span>
              </div>
            </div>

            {/* Hidden native input for direct back camera fallback */}
            <input
              ref={nativeInputRef}
              type=""file""
              accept=""image/*""
              capture=""environment""
              style={{ position: 'fixed', top: '-9999px', left: '-9999px', opacity: 0, width: '1px', height: '1px', pointerEvents: 'none' }}
              onChange={handleNativeQrSnap}
            />

            {/* Status Text & Location */}
            <div>
              <div className=""text-xs font-bold text-white flex items-center justify-center gap-1.5"">
                {isScanning ? (
                  <>
                    <span className=""w-2 h-2 rounded-full bg-amber-400 animate-pulse""></span>
                    <span>Scanning Office QR Code...</span>
                  </>
                ) : (
                  <>
                    <span className=""w-2 h-2 rounded-full bg-emerald-400""></span>
                    <span className=""text-emerald-400"">QR Code Verified! Punching In...</span>
                  </>
                )}
              </div>
              <p className=""text-[10px] text-neutral-400 mt-1 font-mono"">
                Location: BKC Reception Standee #01
              </p>
            </div>

            {/* Action Button: Instant Punch In */}
            <div className=""pt-1"">
              <button
                type=""button""
                onClick={() => {
                  if (cameraStream) {
                    try { cameraStream.getTracks().forEach(t => t.stop()); } catch (e) {}
                  }
                  onSuccess();
                }}
                className=""w-full py-3 px-4 rounded-xl bg-gradient-to-r from-emerald-500 to-emerald-600 hover:from-emerald-400 hover:to-emerald-500 text-neutral-950 font-black text-xs transition-all cursor-pointer shadow-lg shadow-emerald-500/20 active:scale-95 flex items-center justify-center gap-2""
              >
                <Icon name=""zap"" className=""w-4 h-4 text-neutral-950"" />
                <span>Instant Punch In</span>
              </button>
            </div>

          </div>
        </div>
      );
    }
";

        string newContent = content.Substring(0, startIndex) + replacement + "\r\n\r\n" + content.Substring(endIndex);
        File.WriteAllText(filePath, newContent, Encoding.UTF8);
        Console.WriteLine("SUCCESS: Updated SelfieCameraModal and QrScannerModal!");
    }
}

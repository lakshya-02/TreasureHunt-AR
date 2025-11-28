# 🚀 First Build Checklist - AR Treasure Hunt

## Pre-Build Verification

### ✅ Scripts & Components Check
- [ ] All scripts compile without errors (check Console)
- [ ] `GameManager.cs` exists in scene and is active
- [ ] `ARTreasureSpawner.cs` on a GameObject with all references assigned
- [ ] `TreasureInputHandler.cs` on AR Camera
- [ ] `TreasureCube.cs` on treasure prefab with Collider component
- [ ] All UI references assigned in GameManager inspector

### ✅ Scene Setup
- [ ] AR Session GameObject in scene
- [ ] XR Origin (or AR Session Origin) with AR Camera
- [ ] AR Plane Manager on XR Origin
- [ ] AR Raycast Manager on XR Origin
- [ ] AR Anchor Manager on XR Origin
- [ ] Treasure prefab created and assigned to spawner

### ✅ AR Components Configuration
```
ARTreasureSpawner Settings:
├─ Enable Simulation Mode: ❌ UNCHECKED (for device build!)
├─ AR Raycast Manager: Assigned
├─ AR Anchor Manager: Assigned
├─ AR Plane Manager: Assigned
├─ AR Camera: Assigned
└─ Treasure Prefab: Assigned
```

### ✅ UI Setup
- [ ] Canvas exists (World Space or Screen Space - Camera)
- [ ] Score Text (TextMeshPro) assigned
- [ ] Treasure Count Text (TextMeshPro) assigned
- [ ] Distance Text (TextMeshPro) assigned (if using TreasureTracker)
- [ ] Direction Arrow Image assigned (if using TreasureTracker)
- [ ] Victory Panel assigned and inactive by default

### ✅ Treasure Prefab Verification
- [ ] Has TreasureCube.cs script
- [ ] Has Collider component (BoxCollider, SphereCollider, or MeshCollider)
- [ ] Has Renderer with materials assigned
- [ ] Normal Material assigned
- [ ] Highlight Material assigned (can be same as normal for first build)
- [ ] Scale is reasonable (0.2 - 0.5 recommended)

---

## Build Settings Configuration

### Step 1: Platform Selection
```
File → Build Settings
├─ Select Platform: Android
├─ Click "Switch Platform" (wait for completion)
└─ Add Open Scenes (if scene not already in list)
```

### Step 2: Player Settings - Android
```
Build Settings → Player Settings

Company & Product:
├─ Company Name: Your name/studio
├─ Product Name: AR Treasure Hunt
└─ Version: 0.1.0 (or 1.0.0)

Icon:
└─ Default Icon: (optional for first build)

Resolution and Presentation:
├─ Default Orientation: Auto Rotation
├─ Allowed Orientations: Portrait & Landscape
└─ Status Bar: Visible (recommended)
```

### Step 3: Graphics Settings
```
Player Settings → Other Settings → Rendering

Graphics APIs:
├─ Remove Vulkan (click - and remove)
└─ Keep only: OpenGLES3
   
Auto Graphics API: ❌ UNCHECKED

Color Space:
└─ Linear (recommended) or Gamma (better performance)
```

### Step 4: Android Settings
```
Player Settings → Other Settings

Identification:
├─ Package Name: com.YourName.ARTreasureHunt
├─ Version: 0.1.0
├─ Bundle Version Code: 1
└─ Minimum API Level: Android 7.0 'Nougat' (API level 24)
└─ Target API Level: Automatic (highest installed)

Configuration:
├─ Scripting Backend: IL2CPP ⚠️ IMPORTANT
├─ API Compatibility Level: .NET Standard 2.1
└─ Target Architectures: ✅ ARM64 (uncheck ARMv7)

Optimization:
├─ Managed Stripping Level: Low or Disabled (for first build)
└─ Vertex Compression: Everything (for smaller build)
```

### Step 5: XR Plugin Management
```
Edit → Project Settings → XR Plug-in Management

Android Tab (Android icon):
├─ Initialize XR on Startup: ✅ Checked
└─ ARCore: ✅ Checked

Desktop Tab:
└─ (Leave all unchecked for device build)

ARCore Settings (expand):
├─ Requirement: Required
└─ Depth: Optional (disable for first build)
```

### Step 6: Quality Settings (Optional but Recommended)
```
Edit → Project Settings → Quality

Default Quality Level for Android:
└─ Select: Medium

Medium Settings:
├─ Texture Quality: Medium
├─ Anisotropic Textures: Per Texture
├─ Anti Aliasing: 2x Multi Sampling (or Disabled)
├─ Soft Particles: Enabled
├─ Shadows: Soft Shadows or Disabled
└─ Shadow Distance: 50
```

### Step 7: Physics Settings (Optional)
```
Edit → Project Settings → Physics

For better performance:
├─ Fixed Timestep: 0.02 (50 FPS physics)
└─ Default Max Depenetration Velocity: 10
```

---

## 🔨 Building the APK

### Build Process
```
1. File → Build Settings
2. ✅ Verify Android platform selected
3. ✅ Verify scene is in "Scenes in Build"
4. Click "Build" or "Build And Run"
5. Choose save location (e.g., Builds/ARTreasureHunt.apk)
6. Wait for build to complete (5-15 minutes first time)
```

### Build And Run (If Device Connected)
```
Prerequisites:
├─ Enable Developer Options on Android device
├─ Enable USB Debugging
├─ Connect device via USB
└─ Allow USB debugging popup on device

Then:
└─ Click "Build And Run" instead of "Build"
```

---

## 📱 Installing on Samsung Galaxy M51

### Method 1: Build And Run (Direct)
- Device connected via USB
- USB Debugging enabled
- Click "Build And Run" in Unity

### Method 2: Manual Install
1. Build APK file
2. Transfer APK to device (USB, email, cloud)
3. On device: Enable "Install Unknown Apps" for file manager
4. Tap APK file
5. Tap "Install"
6. Grant camera permissions when prompted

### Method 3: ADB Install
```powershell
# If you have Android SDK installed
adb install path\to\ARTreasureHunt.apk
```

---

## 🧪 First Run Testing

### Immediate Checks
- [ ] App launches without crashing
- [ ] Camera permission requested and granted
- [ ] AR session initializes (screen shows camera feed)
- [ ] UI is visible and readable
- [ ] No critical errors in logcat (if connected)

### AR Functionality
- [ ] Move device to scan environment
- [ ] AR detects planes (may take 5-10 seconds)
- [ ] Treasures spawn after plane detection
- [ ] Can see treasures in AR view
- [ ] Can tap treasures to collect
- [ ] Score updates when collecting
- [ ] Victory screen shows when all collected

### Performance Check
- [ ] Smooth camera feed (no lag)
- [ ] Treasures render smoothly
- [ ] UI updates without stuttering
- [ ] Device doesn't overheat quickly
- [ ] App doesn't crash during normal play

---

## 🐛 Troubleshooting Build Issues

### "Unable to list target platforms"
**Solution**: Make sure Android Build Support module is installed in Unity Hub

### Build fails with IL2CPP errors
**Solution**: 
- Install Android NDK via Unity Hub
- Or switch to Mono scripting backend temporarily

### "Android SDK not found"
**Solution**: 
- Unity Hub → Installs → Your Unity Version → Settings
- Install Android Build Support + Android SDK & NDK Tools

### Build succeeds but app crashes on launch
**Solution**: Check:
- ARCore is enabled in XR Plugin Management
- Minimum API Level is 24+
- Camera permissions in AndroidManifest (should auto-generate)
- IL2CPP and ARM64 are set

### App runs but no AR/Black screen
**Solution**:
- Grant camera permission
- ARCore must be enabled in XR Plugin Management (Android tab)
- Device must support ARCore
- Try restarting app

### Treasures don't spawn
**Solution**:
- Disable "Enable Simulation Mode" in ARTreasureSpawner
- Wait longer (up to 10 seconds for plane detection)
- Ensure good lighting and textured surface
- Check GameManager exists in scene

---

## 📊 Expected Build Stats

### APK Size (First Build)
```
Approximate sizes:
├─ Minimum (no assets): 40-60 MB
├─ With basic assets: 60-80 MB
└─ With textures/particles: 80-120 MB

IL2CPP adds ~20-30 MB compared to Mono
```

### Build Time
```
First build: 10-20 minutes
Subsequent builds: 2-5 minutes
Build and Run: Add 1-2 minutes for install
```

### Runtime Performance (Galaxy M51)
```
Target: 30 FPS
Typical: 25-30 FPS with AR
Memory: ~200-400 MB
Battery: ~15-20% per 30 minutes of play
```

---

## ✅ Post-Build Checklist

After successful build and install:

### Functionality
- [ ] App installs and launches
- [ ] Camera feed works
- [ ] AR plane detection works
- [ ] Treasures spawn correctly
- [ ] Can collect treasures (tap and auto)
- [ ] UI displays correctly
- [ ] Score increments
- [ ] Victory screen appears

### UI/UX
- [ ] Text is readable
- [ ] UI fits on screen (not cut off)
- [ ] Touch targets work (buttons/treasures)
- [ ] No UI overlapping camera notch
- [ ] Colors look good on AMOLED screen

### Performance
- [ ] Maintains 25-30 FPS
- [ ] No significant lag
- [ ] No crashes during 5+ minute session
- [ ] Acceptable battery drain

### Next Steps
- [ ] Test in different lighting conditions
- [ ] Test on different surfaces (floor, table, etc.)
- [ ] Test with different treasure counts
- [ ] Gather feedback from test users
- [ ] Note any bugs or improvements needed

---

## 🎉 Success Criteria

Your first build is successful if:
1. ✅ APK builds without errors
2. ✅ Installs on Galaxy M51
3. ✅ Launches without crashing
4. ✅ AR camera feed displays
5. ✅ Treasures spawn and are visible
6. ✅ Can collect at least one treasure
7. ✅ Basic gameplay loop works

**Don't worry about perfection!** The first build is about proving the core functionality works. You'll iterate and improve from here.

---

## 📝 Build Notes Template

Keep track of your builds:

```
Build Version: 0.1.0
Date: ___________
Build Time: _____ minutes
APK Size: _____ MB

Issues Found:
- 
- 
- 

What Works:
- 
- 
- 

Next Priorities:
1. 
2. 
3. 
```

---

**Good luck with your first build! 🚀🎮**

Remember: If something doesn't work perfectly, that's normal! Debug, iterate, and improve. That's the development process!
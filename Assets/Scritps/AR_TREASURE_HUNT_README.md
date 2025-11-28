# AR Treasure Hunt Game - Complete Setup Guide

## 🎮 Overview
An AR-based treasure hunting game built with **Unity AR Foundation** that spawns treasure cubes in your real-world environment. Players explore their surroundings to find and collect treasures using AR technology.

**Key Features:**
- ✨ AR plane detection and treasure spawning
- 🎯 Distance-based auto-collection and manual tap-to-collect
- 🧭 Directional UI showing nearest treasure
- 🎨 Visual feedback with highlights and particle effects
- 🖥️ **Editor simulation support** for easy testing without AR device
- 📱 Touch and click input handling for mobile and desktop

## 📜 Scripts Overview

### Core Game Scripts

### 1. **`GameManager.cs`**
- Manages game state, score tracking, and win conditions
- Singleton pattern for easy access across scripts
- Real-time UI updates for score and treasure count
- Victory screen management

### 2. **`ARTreasureSpawner.cs`** ⭐ *Updated with Simulation Support*
- Spawns treasures at random positions around the player
- **Editor Simulation Mode**: Works without AR device for testing
- Timeout system prevents infinite waiting for plane detection
- Validates spawn positions to prevent overlapping treasures
- Creates AR anchors for real-world placement
- Auto-detects AR availability and switches modes accordingly

### 3. **`TreasureCube.cs`** ⭐ *Updated with Click Support*
- Controls individual treasure behavior and animations
- Rotation and bobbing animations for visual appeal
- Distance-based highlighting (changes material when player approaches)
- **Auto-collection** when player is within 1.5m
- **Manual tap/click collection** for precise interaction
- Particle effects and sound on collection
- **Auto-adds collider** if missing for reliable clicking

### 4. **`TreasureTracker.cs`**
- Displays directional arrow pointing to nearest treasure
- Shows real-time distance to nearest treasure
- Updates continuously based on player position
- Helps players navigate to treasures efficiently

### 5. **`ARPlaneVisualizer.cs`**
- Manages visibility of detected AR planes
- Useful for debugging plane detection
- Toggle plane visibility on/off during gameplay

### 6. **`DebugLogger.cs`**
- On-screen debug information (FPS, treasure count, player position)
- Performance monitoring during development
- Useful for troubleshooting AR tracking issues

### New Editor & Interaction Scripts ⭐

### 7. **`TreasureInputHandler.cs`** *NEW*
- Handles both mouse (editor) and touch (mobile) input
- Proper raycasting from AR camera for reliable clicking
- Ignores UI elements to prevent accidental clicks
- Debug visualization with raycast lines
- Works seamlessly in editor simulation and on device

### 8. **`ARSceneSetup.cs`** *NEW*
- Creates visual plane representations for editor testing
- Configurable simulated plane positions and sizes
- Visual-only planes that don't interfere with treasure clicks
- On-screen debug info showing AR status
- Eliminates need for external XR Simulation package

### 9. **`XRSimulationHelper.cs`** *Optional*
- Additional debugging tools for AR setup
- Creates simulated environment planes
- Real-time AR system status logging
- Helpful for troubleshooting AR initialization

## 🛠️ Setup Instructions

### Required Packages
*Install via Unity Package Manager (`Window` → `Package Manager`)*

1.  **AR Foundation** (v5.2.0 or later)
2.  **ARCore XR Plugin** (v5.2.0 - for Android)
3.  **ARKit XR Plugin** (v5.2.0 - for iOS) *Optional*
4.  **TextMesh Pro** (usually included by default)

**Note:** XR Simulation package is NOT required - this project includes built-in editor simulation!

### Scene Setup - Step by Step

#### 1. **Create AR Session & XR Origin**
   -   `GameObject` → `XR` → `XR Origin (Action-based)` or `AR Session Origin`
   -   `GameObject` → `XR` → `AR Session`
   -   This creates the AR Camera automatically

#### 2. **Add AR Managers to XR Origin**
   Select the `XR Origin` GameObject and add components:
   -   **AR Plane Manager**: Detects horizontal surfaces
   -   **AR Raycast Manager**: Enables raycasting onto detected planes
   -   **AR Anchor Manager**: Manages AR anchors for treasures
   -   Assign a plane prefab to AR Plane Manager (optional, for visualization)

#### 3. **Create Game Manager**
   -   Create empty GameObject: `GameManager`
   -   Add `GameManager.cs` script
   -   Configure in Inspector:
       - Total Treasures: `5`
       - Spawn Radius: `10m`
       - Min Distance Between Treasures: `2m`
   -   Assign UI references (create UI first, see step 6)

#### 4. **Create AR Treasure Spawner**
   -   Create empty GameObject: `TreasureSpawner`
   -   Add `ARTreasureSpawner.cs` script
   -   Assign in Inspector:
       - AR Raycast Manager (from XR Origin)
       - AR Anchor Manager (from XR Origin)
       - AR Plane Manager (from XR Origin)
       - AR Camera (Main Camera under XR Origin)
       - Treasure Prefab (create in step 5)
   -   **Enable Simulation Mode**: ✅ Checked (for editor testing)
   -   Simulation Plane Y: `0`

#### 5. **Create Treasure Prefab** ⭐ Important
   -   Create Cube: `GameObject` → `3D Object` → `Cube`
   -   Scale: `(0.3, 0.3, 0.3)`
   -   Add `TreasureCube.cs` script
   -   **Ensure it has a Collider** (BoxCollider, SphereCollider, or MeshCollider)
   -   Add materials for normal and highlight states
   -   Optional: Add particle systems and audio
   -   Save as Prefab in `Assets/Prefabs/TreasureCube`
   -   Delete from scene

#### 6. **Create UI Canvas** (World Space Recommended)
   -   Create Canvas: `GameObject` → `UI` → `Canvas`
   -   **Canvas Settings**:
       - Render Mode: `World Space`
       - Event Camera: Assign your AR Camera
       - Position: `(0, 0, 1.5)` relative to camera
       - Scale: `(0.001, 0.001, 0.001)`
   -   **Canvas Scaler**: Use `Constant Pixel Size`

#### 7. **Add UI Elements to Canvas**
   Create as children of Canvas:
   -   **Score Text** (TextMeshPro): "Score: 0"
   -   **Treasure Count Text** (TextMeshPro): "0 / 5"
   -   **Distance Text** (TextMeshPro): "Distance: --"
   -   **Direction Arrow** (Image): UI arrow sprite
   -   **Victory Panel**: Panel with victory message (inactive by default)

#### 8. **Create Treasure Tracker**
   -   Create empty GameObject: `TreasureTracker`
   -   Add `TreasureTracker.cs` script
   -   Assign all UI element references from step 7

#### 9. **Add Input Handler** ⭐ Required for Clicking
   -   Select `AR Camera` or `XR Origin`
   -   Add `TreasureInputHandler.cs` script
   -   AR Camera should auto-assign (or drag Main Camera)
   -   Enable debug options for testing:
       - Show Debug Rays: ✅
       - Log Raycast Info: ✅ (optional, for troubleshooting)

#### 10. **Add Editor Simulation Support** ⭐ For Testing
   -   Create empty GameObject: `ARSceneSetup`
   -   Add `ARSceneSetup.cs` script
   -   Configure simulated planes:
       - Enable Editor Simulation: ✅
       - Auto Create Planes: ✅
       - Add multiple plane positions in array
   -   This creates visual planes in editor for testing

#### 11. **Optional Debug Tools**
   -   Create `DebugLogger` GameObject → Add `DebugLogger.cs`
   -   Add `ARPlaneVisualizer.cs` to XR Origin
   -   Add `XRSimulationHelper.cs` to a GameObject (optional extra debugging)

## ⚙️ Build Settings

### Android Build Settings
1.  **File** → **Build Settings** → Select **Android** → **Switch Platform**
2.  **Player Settings**:
    -   **Graphics APIs**: Remove `Vulkan`, keep only `OpenGLES3`
    -   **Minimum API Level**: `Android 7.0 'Nougat' (API level 24)` or higher
    -   **Target API Level**: Latest (API 33 or higher recommended)
    -   **Scripting Backend**: `IL2CPP`
    -   **Target Architectures**: Enable `ARM64` (disable ARMv7 for modern devices)
    -   **Camera Usage Description**: Add description for camera permissions

3.  **XR Plugin Management** (`Edit` → `Project Settings` → `XR Plugin Management`):
    -   Switch to **Android tab** (Android icon)
    -   Enable **ARCore** ✅

4.  **Quality Settings** (for better performance):
    -   `Edit` → `Project Settings` → `Quality`
    -   Set default quality to `Medium` or `Low` for mobile

### iOS Build Settings (Optional)
1.  **File** → **Build Settings** → Select **iOS** → **Switch Platform**
2.  **Player Settings**:
    -   **Camera Usage Description**: Required! Add text like "This app uses AR to place treasures"
    -   **Target Minimum iOS Version**: `11.0` or higher
    -   **Architecture**: `ARM64`

3.  **XR Plugin Management**:
    -   Switch to **iOS tab**
    -   Enable **ARKit** ✅

### Pre-Build Checklist ✅
- [ ] All scripts have no compilation errors
- [ ] Treasure prefab has TreasureCube script and Collider
- [ ] TreasureInputHandler is on AR Camera
- [ ] ARSceneSetup simulation mode is DISABLED for device build
- [ ] All UI references are assigned in GameManager
- [ ] AR managers are properly configured on XR Origin
- [ ] Test in Unity Editor simulation mode first

## ▶️ How to Play

### In Unity Editor (Simulation Mode)
1.  Press **Play** in Unity Editor
2.  Green transparent planes will appear (simulated AR surfaces)
3.  Treasures will spawn automatically after 2 seconds
4.  **Click** on treasures with mouse to collect them
5.  Or walk close (using WASD/arrow keys if you have movement) for auto-collection

### On AR Device (Real AR Mode)
1.  Launch the app on your AR-capable device
2.  Point device at a flat, well-lit, textured surface (floor, table, etc.)
3.  Move device slowly to help AR detect the environment
4.  Once planes are detected, treasures spawn automatically around you
5.  Follow the on-screen arrow and distance indicator
6.  **Tap treasures** to collect them, or walk within 1.5m for auto-collection
7.  Collect all treasures to win!

### Tips for Best Experience
-   🏠 Play in a well-lit room with textured surfaces (avoid blank walls)
-   📱 Move your device slowly at first to help AR calibrate
-   🎯 Use the directional arrow to find treasures efficiently
-   👆 Tap treasures from a distance or walk close for auto-collection
-   🎨 Watch for visual highlights when you're near a treasure

## 🔧 Configuration Options

### `GameManager`
| Property                          | Description                                      | Default |
| --------------------------------- | ------------------------------------------------ | ------- |
| `Total Treasures`                 | Number of treasures to spawn                     | `5`     |
| `Spawn Radius`                    | Max distance from player treasures can spawn     | `10m`   |
| `Min Distance Between Treasures`  | Minimum separation between spawned treasures     | `2m`    |

### `ARTreasureSpawner` ⭐
| Property                  | Description                                   | Default     |
| ------------------------- | --------------------------------------------- | ----------- |
| `Spawn Delay`             | Time delay between each treasure spawn        | `2s`        |
| `Min/Max Height`          | Vertical range for spawning                   | `0.1m-0.5m` |
| `Enable Simulation Mode`  | **Enable for editor testing**                 | `true`      |
| `Simulation Plane Y`      | Ground level for simulation mode              | `0`         |

### `TreasureCube`
| Property               | Description                                  | Default |
| ---------------------- | -------------------------------------------- | ------- |
| `Collection Distance`  | Proximity required to auto-collect           | `1.5m`  |
| `Highlight Distance`   | Proximity at which treasure highlights       | `3m`    |
| `Rotation Speed`       | Speed of cube spinning animation             | `50`    |
| `Bob Speed`            | Speed of up-and-down floating motion         | `1`     |
| `Bob Height`           | Height of bobbing animation                  | `0.2m`  |

### `TreasureInputHandler` ⭐
| Property                | Description                                  | Default |
| ----------------------- | -------------------------------------------- | ------- |
| `Max Raycast Distance`  | How far clicks can reach                     | `20m`   |
| `Show Debug Rays`       | Visualize click raycasts (editor only)       | `true`  |
| `Log Raycast Info`      | Print raycast details to console             | `false` |

### `ARSceneSetup` (Editor Only)
| Property                  | Description                              | Default |
| ------------------------- | ---------------------------------------- | ------- |
| `Enable Editor Simulation`| Creates visual planes in editor          | `true`  |
| `Auto Create Planes`      | Automatically spawn simulated planes     | `true`  |
| `Simulated Planes`        | Array of plane positions and sizes       | 3 planes|

## ⭐ Tips & Best Practices

### For Development
-   🖥️ **Test in editor first** using simulation mode before building to device
-   🐛 Use debug scripts (`DebugLogger`, `ARPlaneVisualizer`) during development
-   📊 Enable `Log Raycast Info` when troubleshooting clicking issues
-   💾 Save your scene frequently during setup
-   🔍 Use Scene view gizmos to visualize treasure distances

### For AR Experience
-   🏠 Test in well-lit environments with textured surfaces (carpets, patterned floors work great)
-   📱 Hold device steady for 2-3 seconds when starting to help AR initialize
-   🎯 Adjust `Spawn Radius` based on your intended play area size
-   🎨 Use distinct materials for highlighted vs. normal treasure states for clear visual feedback
-   ⚡ Start with fewer treasures (3-5) for better performance

### For UI Design
-   📐 World Space Canvas with Scale `0.001` and Distance `1-2m` works well
-   📝 Always use **TextMesh Pro** over legacy Text for sharper rendering
-   🎨 Use high contrast colors for UI elements (white text on dark background)
-   📏 Make UI elements slightly larger than you think - they appear smaller in AR
-   👁️ Keep critical UI in center of view, non-critical info at edges

### For Mobile Performance
-   ⚙️ Use `Medium` or `Low` quality settings for mobile devices
-   ✨ Keep particle effects simple and limited
-   🎮 Limit total treasures to 5-10 for smooth performance
-   📱 Test on lowest-spec device you plan to support
-   🔋 AR apps are battery-intensive - warn users

### For Better Gameplay
-   🎯 Balance auto-collection (1.5m) with manual tapping for player engagement
-   💡 Highlight treasures at 3m to give players visual feedback they're getting close
-   🔊 Add subtle audio cues for proximity (not implemented yet, but recommended)
-   ⏱️ Consider adding a timer or scoring system for replay value
-   🏆 Test spawn radius to ensure treasures are reachable but not too easy

## 🔍 Troubleshooting

### Editor Testing Issues

**Treasures not spawning in editor?**
-   ✅ Check `ARTreasureSpawner` → `Enable Simulation Mode` is checked
-   ✅ Verify `GameManager` exists in scene and is active
-   ✅ Check Console for error messages
-   ✅ Make sure `ARSceneSetup` is creating planes (look for green transparent cubes)

**Can't click treasures in editor?**
-   ✅ Add `TreasureInputHandler` to AR Camera if not already added
-   ✅ Verify treasure prefab has a Collider component
-   ✅ Check Console for "Clicked on treasure" or "Hit object" messages
-   ✅ Enable `Log Raycast Info` in TreasureInputHandler for debugging
-   ✅ Make sure simulated planes don't have colliders (they shouldn't)

**"Couldn't find a readme" error?**
-   ℹ️ This is harmless - a README asset was created to fix this

**Package resolution errors?**
-   ✅ Check `Packages/manifest.json` doesn't have invalid packages
-   ✅ Remove any `com.unity.xr.simulation` entries (not needed)
-   ✅ Refresh Unity: `Assets` → `Refresh` or Ctrl+R

### Device Testing Issues

**Treasures not spawning on device?**
-   📱 Ensure ARCore/ARKit is enabled in XR Plugin Management
-   🏠 Test in well-lit environment with textured surfaces
-   📷 Move device slowly to help detect planes
-   ⏱️ Wait 10 seconds - there's a timeout before simulation mode activates
-   🔧 Use `ARPlaneVisualizer` to confirm planes are being detected
-   ⚙️ **Disable** `Enable Simulation Mode` in ARTreasureSpawner for device builds

**Poor AR tracking?**
-   💡 Improve lighting in the room
-   🎨 Point at textured surfaces (not blank walls)
-   📏 Ensure adequate physical space around you
-   🔄 Restart the app to recalibrate AR

**Can't tap treasures on device?**
-   ✅ Verify `TreasureInputHandler` is on AR Camera
-   ✅ Check treasure prefab has Collider
-   ✅ Treasures might be too far - walk closer
-   ✅ Try tapping center of treasure cube
-   ℹ️ Auto-collection activates within 1.5m as backup

**UI not visible?**
-   📐 Check Canvas Scale is very small (e.g., 0.001)
-   📍 Verify Canvas position is in front of camera (e.g., Z: 1.5)
-   🎥 Ensure Canvas `Event Camera` is set to AR Camera
-   📝 Use TextMesh Pro (not legacy UI Text)

**Performance issues?**
-   🎮 Reduce `Total Treasures` in GameManager
-   ✨ Simplify or disable particle effects
-   📱 Lower Quality Settings: `Edit` → `Project Settings` → `Quality`
-   🔧 Use `Medium` or `Low` quality preset for mobile

### Build Issues

**Build fails with errors?**
-   ✅ Check all scripts compile without errors
-   ✅ Verify Minimum API Level is 24 or higher (Android)
-   ✅ Remove Vulkan from Graphics APIs (Android)
-   ✅ Enable IL2CPP and ARM64 (Android)

**App crashes on startup?**
-   📷 Check Camera permissions are granted
-   ✅ Verify ARCore/ARKit is enabled in XR Plug-in Management
-   ✅ Test on ARCore/ARKit compatible device
-   📱 Check device meets minimum OS requirements

## 🚀 Next Steps & Future Enhancements

### Immediate Improvements
-   🔊 Add sound effects for treasure collection and proximity alerts
-   ✨ Implement more complex particle effects for spawning and collection
-   🎨 Create different treasure types with varying point values
-   ⏱️ Add countdown timer for challenge mode
-   🏆 Implement high score system with PlayerPrefs

### Advanced Features
-   🌟 Power-ups (2x points, reveal all treasures, etc.)
-   🗺️ Minimap showing treasure locations
-   📊 Statistics screen (time played, treasures found, etc.)
-   🎵 Background music and ambient sounds
-   📱 Haptic feedback on treasure proximity (mobile only)
-   🌈 Trail renderers showing path to nearest treasure

### Multiplayer Ideas
-   👥 Local multiplayer (shared AR session)
-   🏁 Competitive mode (who finds most treasures in time limit)
-   🤝 Cooperative mode (find treasures together)
-   💬 Online leaderboards

### Polish & Optimization
-   📦 Object pooling for treasures and particles
-   🎨 More sophisticated shader effects
-   📱 Device-specific UI scaling and resolution settings
-   🌍 Localization for multiple languages
-   ♿ Accessibility options (larger UI, audio cues)

### Additional Resources
-   📖 See `CLICKING_FIX_GUIDE.md` for detailed clicking troubleshooting
-   📖 See `XR_Simulation_Setup_Guide.md` for editor simulation details
-   📖 See `Samsung_Galaxy_M51_UI_Guide.md` for device-specific UI setup

---

## 📱 Device Compatibility

**Tested/Recommended:**
-   Samsung Galaxy M51 and similar ARCore-compatible Android devices
-   Minimum: Android 7.0 (API 24), ARCore support required
-   Recommended: Android 10+ with ARCore 1.7+

**iOS:**
-   iPhone 6S and newer with ARKit support
-   iOS 11.0 or higher

## 📄 License & Credits

This is a learning project demonstrating AR Foundation capabilities.
Feel free to use, modify, and build upon this project for educational purposes.

**Built with:**
-   Unity 2021.3+ LTS
-   AR Foundation 5.2.0
-   ARCore XR Plugin 5.2.0

---

**Happy Treasure Hunting! 🎮✨**

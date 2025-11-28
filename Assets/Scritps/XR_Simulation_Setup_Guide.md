# AR Treasure Hunt - XR Simulation Troubleshooting Guide (Updated)

## Issues Fixed

### 1. **Added Editor Simulation Support**
- Modified `ARTreasureSpawner.cs` to work in both real AR and editor simulation
- Created `ARSceneSetup.cs` for visual plane simulation in editor
- Added fallback simulation mode that doesn't require XR Simulation package
- Removed dependency on unavailable XR Simulation package

### 2. **Fixed Package Dependencies**
- Removed invalid `com.unity.xr.simulation` package reference
- Project now uses only stable, available packages
- Added proper README asset to prevent editor errors

### 3. **Improved Error Handling**
- Added null checks for AR components
- Added detection for when AR systems are not available
- Created fallback positioning system for editor testing

## Setup Instructions

### 1. **Unity Editor Setup**
1. Open your project in Unity
2. Go to **Window > XR > XR Plug-in Management**
3. In the **XR Plug-in Management** settings:
   - Check **ARCore** under the Android icon (if targeting Android)
   - Check **ARKit** under the iOS icon (if targeting iOS)
   - **Note**: XR Simulation is not required with this setup

### 2. **Scene Setup**
1. Make sure your scene has these GameObjects:
   - **AR Session**
   - **XR Origin (or AR Camera)**
   - **AR Plane Manager**
   - **AR Raycast Manager** 
   - **AR Anchor Manager**

2. Add the **ARSceneSetup** script to any GameObject in your scene for editor simulation

### 3. **ARTreasureSpawner Configuration**
1. Select the GameObject with `ARTreasureSpawner` script
2. In the inspector, make sure:
   - **Enable Simulation Mode** is checked
   - **Simulation Plane Y** is set to 0 (ground level)
   - All AR component references are assigned

### 4. **Testing in Editor**
1. Press Play in the Unity Editor
2. The ARSceneSetup will create visual plane representations
3. Treasure spawning should start automatically after a short delay
4. Green transparent planes will show where AR planes would be detected

## New Script Features

### ARTreasureSpawner Improvements:
- **enableSimulationMode**: Toggle between real AR and simulation
- **simulationPlaneY**: Set the ground level for simulation
- **Timeout system**: Prevents infinite waiting for plane detection
- **Dual mode support**: Works in both editor and device

### XRSimulationHelper Features:
- **Automatic plane creation**: Creates simulated planes for testing
- **Debug information**: Shows AR system status
- **Visual feedback**: Displays simulation status on screen

## Troubleshooting

### If treasures still don't spawn:

1. **Check Console for Errors**
   - Look for messages starting with "AR Status" or "Treasure"
   - Common issues: Missing GameManager, unassigned prefabs

2. **Verify Component Assignment**
   - ARTreasureSpawner: Check all AR component references
   - GameManager: Make sure it exists in scene and is active

3. **Test Simulation Mode**
   - Make sure "Enable Simulation Mode" is checked
   - Try adjusting "Simulation Plane Y" value

4. **Manual Testing**
   - Add XRSimulationHelper to scene for detailed debug info
   - Check if GameManager.Instance is accessible

### Common Errors and Solutions:

**Error**: "GameManager.Instance is null"
**Solution**: Make sure GameManager script is in the scene and Awake() runs first

**Error**: "arPlaneManager is null" 
**Solution**: Assign AR Plane Manager in the ARTreasureSpawner inspector

**Error**: "No planes detected"
**Solution**: Enable simulation mode or wait for XR Simulation to create planes

## Testing Workflow

1. **Editor Testing**: Use simulation mode with XRSimulationHelper
2. **Device Testing**: Deploy to device with AR support
3. **Debugging**: Use DebugLogger script for runtime information

## Additional Notes

- The script now automatically detects if AR is available
- If AR isn't working, it falls back to simulation mode
- Simulation mode places treasures on a flat ground plane
- All previous functionality is preserved for real AR devices
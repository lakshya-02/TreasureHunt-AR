# Treasure Hunt AR

An Augmented Reality treasure hunting game where players use their mobile device camera to discover and collect virtual treasures in the real world.

## Demo Video



https://github.com/user-attachments/assets/6ceba10e-608b-464a-9af5-8ff9ff80822f



## Overview

Treasure Hunt AR is a mobile AR game built with Unity and AR Foundation that combines real-world exploration with virtual treasure hunting. Players scan their environment to spawn treasure chests, then navigate through physical space to collect them all and win the game.

## Features

- AR plane detection and treasure spawning
- Distance-based treasure interactions
- Visual effects (rotation, bobbing animation, highlighting)
- Audio feedback on treasure collection
- Victory screen with game statistics
- Restart and exit game functionality
- Mobile touch and mouse input support
- Fallback direct placement when planes are not detected

## Technical Stack

- Unity 2022.3 or later
- AR Foundation 5.2.0
- ARCore XR Plugin 5.2.0
- TextMesh Pro 3.0.7
- C# scripting

## Requirements

### Mobile Device
- Android device with ARCore support
- Android 7.0 (API level 24) or higher
- Camera permission

### Development
- Unity Editor 2022.3 or later
- Android Build Support module
- AR Foundation package
- ARCore XR Plugin package

## Setup Instructions

### 1. Clone Repository
```bash
git clone https://github.com/lakshya-02/Treasure-Hunt-AR-.git
cd Treasure-Hunt-AR-
```

### 2. Open in Unity
- Launch Unity Hub
- Click "Open" and select the project folder
- Wait for Unity to import all packages

### 3. Configure Build Settings
- Go to File > Build Settings
- Switch platform to Android
- Click "Switch Platform"

### 4. Configure Player Settings
- In Build Settings, click "Player Settings"
- Under "Other Settings":
  - Set Minimum API Level to Android 7.0 (API level 24)
  - Set Scripting Backend to IL2CPP
  - Enable ARM64 architecture
- Under "XR Plug-in Management":
  - Enable ARCore

### 5. Build and Deploy
- Connect Android device via USB
- Enable Developer Options and USB Debugging on device
- In Build Settings, click "Build and Run"
- Grant camera permissions when app launches

## Game Configuration

### GameManager Parameters
- `totalTreasures`: Number of treasures to spawn (default: 5)
- `spawnRadius`: Spawn radius around player (default: 10m)
- `minDistanceBetweenTreasures`: Minimum separation distance (default: 2m)
- `victoryUIDelay`: Victory screen delay (default: 2s)

### ARTreasureSpawner Parameters
- `planeDetectionTimeout`: Timeout for AR plane detection (default: 3s)
- `minHeight` / `maxHeight`: Treasure height variation

### TreasureCube Parameters
- `highlightDistance`: Distance for treasure highlighting (default: 3m)
- `collectionDistance`: Auto-collection distance (default: 1.5m)
- `rotationSpeed`: Treasure rotation speed
- `bobbingSpeed`: Bobbing animation speed

## Controls

- Point device camera at flat surfaces to detect AR planes
- Walk around to locate treasures
- Tap on highlighted treasures to collect them
- Treasures auto-collect when you get very close
- Collect all treasures to win

## Known Limitations

- Requires good lighting for AR plane detection
- Performance may vary on older devices
- ARCore support required for Android devices

## Unity Packages

Core dependencies:
- com.unity.xr.arfoundation: 5.2.0
- com.unity.xr.arcore: 5.2.0
- com.unity.textmeshpro: 3.0.7
- com.unity.ugui: 1.0.0

## Build Information

- Target Platform: Android
- Minimum Android Version: 7.0 (API 24)
- Architecture: ARM64
- Scripting Backend: IL2CPP

## License

This project is available for educational and personal use.

## Contact

For questions or issues, please open an issue on the GitHub repository.

Repository: https://github.com/lakshya-02/Treasure-Hunt-AR-

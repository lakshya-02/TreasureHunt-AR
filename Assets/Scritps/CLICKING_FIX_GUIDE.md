# Treasure Clicking Fix - Quick Setup

## Problem
Treasures were not clickable in simulation because:
1. Missing or disabled colliders on treasure cubes
2. `OnMouseDown()` doesn't work reliably with AR cameras
3. Simulated plane colliders were blocking raycasts

## Solutions Applied

### 1. TreasureCube.cs - Auto Collider Setup
- Automatically adds BoxCollider if missing
- Ensures collider is enabled for clicking

### 2. TreasureInputHandler.cs - NEW Script
- Handles both mouse (editor) and touch (mobile) input
- Uses proper raycasting from AR camera
- Ignores UI touches
- Shows debug information

### 3. ARTreasureSpawner.cs - Spawn Setup
- Ensures all spawned treasures have colliders
- Adds SphereCollider as fallback if needed

### 4. ARSceneSetup.cs - Plane Fix
- Removed colliders from simulated planes
- Planes are now visual-only and won't block clicks

## Setup Instructions

### Step 1: Add TreasureInputHandler to Scene
1. In Unity, find your AR camera or XR Origin
2. Add Component > **TreasureInputHandler**
3. Assign the AR Camera field (or it will use Camera.main)
4. Leave "Show Debug Rays" checked for testing

### Step 2: Verify Treasure Prefab
1. Find your treasure prefab in the project
2. Make sure it has:
   - TreasureCube script
   - A Collider (BoxCollider, SphereCollider, or MeshCollider)
   - A Renderer with a material

### Step 3: Test in Editor
1. Press Play
2. Click directly on a treasure cube
3. Check console for "Clicked on treasure" message
4. Treasure should collect when clicked

## Troubleshooting

### Clicks Not Working?
1. **Check Console**: Look for "Clicked on treasure" or "Hit object" messages
2. **Enable Debug**: In TreasureInputHandler, enable "Log Raycast Info"
3. **Verify Camera**: Make sure AR Camera is assigned in TreasureInputHandler
4. **Check Layers**: Treasures should not be on an ignored layer

### Still Can't Click?
1. Select treasure in Hierarchy
2. Look for Collider component in Inspector
3. Make sure "Is Trigger" is UNCHECKED
4. Try clicking on the center of the cube
5. Check if UI is blocking (disable UI temporarily)

## Debug Features

### TreasureInputHandler Debug Options:
- **Show Debug Rays**: Draws red lines showing raycast direction
- **Log Raycast Info**: Prints detailed raycast information to console
- **Max Raycast Distance**: Adjust if treasures are far away (default: 20m)

### Console Messages to Look For:
- ✅ "Clicked on treasure: TreasureName" - Success!
- ⚠️ "Hit object: ObjectName" - Clicking something else
- ⚠️ "Raycast hit nothing" - Missing/disabled colliders
- ⚠️ "AR Camera not assigned!" - Need to assign camera

## Testing Tips

1. **Editor Testing**: Use mouse clicks
2. **Mobile Testing**: Use touch input (works automatically)
3. **Distance**: Stand close to treasures for easier clicking
4. **Auto-Collection**: Treasures auto-collect when within 1.5m
5. **Visual Feedback**: Treasures highlight when within 3m

## Quick Fixes

### If treasures spawn but can't click:
```
Select treasure in Hierarchy
→ Add Component → Box Collider
→ Make sure it's enabled
```

### If clicks go through treasures:
```
Check TreasureInputHandler is on AR camera
Check camera is assigned in inspector
Increase Max Raycast Distance
```

### If getting "Hit object: SimulatedARPlane":
```
This should be fixed - planes no longer have colliders
If still happening, manually delete plane colliders
```
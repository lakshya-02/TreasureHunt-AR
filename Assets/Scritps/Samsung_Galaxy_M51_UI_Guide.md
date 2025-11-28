# Samsung Galaxy M51 - UI Configuration Guide

## 📱 Device Specifications

**Samsung Galaxy M51 Display:**
- **Resolution**: 2400 × 1080 pixels (Full HD+)
- **Screen Size**: 6.7 inches
- **Aspect Ratio**: 20:9
- **Pixel Density**: ~394 PPI
- **Display Type**: Super AMOLED Plus

**AR Capabilities:**
- ✅ ARCore Compatible
- Android Version: 10 (upgradable to Android 11/12)
- Processor: Snapdragon 730G
- RAM: 6GB/8GB variants

---

## 🎨 Recommended UI Settings for Galaxy M51

### Canvas Configuration

#### World Space Canvas (Recommended for AR)
```
Canvas Component:
├─ Render Mode: World Space
├─ Event Camera: Main Camera (AR Camera)
├─ Sorting Layer: Default
└─ Order in Layer: 0

Transform:
├─ Position: (0, 0.5, 2.0)  // Relative to camera
├─ Rotation: (0, 0, 0)
└─ Scale: (0.0015, 0.0015, 0.0015)  // Adjusted for 6.7" screen

Canvas Scaler:
├─ UI Scale Mode: Constant Pixel Size
├─ Scale Factor: 1
├─ Reference Pixels Per Unit: 100
└─ Dynamic Pixels Per Unit: 1
```

**Why these values?**
- Scale `0.0015` provides good readability on the large 6.7" display
- Position Z `2.0` keeps UI comfortably in view without being too close
- Position Y `0.5` places UI slightly above center for ergonomic viewing

#### Alternative: Screen Space Canvas
```
Canvas Component:
├─ Render Mode: Screen Space - Camera
├─ Render Camera: Main Camera (AR Camera)
└─ Plane Distance: 1.0

Canvas Scaler:
├─ UI Scale Mode: Scale With Screen Size
├─ Reference Resolution: (1080, 2400)  // M51 native resolution
├─ Screen Match Mode: Match Width Or Height
└─ Match: 0.5 (balanced)
```

---

## 📐 UI Element Sizing Guide

### Text Elements (TextMeshPro)

#### Score Text
```
Rect Transform:
├─ Width: 300
├─ Height: 80
└─ Position: Top-left or top-center

TextMeshPro Settings:
├─ Font Size: 48-56
├─ Font Style: Bold
├─ Color: White with black outline
├─ Alignment: Center
└─ Auto Size: Disabled
```

#### Treasure Counter
```
Rect Transform:
├─ Width: 200
├─ Height: 70
└─ Position: Top-right

TextMeshPro Settings:
├─ Font Size: 44-52
├─ Font Style: Bold
├─ Color: #FFD700 (Gold) or White
└─ Alignment: Center
```

#### Distance Indicator
```
Rect Transform:
├─ Width: 280
├─ Height: 60
└─ Position: Bottom-center or top-center

TextMeshPro Settings:
├─ Font Size: 38-44
├─ Font Style: Regular or Bold
├─ Color: White or Yellow
└─ Alignment: Center
```

### UI Images

#### Direction Arrow
```
Rect Transform:
├─ Width: 120-150
├─ Height: 120-150
└─ Position: Center or slightly above center

Image Component:
├─ Preserve Aspect: Enabled
├─ Raycast Target: Disabled (performance)
└─ Color: Yellow/Gold (#FFD700) or White
```

#### Victory Panel
```
Rect Transform:
├─ Anchor: Stretch (fills screen)
├─ Left/Right/Top/Bottom: 100 (margin from edges)
└─ Scale: (1, 1, 1)

Panel Settings:
├─ Color: Semi-transparent black (0, 0, 0, 0.8)
└─ Include child elements for victory text and buttons
```

---

## 📏 Safe Area & Notch Handling

Samsung Galaxy M51 has a **centered punch-hole camera** (Infinity-O display).

### Punch-Hole Safe Zone
```
Top Safe Area:
├─ Avoid placing critical UI in top 120 pixels
├─ Center width 80 pixels reserved for camera
└─ Recommended: Place UI 150+ pixels from top

Best Practice:
├─ Use Safe Area component/script
├─ Offset top UI by 150-200 pixels
└─ Or use bottom-anchored UI for critical info
```

### Safe Area Script (Optional)
```csharp
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaHandler : MonoBehaviour
{
    private RectTransform rectTransform;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }
    
    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
```

---

## 🎯 Recommended UI Layout for Galaxy M51

### Layout Option 1: Minimalist AR (Recommended)
```
┌─────────────────────────────┐
│   [Score: 3]    [3/5]       │ ← Top bar (150px from top)
│                             │
│                             │
│        [Direction           │
│          Arrow]             │ ← Center (floating)
│                             │
│                             │
│     [Distance: 5.2m]        │ ← Below arrow
│                             │
│                             │
└─────────────────────────────┘
```

### Layout Option 2: Full Info Display
```
┌─────────────────────────────┐
│ [Score: 3]      [3/5]       │ ← Top (avoid camera hole)
│                             │
│   [↑ Direction Arrow]       │ ← Upper center
│   [Distance: 5.2m]          │
│                             │
│   (AR View / Game Space)    │
│                             │
│                             │
│ [Debug: FPS, Position]      │ ← Bottom (optional)
└─────────────────────────────┘
```

### Layout Option 3: World Space HUD
```
All UI follows camera in world space
├─ Small floating panel above treasures
├─ Score in top-left of view
├─ Arrow pointing in 3D space
└─ Distance updates in real-time
```

---

## 🎨 Visual Recommendations for AMOLED Display

### Colors That Pop on AMOLED
```css
Excellent Choices:
├─ Pure White: #FFFFFF (high contrast, battery efficient)
├─ Bright Yellow/Gold: #FFD700 (treasure theme)
├─ Cyan/Blue: #00FFFF (AR aesthetic)
├─ Lime Green: #00FF00 (visibility)
└─ Magenta: #FF00FF (highlights)

Avoid:
├─ Pure Black UI on black backgrounds (invisible!)
├─ Very dark grays (poor contrast)
└─ Dull colors (AMOLED can show vibrant colors - use them!)

Background Panels:
├─ Semi-transparent dark: (0, 0, 0, 0.6-0.8)
├─ Blur effect if supported
└─ Gradient overlays for depth
```

### Text Readability
```
Best Practices:
├─ White text with black outline (2-3px)
├─ Font size 38+ for readability
├─ Bold or ExtraBold weights
├─ High contrast ratio (minimum 4.5:1)
└─ Drop shadows for depth

TextMeshPro Outline Settings:
├─ Outline Color: Black
├─ Outline Width: 0.2-0.3
└─ Enable outline in Material settings
```

---

## ⚙️ Performance Optimization for M51

### Snapdragon 730G Considerations
```
Canvas Settings:
├─ Pixel Perfect: Disabled (performance)
├─ Additional Shader Channels: None (unless needed)
└─ Raycast Target: Disable on non-interactive elements

Quality Settings:
├─ Use Medium quality preset
├─ Texture Quality: Medium
├─ Anti-aliasing: 2x or disabled
└─ Shadow Quality: Soft Shadows or disabled

Frame Rate:
├─ Target: 30 FPS (AR standard)
├─ VSync: On (recommended for AR)
└─ Fixed Timestep: 0.033 (30 FPS)
```

### Battery Optimization
```
AR Session:
├─ Match Frame Rate: Disabled (unless needed)
├─ Light Estimation: Disabled (if not used)
└─ Plane Detection: Horizontal only (if sufficient)

Graphics:
├─ Use Baked Lighting when possible
├─ Limit real-time shadows
└─ Optimize particle effects (max 2-3 systems)
```

---

## 🧪 Testing Checklist for Galaxy M51

### Before Building
- [ ] Canvas scale appropriate for 6.7" display
- [ ] Text readable at arm's length (~50cm)
- [ ] UI elements don't overlap with punch-hole camera
- [ ] All text uses TextMeshPro
- [ ] Colors have good contrast on AMOLED
- [ ] Safe area margins applied (top 150px minimum)

### On Device Testing
- [ ] UI visible in bright outdoor light
- [ ] UI visible in dim indoor light
- [ ] Text is sharp and readable
- [ ] Touch targets are large enough (minimum 60x60 pixels)
- [ ] No UI elements behind notch
- [ ] 30 FPS maintained during gameplay
- [ ] Battery drain is acceptable (<20% per 30 min)
- [ ] App doesn't overheat during extended play

### AR Specific
- [ ] UI visible while looking at treasures
- [ ] Direction arrow clearly indicates direction
- [ ] Distance updates smoothly
- [ ] No UI lag when moving device
- [ ] World space UI faces camera correctly

---

## 📱 Quick Setup Template

### For Quick Galaxy M51 Setup:
```
1. Use Screen Space - Camera canvas
2. Reference Resolution: (1080, 2400)
3. Font Size: 48+ for primary text
4. Top margin: 150px minimum
5. Touch targets: 60x60 minimum
6. Test at arm's length in real lighting
```

### World Space Quick Setup:
```
1. Canvas Scale: (0.0015, 0.0015, 0.0015)
2. Position: (0, 0.5, 2.0) relative to camera
3. Font Size: 48-56
4. Parent to camera or use follow script
5. Test movement in AR
```

---

## 🔧 Common Issues & Solutions

### Issue: UI too small
**Solution**: Increase canvas scale to `0.002` or font sizes by 20%

### Issue: UI too large/close
**Solution**: Decrease canvas scale to `0.001` or increase Z position to `3.0`

### Issue: Text blurry
**Solution**: 
- Use TextMeshPro (not legacy Text)
- Enable "Extra Padding" in TMP settings
- Increase font atlas resolution

### Issue: UI behind camera notch
**Solution**: 
- Add 150-200px top margin
- Use SafeAreaHandler script
- Test on actual device

### Issue: Poor performance
**Solution**:
- Disable Pixel Perfect on canvas
- Set Quality to Medium
- Reduce particle effects
- Disable raycast on non-interactive UI

### Issue: Colors look washed out
**Solution**:
- Use brighter, more saturated colors on AMOLED
- Add outlines to text for contrast
- Use pure white (#FFFFFF) for important text

---

## 📊 Resolution Scale Reference

```
If targeting multiple devices:

Small phones (5.5"):     Scale: 0.0010
Medium phones (6.0-6.5"): Scale: 0.0012-0.0015
Large phones (6.5-7.0"): Scale: 0.0015-0.0020 ← Galaxy M51
Tablets (8"+):           Scale: 0.0025-0.0040

Adjust based on actual testing!
```

---

**Pro Tip**: Always test on the actual device! The 6.7" AMOLED display of the Galaxy M51 can look very different from Unity's Game view or smaller test devices.

**Remember**: What looks good in Unity Editor may be too small or too large on device. Build early, test often! 🚀
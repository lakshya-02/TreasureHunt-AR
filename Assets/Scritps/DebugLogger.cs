using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simplified debug logger - Only enabled when needed
/// Toggle with 'D' key during play
/// </summary>
public class DebugLogger : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool showDebugInfo = false; // Disabled by default
    [SerializeField] private KeyCode toggleKey = KeyCode.D;
    
    [Header("What to Show")]
    [SerializeField] private bool showFPS = true;
    [SerializeField] private bool showTreasureCount = true;
    [SerializeField] private bool showPlayerPosition = false; // Less clutter

    [Header("UI Settings")]
    [SerializeField] private int fontSize = 18; // Smaller, less intrusive
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private bool showBackground = true;

    private float deltaTime = 0.0f;
    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
    }

    void Update()
    {
        // Toggle debug info with key press
        if (Input.GetKeyDown(toggleKey))
        {
            showDebugInfo = !showDebugInfo;
            Debug.Log($"Debug Info: {(showDebugInfo ? "ON" : "OFF")}");
        }

        if (showFPS)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }
    }

    void OnGUI()
    {
        if (!showDebugInfo) return;

        int w = Screen.width;
        int h = Screen.height;

        // Background box for readability
        if (showBackground)
        {
            GUI.Box(new Rect(5, 5, 220, GetDebugHeight()), "");
        }

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = fontSize;
        style.normal.textColor = textColor;

        string text = "";

        if (showFPS)
        {
            float fps = 1.0f / deltaTime;
            text += string.Format("FPS: {0:0}\n", fps);
        }

        if (showTreasureCount)
        {
            TreasureCube[] treasures = FindObjectsOfType<TreasureCube>();
            text += $"Treasures: {treasures.Length}\n";
        }

        if (showPlayerPosition && arCamera != null)
        {
            Vector3 pos = arCamera.transform.position;
            text += $"Pos: ({pos.x:F1}, {pos.y:F1}, {pos.z:F1})\n";
        }

        text += $"\nPress '{toggleKey}' to toggle";

        GUI.Label(rect, text, style);
    }

    private int GetDebugHeight()
    {
        int lines = 1; // Toggle instruction
        if (showFPS) lines++;
        if (showTreasureCount) lines++;
        if (showPlayerPosition) lines++;
        return lines * (fontSize + 5) + 10;
    }

    public void ToggleDebug()
    {
        showDebugInfo = !showDebugInfo;
    }

    // Quick enable/disable methods
    public void EnableDebug() => showDebugInfo = true;
    public void DisableDebug() => showDebugInfo = false;
}

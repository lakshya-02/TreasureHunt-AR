using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages UI layout and alignment for better visual appearance
/// Handles both World Space and Screen Space canvases
/// </summary>
public class UILayoutManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private RectTransform topPanel;
    [SerializeField] private RectTransform centerPanel;
    [SerializeField] private RectTransform bottomPanel;
    [SerializeField] private RectTransform victoryPanel;

    [Header("Top Panel Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI treasureCountText;

    [Header("Center Panel Elements")]
    [SerializeField] private Image directionArrow;
    [SerializeField] private TextMeshProUGUI distanceText;

    [Header("Layout Settings")]
    [SerializeField] private float topPanelMargin = 50f;
    [SerializeField] private float bottomPanelMargin = 50f;
    [SerializeField] private float elementSpacing = 20f;

    [Header("Auto-Align Options")]
    [SerializeField] private bool autoAlignOnStart = true;
    [SerializeField] private bool keepPanelsMinimal = true;

    void Start()
    {
        if (autoAlignOnStart)
        {
            AlignAllPanels();
        }
    }

    public void AlignAllPanels()
    {
        AlignTopPanel();
        AlignCenterPanel();
        AlignBottomPanel();
        ConfigureTextElements();
    }

    private void AlignTopPanel()
    {
        if (topPanel == null) return;

        // Anchor to top of screen
        topPanel.anchorMin = new Vector2(0, 1);
        topPanel.anchorMax = new Vector2(1, 1);
        topPanel.pivot = new Vector2(0.5f, 1);

        // Position from top with margin
        topPanel.anchoredPosition = new Vector2(0, -topPanelMargin);
        topPanel.sizeDelta = new Vector2(-40, 80); // Full width minus padding, fixed height

        // Layout group for proper spacing
        if (!topPanel.GetComponent<HorizontalLayoutGroup>())
        {
            HorizontalLayoutGroup layoutGroup = topPanel.gameObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.spacing = elementSpacing;
            layoutGroup.padding = new RectOffset(20, 20, 10, 10);
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;
        }
    }

    private void AlignCenterPanel()
    {
        if (centerPanel == null) return;

        // Anchor to center of screen
        centerPanel.anchorMin = new Vector2(0.5f, 0.5f);
        centerPanel.anchorMax = new Vector2(0.5f, 0.5f);
        centerPanel.pivot = new Vector2(0.5f, 0.5f);

        // Center position with slight offset upward for better viewing
        centerPanel.anchoredPosition = new Vector2(0, 50);
        centerPanel.sizeDelta = new Vector2(300, 300);

        // Vertical layout for arrow and distance
        if (!centerPanel.GetComponent<VerticalLayoutGroup>())
        {
            VerticalLayoutGroup layoutGroup = centerPanel.gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.spacing = 15;
            layoutGroup.padding = new RectOffset(10, 10, 10, 10);
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;
        }
    }

    private void AlignBottomPanel()
    {
        if (bottomPanel == null) return;

        // Anchor to bottom of screen
        bottomPanel.anchorMin = new Vector2(0, 0);
        bottomPanel.anchorMax = new Vector2(1, 0);
        bottomPanel.pivot = new Vector2(0.5f, 0);

        // Position from bottom with margin
        bottomPanel.anchoredPosition = new Vector2(0, bottomPanelMargin);
        bottomPanel.sizeDelta = new Vector2(-40, 60);
    }

    private void ConfigureTextElements()
    {
        // Score text alignment
        if (scoreText != null)
        {
            scoreText.alignment = TextAlignmentOptions.Left;
            scoreText.fontSize = 48;
            scoreText.fontStyle = FontStyles.Bold;
            scoreText.enableAutoSizing = false;
        }

        // Treasure count text alignment
        if (treasureCountText != null)
        {
            treasureCountText.alignment = TextAlignmentOptions.Right;
            treasureCountText.fontSize = 48;
            treasureCountText.fontStyle = FontStyles.Bold;
            treasureCountText.enableAutoSizing = false;
        }

        // Distance text alignment
        if (distanceText != null)
        {
            distanceText.alignment = TextAlignmentOptions.Center;
            distanceText.fontSize = 44;
            distanceText.fontStyle = FontStyles.Bold;
            distanceText.enableAutoSizing = false;
        }
    }

    // Call this to refresh layout at runtime
    public void RefreshLayout()
    {
        AlignAllPanels();
    }

    // Minimal mode - hides panels until needed
    public void SetMinimalMode(bool enabled)
    {
        keepPanelsMinimal = enabled;
        
        if (centerPanel != null)
        {
            CanvasGroup cg = centerPanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = centerPanel.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = enabled ? 0.7f : 1f;
        }
    }

#if UNITY_EDITOR
    // Editor helper to test alignment
    [ContextMenu("Align All Panels")]
    private void EditorAlignPanels()
    {
        AlignAllPanels();
        Debug.Log("UI Panels aligned!");
    }
#endif
}

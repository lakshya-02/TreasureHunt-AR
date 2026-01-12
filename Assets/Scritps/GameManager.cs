using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int totalTreasures = 5;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minDistanceBetweenTreasures = 2f;
    [SerializeField] private float victoryUIDelay = 2f;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI treasureCountText;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject[] otherUIElements;

    private int treasuresFound = 0;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        
        if (victoryUI != null)
            victoryUI.SetActive(false);
    }

    public void OnTreasureFound()
    {
        treasuresFound++;
        UpdateUI();

        if (treasuresFound >= totalTreasures)
        {
            GameWon();
        }
    }

    private void UpdateUI()
    {
        if (treasureCountText != null)
            treasureCountText.text = $"{treasuresFound} / {totalTreasures}";
    }

    private void GameWon()
    {
        StartCoroutine(ShowVictoryUIWithDelay());
    }

    private IEnumerator ShowVictoryUIWithDelay()
    {
        yield return new WaitForSeconds(victoryUIDelay);
        
        if (victoryUI != null)
            victoryUI.SetActive(true);
        
        foreach (GameObject uiElement in otherUIElements)
        {
            if (uiElement != null)
                uiElement.SetActive(false);
        }
    }

    public bool IsValidSpawnPosition(Vector3 position)
    {
        foreach (Vector3 spawnedPos in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPos) < minDistanceBetweenTreasures)
            {
                return false;
            }
        }
        return true;
    }

    public void RegisterSpawnPosition(Vector3 position)
    {
        spawnedPositions.Add(position);
    }

    public int GetTotalTreasures()
    {
        return totalTreasures;
    }

    public float GetSpawnRadius()
    {
        return spawnRadius;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void ExitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

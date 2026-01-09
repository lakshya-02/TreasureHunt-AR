using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureTracker : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image directionArrow;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private GameObject trackerUI;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private bool showDistance = true;
    [SerializeField] private bool showDirection = true;

    private Camera arCamera;
    private GameObject nearestTreasure;
    private List<GameObject> allTreasures = new List<GameObject>();

    void Start()
    {
        arCamera = Camera.main;
        StartCoroutine(UpdateTracker());
    }

    void Update()
    {
        FindAllTreasures();
        FindNearestTreasure();

        if (nearestTreasure != null)
        {
            if (showDirection && directionArrow != null)
            {
                UpdateDirectionArrow();
            }

            if (showDistance && distanceText != null)
            {
                UpdateDistanceText();
            }

            if (trackerUI != null && !trackerUI.activeSelf)
            {
                trackerUI.SetActive(true);
            }
        }
        else
        {
            if (trackerUI != null && trackerUI.activeSelf)
            {
                trackerUI.SetActive(false);
            }
        }
    }

    private void FindAllTreasures()
    {
        allTreasures.Clear();
        TreasureCube[] treasures = FindObjectsOfType<TreasureCube>();
        
        foreach (TreasureCube treasure in treasures)
        {
            // Only add treasures that haven't been collected yet
            if (treasure != null && !treasure.IsCollected)
            {
                allTreasures.Add(treasure.gameObject);
            }
        }
    }

    private void FindNearestTreasure()
    {
        if (allTreasures.Count == 0)
        {
            nearestTreasure = null;
            return;
        }

        float minDistance = float.MaxValue;
        GameObject nearest = null;

        foreach (GameObject treasure in allTreasures)
        {
            if (treasure == null) continue;

            float distance = Vector3.Distance(arCamera.transform.position, treasure.transform.position);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = treasure;
            }
        }

        nearestTreasure = nearest;
    }

    private void UpdateDirectionArrow()
    {
        if (directionArrow == null || nearestTreasure == null) return;

        // Calculate direction to treasure
        Vector3 directionToTreasure = nearestTreasure.transform.position - arCamera.transform.position;
        directionToTreasure.y = 0; // Keep it horizontal

        // Get camera forward direction (horizontal plane)
        Vector3 cameraForward = arCamera.transform.forward;
        cameraForward.y = 0;

        // Calculate angle between camera forward and treasure direction
        float angle = Vector3.SignedAngle(cameraForward, directionToTreasure, Vector3.up);

        // Rotate arrow
        directionArrow.transform.rotation = Quaternion.Euler(0, 0, -angle);

        // Optional: Change arrow color based on distance
        float distance = Vector3.Distance(arCamera.transform.position, nearestTreasure.transform.position);
        if (distance < 2f)
        {
            directionArrow.color = Color.green;
        }
        else if (distance < 5f)
        {
            directionArrow.color = Color.yellow;
        }
        else
        {
            directionArrow.color = Color.white;
        }
    }

    private void UpdateDistanceText()
    {
        if (distanceText == null || nearestTreasure == null) return;

        float distance = Vector3.Distance(arCamera.transform.position, nearestTreasure.transform.position);
        distanceText.text = distance.ToString("F1") + "m";
    }

    private IEnumerator UpdateTracker()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
        }
    }
}

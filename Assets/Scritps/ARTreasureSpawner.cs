using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTreasureSpawner : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private ARAnchorManager arAnchorManager;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private Camera arCamera;

    [Header("Treasure Prefab")]
    [SerializeField] private GameObject treasurePrefab;

    [Header("Spawn Settings")]
    //[SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float minHeight = 0.1f;
    [SerializeField] private float maxHeight = 0.5f;
    [SerializeField] private float planeDetectionTimeout = 3f; // Timeout for plane detection

    private List<ARAnchor> spawnedAnchors = new List<ARAnchor>();
    private int treasuresSpawned = 0;
    private bool useDirectPlacement = false;

    void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main;

        StartCoroutine(WaitForPlaneDetection());
    }

    private IEnumerator WaitForPlaneDetection()
    {
        float elapsed = 0f;
        
        // Wait until at least one plane is detected or timeout
        while ((arPlaneManager == null || arPlaneManager.trackables.count == 0) && elapsed < planeDetectionTimeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (elapsed >= planeDetectionTimeout)
        {
            Debug.LogWarning("AR Plane detection timeout - using direct placement mode");
            useDirectPlacement = true;
        }
        else
        {
            Debug.Log("AR Planes detected. Starting treasure spawn...");
        }

        // Wait a bit for stabilization
        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnTreasures());
    }

    private IEnumerator SpawnTreasures()
    {
        int totalTreasures = GameManager.Instance.GetTotalTreasures();
        float spawnRadius = GameManager.Instance.GetSpawnRadius();

        // Spawn all treasures at once
        while (treasuresSpawned < totalTreasures)
        {
            Vector3 randomPosition = GetRandomPositionAroundPlayer(spawnRadius);
            
            if (TryPlaceAnchorAtPosition(randomPosition))
            {
                treasuresSpawned++;
                Debug.Log($"Treasure {treasuresSpawned}/{totalTreasures} spawned!");
            }

            // No delay - spawn all immediately
            yield return null;
        }

        Debug.Log("All treasures spawned!");
    }

    private Vector3 GetRandomPositionAroundPlayer(float radius)
    {
        Vector3 playerPosition = arCamera.transform.position;
        
        // Generate random position in a circle around the player
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(radius * 0.3f, radius);
        
        float x = playerPosition.x + distance * Mathf.Cos(angle);
        float z = playerPosition.z + distance * Mathf.Sin(angle);
        
        return new Vector3(x, playerPosition.y, z);
    }

    private bool TryPlaceAnchorAtPosition(Vector3 targetPosition)
    {
        // If using direct placement (no AR planes detected), place directly
        if (useDirectPlacement)
        {
            return TryDirectPlacement(targetPosition);
        }
        
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        
        // Raycast from above the target position downward
        Vector3 rayOrigin = new Vector3(targetPosition.x, targetPosition.y + 5f, targetPosition.z);
        Vector3 rayDirection = Vector3.down;

        if (arRaycastManager != null && arRaycastManager.Raycast(new Ray(rayOrigin, rayDirection), hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            
            // Check if position is valid (not too close to other treasures)
            if (!GameManager.Instance.IsValidSpawnPosition(hitPose.position))
            {
                return false;
            }

            // Adjust height slightly
            Vector3 spawnPosition = hitPose.position;
            spawnPosition.y += Random.Range(minHeight, maxHeight);

            return CreateAnchorAndTreasure(spawnPosition, hitPose.rotation);
        }

        return false;
    }

    private bool TryDirectPlacement(Vector3 targetPosition)
    {
        // Place at ground level (Y=0) for fallback
        Vector3 groundPosition = new Vector3(targetPosition.x, 0f, targetPosition.z);
        
        // Check if position is valid
        if (!GameManager.Instance.IsValidSpawnPosition(groundPosition))
        {
            return false;
        }

        // Adjust height slightly
        Vector3 spawnPosition = groundPosition;
        spawnPosition.y += Random.Range(minHeight, maxHeight);

        return CreateAnchorAndTreasure(spawnPosition, Quaternion.identity);
    }

    private bool CreateAnchorAndTreasure(Vector3 position, Quaternion rotation)
    {
        // Create anchor
        GameObject anchorObject = new GameObject("TreasureAnchor");
        anchorObject.transform.position = position;
        anchorObject.transform.rotation = rotation;
        
        // Only add ARAnchor if not in direct placement mode
        if (!useDirectPlacement && arAnchorManager != null)
        {
            ARAnchor anchor = anchorObject.AddComponent<ARAnchor>();
            if (anchor != null)
            {
                spawnedAnchors.Add(anchor);
            }
        }
        
        // Instantiate treasure as child of anchor with local position zero
        GameObject treasure = Instantiate(treasurePrefab, anchorObject.transform);
        treasure.transform.localPosition = Vector3.zero;
        treasure.transform.localRotation = Quaternion.identity;
        
        // Ensure treasure has proper setup for interaction
        SetupTreasureForInteraction(treasure);
        
        // Register position with game manager
        GameManager.Instance.RegisterSpawnPosition(position);
        
        Debug.Log($"Treasure spawned at position: {position}");
        return true;
    }

    private void SetupTreasureForInteraction(GameObject treasure)
    {
        // Ensure the treasure has a collider
        Collider col = treasure.GetComponent<Collider>();
        if (col == null)
        {
            col = treasure.GetComponentInChildren<Collider>();
        }
        
        if (col == null)
        {
            // Add a sphere collider as fallback
            SphereCollider sphereCol = treasure.AddComponent<SphereCollider>();
            sphereCol.radius = 0.5f;
            Debug.Log($"Added SphereCollider to treasure: {treasure.name}");
        }
        
        // Make sure collider is enabled
        if (col != null)
        {
            col.enabled = true;
        }
    }

    public void ClearAllAnchors()
    {
        foreach (ARAnchor anchor in spawnedAnchors)
        {
            if (anchor != null)
            {
                Destroy(anchor.gameObject);
            }
        }
        spawnedAnchors.Clear();
        treasuresSpawned = 0;
    }

    private void OnDestroy()
    {
        ClearAllAnchors();
    }
}

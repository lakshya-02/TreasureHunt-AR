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

    [Header("Fallback Settings")]
    [SerializeField] private bool enableSimulationMode = true;
    [SerializeField] private float simulationPlaneY = 0f;

    [Header("Treasure Prefab")]
    [SerializeField] private GameObject treasurePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float minHeight = 0.1f;
    [SerializeField] private float maxHeight = 0.5f;

    private List<ARAnchor> spawnedAnchors = new List<ARAnchor>();
    private int treasuresSpawned = 0;
    //private bool canSpawn = false;

    void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main;

        // Check if we're in editor/simulation mode
        bool isSimulation = Application.isEditor || !IsARSupported();
        
        if (isSimulation && enableSimulationMode)
        {
            Debug.Log("Running in simulation mode - bypassing plane detection");
            StartCoroutine(SimulationSpawnDelay());
        }
        else
        {
            StartCoroutine(WaitForPlaneDetection());
        }
    }

    private bool IsARSupported()
    {
        return arRaycastManager != null && arPlaneManager != null && 
               arRaycastManager.enabled && arPlaneManager.enabled;
    }

    private IEnumerator SimulationSpawnDelay()
    {
        // Small delay to ensure everything is initialized
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Starting simulation treasure spawn...");
        StartCoroutine(SpawnTreasures());
    }

    private IEnumerator WaitForPlaneDetection()
    {
        // Add timeout for plane detection
        float timeout = 10f;
        float elapsed = 0f;
        
        // Wait until at least one plane is detected or timeout
        while ((arPlaneManager == null || arPlaneManager.trackables.count == 0) && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (elapsed >= timeout)
        {
            Debug.LogWarning("Plane detection timeout - switching to simulation mode");
            if (enableSimulationMode)
            {
                StartCoroutine(SpawnTreasures());
                yield break;
            }
        }
        
        Debug.Log("Planes detected. Starting treasure spawn...");

        // Wait a bit for plane stabilization
        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnTreasures());
    }

    private IEnumerator SpawnTreasures()
    {
        int totalTreasures = GameManager.Instance.GetTotalTreasures();
        float spawnRadius = GameManager.Instance.GetSpawnRadius();

        while (treasuresSpawned < totalTreasures)
        {
            Vector3 randomPosition = GetRandomPositionAroundPlayer(spawnRadius);
            
            if (TryPlaceAnchorAtPosition(randomPosition))
            {
                treasuresSpawned++;
                Debug.Log($"Treasure {treasuresSpawned}/{totalTreasures} spawned!");
            }

            yield return new WaitForSeconds(spawnDelay);
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
        // Check if we should use simulation mode
        bool useSimulation = Application.isEditor || !IsARSupported() || 
                           (arRaycastManager != null && !arRaycastManager.enabled);
                           
        if (useSimulation && enableSimulationMode)
        {
            return TryPlaceAnchorSimulation(targetPosition);
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

    private bool TryPlaceAnchorSimulation(Vector3 targetPosition)
    {
        // Simulate placing on a ground plane
        Vector3 groundPosition = new Vector3(targetPosition.x, simulationPlaneY, targetPosition.z);
        
        // Check if position is valid (not too close to other treasures)
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
        // Create anchor using the new method
        GameObject anchorObject = new GameObject("TreasureAnchor");
        anchorObject.transform.position = position;
        anchorObject.transform.rotation = rotation;
        
        // Only add ARAnchor if AR is supported
        ARAnchor anchor = null;
        if (IsARSupported() && !Application.isEditor)
        {
            anchor = anchorObject.AddComponent<ARAnchor>();
            if (anchor != null)
            {
                spawnedAnchors.Add(anchor);
            }
        }
        
        // Instantiate treasure at anchor position
        GameObject treasure = Instantiate(treasurePrefab, position, Quaternion.identity);
        treasure.transform.SetParent(anchorObject.transform);
        
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

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
    [SerializeField] private float minHeight = 0.1f;
    [SerializeField] private float maxHeight = 0.5f;
    [SerializeField] private float planeDetectionTimeout = 3f;

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
        
        while ((arPlaneManager == null || arPlaneManager.trackables.count == 0) && elapsed < planeDetectionTimeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (elapsed >= planeDetectionTimeout)
        {
            useDirectPlacement = true;
        }

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
            }

            yield return null;
        }
    }

    private Vector3 GetRandomPositionAroundPlayer(float radius)
    {
        Vector3 playerPosition = arCamera.transform.position;
        
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(radius * 0.3f, radius);
        
        float x = playerPosition.x + distance * Mathf.Cos(angle);
        float z = playerPosition.z + distance * Mathf.Sin(angle);
        
        return new Vector3(x, playerPosition.y, z);
    }

    private bool TryPlaceAnchorAtPosition(Vector3 targetPosition)
    {
        if (useDirectPlacement)
        {
            return TryDirectPlacement(targetPosition);
        }
        
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        
        Vector3 rayOrigin = new Vector3(targetPosition.x, targetPosition.y + 5f, targetPosition.z);
        Vector3 rayDirection = Vector3.down;

        if (arRaycastManager != null && arRaycastManager.Raycast(new Ray(rayOrigin, rayDirection), hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            
            if (!GameManager.Instance.IsValidSpawnPosition(hitPose.position))
            {
                return false;
            }

            Vector3 spawnPosition = hitPose.position;
            spawnPosition.y += Random.Range(minHeight, maxHeight);

            return CreateAnchorAndTreasure(spawnPosition, hitPose.rotation);
        }

        return false;
    }

    private bool TryDirectPlacement(Vector3 targetPosition)
    {
        Vector3 groundPosition = new Vector3(targetPosition.x, 0f, targetPosition.z);
        
        if (!GameManager.Instance.IsValidSpawnPosition(groundPosition))
        {
            return false;
        }

        Vector3 spawnPosition = groundPosition;
        spawnPosition.y += Random.Range(minHeight, maxHeight);

        return CreateAnchorAndTreasure(spawnPosition, Quaternion.identity);
    }

    private bool CreateAnchorAndTreasure(Vector3 position, Quaternion rotation)
    {
        GameObject anchorObject = new GameObject("TreasureAnchor");
        anchorObject.transform.position = position;
        anchorObject.transform.rotation = rotation;
        
        if (!useDirectPlacement && arAnchorManager != null)
        {
            ARAnchor anchor = anchorObject.AddComponent<ARAnchor>();
            if (anchor != null)
            {
                spawnedAnchors.Add(anchor);
            }
        }
        
        GameObject treasure = Instantiate(treasurePrefab, anchorObject.transform);
        treasure.transform.localPosition = Vector3.zero;
        treasure.transform.localRotation = Quaternion.identity;
        
        SetupTreasureForInteraction(treasure);
        
        GameManager.Instance.RegisterSpawnPosition(position);
        
        return true;
    }

    private void SetupTreasureForInteraction(GameObject treasure)
    {
        Collider col = treasure.GetComponent<Collider>();
        if (col == null)
        {
            col = treasure.GetComponentInChildren<Collider>();
        }
        
        if (col == null)
        {
            SphereCollider sphereCol = treasure.AddComponent<SphereCollider>();
            sphereCol.radius = 0.5f;
        }
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureInputHandler : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera arCamera;
    [SerializeField] private float maxRaycastDistance = 20f;
    [SerializeField] private LayerMask treasureLayer;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugRays = true;
    [SerializeField] private bool logRaycastInfo = false;

    private void Start()
    {
        if (arCamera == null)
        {
            arCamera = Camera.main;
        }
    }

    void Update()
    {
        // Handle mouse input (for editor)
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }

        // Handle touch input (for mobile)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if touching UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return; // Don't process if touching UI
            }
            
            HandleClick(Input.GetTouch(0).position);
        }
    }

    private void HandleClick(Vector3 screenPosition)
    {
        if (arCamera == null)
        {
            Debug.LogWarning("AR Camera not assigned!");
            return;
        }

        // Check if clicking on UI (for mouse input)
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (logRaycastInfo)
        {
            Debug.Log($"Raycasting from {ray.origin} in direction {ray.direction}");
        }

        // Raycast to find treasures
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            if (logRaycastInfo)
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}");
            }

            TreasureCube treasure = hit.collider.GetComponent<TreasureCube>();
            if (treasure != null)
            {
                Debug.Log($"Clicked on treasure: {treasure.gameObject.name}");
                treasure.CollectTreasure();
            }
        }
        else
        {
            if (logRaycastInfo)
            {
                Debug.Log("Raycast hit nothing");
            }
        }

        // Draw debug ray
        if (showDebugRays)
        {
            Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance, Color.red, 1f);
        }
    }

    private void OnGUI()
    {
        if (showDebugRays && Application.isEditor)
        {
            GUI.Label(new Rect(10, 150, 300, 20), "Click/Tap treasures to collect");
            GUI.Label(new Rect(10, 170, 300, 20), $"Camera: {(arCamera != null ? "Found" : "Missing")}");
        }
    }
}

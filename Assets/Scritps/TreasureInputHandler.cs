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
            // Check if clicking on UI first
            if (IsPointerOverUIObject())
            {
                return; // Don't process if clicking UI
            }
            
            HandleClick(Input.mousePosition);
        }

        // Handle touch input (for mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                // Check if touching UI
                if (IsPointerOverUIObject(touch.fingerId))
                {
                    return; // Don't process if touching UI
                }
                
                HandleClick(touch.position);
            }
        }
    }

    private bool IsPointerOverUIObject(int fingerId = -1)
    {
        if (EventSystem.current == null)
            return false;

        // For touch input
        if (fingerId >= 0)
        {
            return EventSystem.current.IsPointerOverGameObject(fingerId);
        }
        
        // For mouse input
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void HandleClick(Vector3 screenPosition)
    {
        if (arCamera == null)
        {
            Debug.LogWarning("AR Camera not assigned!");
            return;
        }

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (showDebugRays)
        {
            Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance, Color.yellow, 1f);
        }

        // Raycast to find treasures - ignore layer mask if not set
        bool hitSomething = (treasureLayer.value == 0) 
            ? Physics.Raycast(ray, out hit, maxRaycastDistance)
            : Physics.Raycast(ray, out hit, maxRaycastDistance, treasureLayer);
        
        if (hitSomething)
        {
            if (showDebugRays)
            {
                Debug.Log($"Hit: {hit.collider.gameObject.name} at distance {hit.distance}");
            }
            
            TreasureCube treasure = hit.collider.GetComponent<TreasureCube>();
            if (treasure != null && !treasure.IsCollected)
            {
                Debug.Log($"Collecting treasure: {hit.collider.gameObject.name}");
                treasure.CollectTreasure();
            }
        }
    }
}

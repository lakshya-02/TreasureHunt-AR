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

        // Raycast to find treasures
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            TreasureCube treasure = hit.collider.GetComponent<TreasureCube>();
            if (treasure != null)
            {
                treasure.CollectTreasure();
            }
        }
    }
}

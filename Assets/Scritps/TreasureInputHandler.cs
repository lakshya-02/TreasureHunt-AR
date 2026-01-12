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
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject())
            {
                return;
            }
            
            HandleClick(Input.mousePosition);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                if (IsPointerOverUIObject(touch.fingerId))
                {
                    return;
                }
                
                HandleClick(touch.position);
            }
        }
    }

    private bool IsPointerOverUIObject(int fingerId = -1)
    {
        if (EventSystem.current == null)
            return false;

        if (fingerId >= 0)
        {
            return EventSystem.current.IsPointerOverGameObject(fingerId);
        }
        
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void HandleClick(Vector3 screenPosition)
    {
        if (arCamera == null)
        {
            return;
        }

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (showDebugRays)
        {
            Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance, Color.yellow, 1f);
        }

        bool hitSomething = (treasureLayer.value == 0) 
            ? Physics.Raycast(ray, out hit, maxRaycastDistance)
            : Physics.Raycast(ray, out hit, maxRaycastDistance, treasureLayer);
        
        if (hitSomething)
        {
            TreasureCube treasure = hit.collider.GetComponent<TreasureCube>();
            if (treasure != null && !treasure.IsCollected)
            {
                treasure.CollectTreasure();
            }
        }
    }
}

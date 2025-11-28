using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneVisualizer : MonoBehaviour
{
    [Header("AR Plane Manager")]
    [SerializeField] private ARPlaneManager arPlaneManager;

    [Header("Plane Visibility")]
    [SerializeField] private bool showPlanes = true;
    [SerializeField] private Material planeMaterial;

    void Start()
    {
        if (arPlaneManager == null)
        {
            arPlaneManager = GetComponent<ARPlaneManager>();
        }

        if (arPlaneManager != null)
        {
            arPlaneManager.planesChanged += OnPlanesChanged;
        }

        UpdatePlaneVisibility();
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            SetPlaneVisibility(plane, showPlanes);
        }

        foreach (var plane in args.updated)
        {
            SetPlaneVisibility(plane, showPlanes);
        }
    }

    private void SetPlaneVisibility(ARPlane plane, bool visible)
    {
        var meshRenderer = plane.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = visible;
            
            if (visible && planeMaterial != null)
            {
                meshRenderer.material = planeMaterial;
            }
        }

        var lineRenderer = plane.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.enabled = visible;
        }
    }

    public void TogglePlaneVisibility()
    {
        showPlanes = !showPlanes;
        UpdatePlaneVisibility();
    }

    public void UpdatePlaneVisibility()
    {
        if (arPlaneManager != null)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                SetPlaneVisibility(plane, showPlanes);
            }
        }
    }

    private void OnDestroy()
    {
        if (arPlaneManager != null)
        {
            arPlaneManager.planesChanged -= OnPlanesChanged;
        }
    }
}

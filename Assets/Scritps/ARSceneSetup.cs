using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class SimulatedPlaneData
{
    public Vector3 position = Vector3.zero;
    public Vector3 size = new Vector3(2f, 0.1f, 2f);
    public bool isVisible = true;
}

public class ARSceneSetup : MonoBehaviour
{
    [Header("Editor Simulation")]
    [SerializeField] private bool enableEditorSimulation = true;
    [SerializeField] private bool autoCreatePlanes = true;
    [SerializeField] private SimulatedPlaneData[] simulatedPlanes = {
        new SimulatedPlaneData { position = new Vector3(0, 0, 0), size = new Vector3(4f, 0.1f, 4f) },
        new SimulatedPlaneData { position = new Vector3(3, 0, 3), size = new Vector3(2f, 0.1f, 2f) },
        new SimulatedPlaneData { position = new Vector3(-3, 0, 3), size = new Vector3(2f, 0.1f, 2f) },
    };
    
    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject planePrefab;
    
    private bool isSimulationActive = false;

    void Start()
    {
        // Find AR components if not assigned
        if (arSession == null)
            arSession = FindObjectOfType<ARSession>();
        if (planeManager == null)
            planeManager = FindObjectOfType<ARPlaneManager>();

        if (Application.isEditor && enableEditorSimulation)
        {
            StartCoroutine(SetupEditorSimulation());
        }
    }

    private IEnumerator SetupEditorSimulation()
    {
        Debug.Log("Setting up Editor AR Simulation...");
        
        // Wait a moment for AR to initialize
        yield return new WaitForSeconds(1f);
        
        if (autoCreatePlanes)
        {
            CreateSimulatedPlanes();
        }
        
        isSimulationActive = true;
        Debug.Log("Editor AR Simulation ready!");
    }

    private void CreateSimulatedPlanes()
    {
        Debug.Log("Creating simulated AR planes...");
        
        for (int i = 0; i < simulatedPlanes.Length; i++)
        {
            var planeData = simulatedPlanes[i];
            if (!planeData.isVisible) continue;
            
            GameObject plane = CreateVisualPlane(planeData, i);
            Debug.Log($"Created simulated plane {i + 1} at {planeData.position}");
        }
    }

    private GameObject CreateVisualPlane(SimulatedPlaneData data, int index)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plane.name = $"SimulatedARPlane_{index}";
        plane.transform.position = data.position;
        plane.transform.localScale = data.size;
        
        // Make it look like an AR plane
        Renderer renderer = plane.GetComponent<Renderer>();
        Material mat = CreatePlaneMaterial();
        renderer.material = mat;
        
        // IMPORTANT: Disable collider so it doesn't block treasure clicks
        var collider = plane.GetComponent<Collider>();
        if (collider != null)
        {
            Destroy(collider); // Remove collider completely
        }
        
        return plane;
    }

    private Material CreatePlaneMaterial()
    {
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(0f, 1f, 0f, 0.2f); // Green with transparency
        mat.SetFloat("_Mode", 3); // Transparent mode
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        
        return mat;
    }

    public bool IsSimulationActive()
    {
        return isSimulationActive && Application.isEditor;
    }

    public Vector3[] GetSimulatedPlanePositions()
    {
        Vector3[] positions = new Vector3[simulatedPlanes.Length];
        for (int i = 0; i < simulatedPlanes.Length; i++)
        {
            positions[i] = simulatedPlanes[i].position;
        }
        return positions;
    }

    // Minimal on-screen info (only when simulation is active)
    void OnGUI()
    {
        if (!enableEditorSimulation || !Application.isEditor || !isSimulationActive) return;
        
        // Small notification that simulation is active
        GUI.Label(new Rect(10, Screen.height - 30, 200, 20), "AR Simulation Active");
    }
}
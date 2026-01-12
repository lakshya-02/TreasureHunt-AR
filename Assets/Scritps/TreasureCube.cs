using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCube : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float bobSpeed = 1f;
    [SerializeField] private float bobHeight = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Distance Settings")]
    [SerializeField] private float collectionDistance = 1.5f;
    [SerializeField] private float highlightDistance = 3f;

    private Camera arCamera;
    private Vector3 startPosition;
    private Vector3 localStartPosition;
    private bool isCollected = false;
    private Renderer treasureRenderer;
    private bool isHighlighted = false;

    // Public property to check if collected
    public bool IsCollected => isCollected;

    void Start()
    {
        arCamera = Camera.main;
        startPosition = transform.position;
        localStartPosition = transform.localPosition;
        treasureRenderer = GetComponent<Renderer>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the treasure has a collider for clicking
        EnsureCollider();
    }

    private void EnsureCollider()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            // Add a box collider if none exists
            BoxCollider boxCol = gameObject.AddComponent<BoxCollider>();
            Debug.Log($"Added BoxCollider to {gameObject.name}");
        }
        else
        {
            // Make sure the collider is enabled
            col.enabled = true;
        }
    }

    void Update()
    {
        if (isCollected) return;

        // Rotate the treasure
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Bob up and down using LOCAL position to maintain AR anchor
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = new Vector3(localStartPosition.x, localStartPosition.y + bobOffset, localStartPosition.z);

        // Check distance to player
        if (arCamera != null)
        {
            float distance = Vector3.Distance(transform.position, arCamera.transform.position);

            // Highlight when close
            if (distance <= highlightDistance && !isHighlighted)
            {
                HighlightTreasure(true);
            }
            else if (distance > highlightDistance && isHighlighted)
            {
                HighlightTreasure(false);
            }

            // Auto-collect when very close
            if (distance <= collectionDistance)
            {
                CollectTreasure();
            }
        }
    }

    private void HighlightTreasure(bool highlight)
    {
        isHighlighted = highlight;
        
        if (treasureRenderer != null && highlightMaterial != null && normalMaterial != null)
        {
            treasureRenderer.material = highlight ? highlightMaterial : normalMaterial;
        }

        // Scale effect
        float targetScale = highlight ? 1.2f : 1f;
        StartCoroutine(ScaleTreasure(targetScale));
    }

    private IEnumerator ScaleTreasure(float targetScale)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 targetScaleVector = Vector3.one * targetScale;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScaleVector, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScaleVector;
    }

    public void CollectTreasure()
    {
        if (isCollected) return;

        isCollected = true;

        // Play sound
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTreasureFound();
        }

        // Animate collection
        StartCoroutine(CollectAnimation());
    }

    private IEnumerator CollectAnimation()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = arCamera.transform.position;
        Vector3 startScale = transform.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    // Optional: Manual collection by tapping
    void OnMouseDown()
    {
        if (!isCollected)
        {
            CollectTreasure();
        }
    }
}

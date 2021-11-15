using System.Collections.Generic;
using UnityEngine;

public class DestructibleSurface : MonoBehaviour {
    private static readonly List<DestructibleSurface> allDestructibleSurfaces = new List<DestructibleSurface>();

    [SerializeField] private int numberOfHits = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;

Color startColor;
    [SerializeField] private Color endColor;

    private int currentNumberOfHits;
    private void Awake() {
        currentNumberOfHits = numberOfHits;
        
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        startColor = spriteRenderer.color;
    }

    private void OnDisable() {
        allDestructibleSurfaces.Add(this);
        currentNumberOfHits = numberOfHits;

    }

    public static void ResetAllDestructibleSurfaces() {
        foreach (var surface in allDestructibleSurfaces) {
            if (surface != null) {
                surface.gameObject.SetActive(true);
                surface.spriteRenderer.color = surface.startColor;
            }
        }
        allDestructibleSurfaces.Clear();
        
    }


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball")) {
            float alpha = (float)--currentNumberOfHits / (float)numberOfHits;
            Color color = Color.Lerp(endColor, startColor, alpha);
            spriteRenderer.color = color;
            if (currentNumberOfHits == 0) {
                gameObject.SetActive(false);
            }
        }
    }
}

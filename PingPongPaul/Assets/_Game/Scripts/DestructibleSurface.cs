using System.Collections.Generic;
using UnityEngine;

public class DestructibleSurface : MonoBehaviour {
    private static readonly List<DestructibleSurface> allDestructibleSurfaces = new List<DestructibleSurface>();

    [SerializeField] private int numberOfHits = 1;

    private int currentNumberOfHits;
    private void Awake() {
        currentNumberOfHits = numberOfHits;
    }

    private void OnDisable() {
        allDestructibleSurfaces.Add(this);
        currentNumberOfHits = numberOfHits;
    }

    public static void ResetAllDestructibleSurfaces() {
        foreach (var surface in allDestructibleSurfaces) {
            if (surface != null) {
                surface.gameObject.SetActive(true);
            }
        }
        allDestructibleSurfaces.Clear();
    }


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball")) {
            if (--currentNumberOfHits == 0)
                gameObject.SetActive(false);
        }
    }
}

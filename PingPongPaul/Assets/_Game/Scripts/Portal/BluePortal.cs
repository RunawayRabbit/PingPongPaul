using UnityEngine;

public class BluePortal : MonoBehaviour {

    private static BluePortal other;

    OrangePortal otherPortal;

    new BoxCollider2D collider;
    [SerializeField] private Vector2 normal;

    void Start() {
        if (other != null) {
            Destroy(other.gameObject);
        }
        other = this;
    }

    public void SetPortalNormal(Vector2 normal) {
        this.normal = normal;
    }

}

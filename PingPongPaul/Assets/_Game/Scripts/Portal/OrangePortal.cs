using UnityEngine;

public class OrangePortal : MonoBehaviour {

    private static OrangePortal other;

    BluePortal otherPortal;

    new BoxCollider2D collider;
    [SerializeField] Vector2 normal;

    // Start is called once per start, just so you know Euan!!!!
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
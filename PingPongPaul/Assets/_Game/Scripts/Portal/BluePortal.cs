using UnityEngine;

public class BluePortal : MonoBehaviour {

    public static BluePortal bluePortal;

    [SerializeField] private new BoxCollider2D collider;
    [SerializeField] private Vector2 normal;
    public static Vector2 HitDirection;
    public static Vector2 OutDirection;

    [SerializeField] private bool canTeleport;

    void Start() {
        if (bluePortal != null) {
            Destroy(bluePortal.gameObject);
        }
        bluePortal = this;

        canTeleport = true;
    }

    public void SetPortalNormal(Vector2 normal) {
        this.normal = normal;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        print("Hello from Blueportal");
        if (canTeleport == true && OrangePortal.orangePortal != null) {
            GameObject paul = other.gameObject;
            Rigidbody2D rigidbody = paul.GetComponent<Rigidbody2D>();

            Vector3 inPosition = this.transform.InverseTransformPoint(paul.transform.position);
            inPosition = -inPosition;
            Vector3 outPosition = OrangePortal.orangePortal.transform.TransformPoint(inPosition);

            Vector3 inDirection = this.transform.InverseTransformDirection(rigidbody.velocity);
            Vector3 outDirection = OrangePortal.orangePortal.transform.TransformDirection(inDirection);

            paul.transform.position = outPosition;
            rigidbody.velocity = -outDirection;

            OrangePortal.orangePortal.CanTeleport(false);
            canTeleport = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        canTeleport = true;
    }

    public void CanTeleport(bool newActive) {
        canTeleport = newActive;
    }

}

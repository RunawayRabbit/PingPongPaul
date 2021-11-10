using UnityEngine;

public class OrangePortal : MonoBehaviour {

    public static OrangePortal orangePortal;

    [SerializeField] private new BoxCollider2D collider;
    [SerializeField] private Vector2 normal;
    public static Vector2 HitDirection;
    public static Vector2 OutDirection;

    [SerializeField] private bool canTeleport;

    void Start() {
        if (orangePortal != null) {
            Destroy(orangePortal.gameObject);
        }
        orangePortal = this;

        canTeleport = true;
    }


    public void SetPortalNormal(Vector2 normal) {
        this.normal = normal;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        print("Hello from Orangeportal");

        if (canTeleport == true && BluePortal.bluePortal != null) {
            GameObject paul = other.gameObject;
            Rigidbody2D rigidbody = paul.GetComponent<Rigidbody2D>();

            Vector3 inPosition = transform.InverseTransformPoint(paul.transform.position);
            inPosition = -inPosition;
            Vector3 outPosition = BluePortal.bluePortal.transform.TransformPoint(inPosition);

            Vector3 inDirection = transform.InverseTransformDirection(rigidbody.velocity);
            Vector3 outDirection = BluePortal.bluePortal.transform.TransformDirection(inDirection);

            paul.transform.position = outPosition;
            rigidbody.velocity = -outDirection;

            BluePortal.bluePortal.CanTeleport(false);
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
using UnityEngine;

public abstract class PortalBase : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] protected bool canBeReset = true;

    [Header("Debug")]
    [SerializeField] protected bool canTeleport;

    protected void CalculateExitPosition(PortalBase otherPortal, GameObject paul) {
        Rigidbody2D rigidbody = paul.GetComponent<Rigidbody2D>();

        Vector3 inPosition = this.transform.InverseTransformPoint(paul.transform.position);
        inPosition = -inPosition;
        Vector3 outPosition = otherPortal.transform.TransformPoint(inPosition);

        Vector3 inDirection = this.transform.InverseTransformDirection(rigidbody.velocity);
        Vector3 outDirection = otherPortal.transform.TransformDirection(inDirection);

        paul.transform.position = outPosition;
        rigidbody.velocity = -outDirection;

        otherPortal.CanTeleport(false);
        canTeleport = false;
    }

    protected void CanTeleport(bool newActive) {
        canTeleport = newActive;
    }

    public void ResetPortal() {
        if (canBeReset == true) {
            OnReset();
        }
    }

    protected abstract void OnReset();

    public abstract void ApplySettings(PortalSettings settings);

}

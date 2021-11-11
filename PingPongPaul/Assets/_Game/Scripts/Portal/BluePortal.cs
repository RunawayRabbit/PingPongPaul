using UnityEngine;

public class BluePortal : PortalBase {

    public static BluePortal bluePortal;

    void Start() {
        if (bluePortal != null) {
            Destroy(bluePortal.gameObject);
        }
        bluePortal = this;
        canTeleport = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        OrangePortal orangePortal = OrangePortal.orangePortal;
        if (canTeleport == true && orangePortal != null) {
            CalculateExitPosition(orangePortal, other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        canTeleport = true;
    }

    protected override void OnReset() {
        Destroy(bluePortal.gameObject);
    }

    public override void ApplySettings(PortalSettings settings) {
        canBeReset = settings.CanBluePortalReset;
    }

}

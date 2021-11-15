using UnityEngine;

public class BluePortal : PortalBase {

    public static BluePortal bluePortal;

    protected override void OnConfirmPortalPlacement() {
        if (bluePortal != null) {
            Destroy(bluePortal.gameObject);
        }
        bluePortal = this;

        canTeleport = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        OrangePortal orangePortal = OrangePortal.orangePortal;
        if (canTeleport == true && orangePortal != null && orangePortal.CanTeleport() == true) {
            CalculateExitPosition(orangePortal, other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (isGhost == false) {
            canTeleport = true;
        }
    }

    protected override void OnReset() {
        Destroy(bluePortal.gameObject);
    }

    public override void ApplySettings(PortalSettings settings) {
        canBeReset = settings.CanBluePortalReset;
    }

}

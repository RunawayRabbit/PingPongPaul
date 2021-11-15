using UnityEngine;

public class OrangePortal : PortalBase {

    public static OrangePortal orangePortal;

    protected override void OnConfirmPortalPlacement() {
        if (orangePortal != null) {
            Destroy(orangePortal.gameObject);
        }
        orangePortal = this;

        canTeleport = true;

        print("Hello?");
    }


    private void OnTriggerEnter2D(Collider2D other) {
        BluePortal bluePortal = BluePortal.bluePortal;
        if (canTeleport == true && bluePortal != null && bluePortal.CanTeleport() == true) {
            CalculateExitPosition(bluePortal, other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (isGhost == false) {
            canTeleport = true;
        }
    }

    protected override void OnReset() {
        Destroy(orangePortal.gameObject);
    }

    public override void ApplySettings(PortalSettings settings) {
        canBeReset = settings.CanOrangePortalReset;
    }
}
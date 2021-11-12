using UnityEngine;

public class OrangePortal : PortalBase {

    public static OrangePortal orangePortal;


    void Start() {
        if (orangePortal != null) {
            Destroy(orangePortal.gameObject);
        }
        orangePortal = this;

        canTeleport = true;

        StartCoroutine(CheckPositionDelayed());
    }


    private void OnTriggerEnter2D(Collider2D other) {
        BluePortal bluePortal = BluePortal.bluePortal;
        if (canTeleport == true && bluePortal != null) {
            CalculateExitPosition(bluePortal, other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        canTeleport = true;
    }

    protected override void OnReset() {
        Destroy(orangePortal.gameObject);
    }

    public override void ApplySettings(PortalSettings settings) {
        canBeReset = settings.CanOrangePortalReset;
    }
}
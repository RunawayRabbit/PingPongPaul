using UnityEngine;

public class PlayerDeathBox : MonoBehaviour {

    private int ballLayer = 10;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == ballLayer) {
            PC_UIController.pc_uiController.ShowLoseScreen();
            PC_UIController.pc_uiController.SetCurrentShots(0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == ballLayer) {
            PC_UIController.pc_uiController.ShowLoseScreen();
            PC_VelocityProgress.pc_velocityProgress.SetShots(0);
        }
    }
}

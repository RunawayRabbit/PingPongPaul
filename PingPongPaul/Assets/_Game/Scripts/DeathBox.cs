using UnityEngine;

public class DeathBox : MonoBehaviour {

    [SerializeField] private int paulLayer = 7;
    [SerializeField] private int ballLayer = 10;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == paulLayer) {
            PC_UIController.pc_uiController.ShowWinScreen();
        }

        if (collision.gameObject.layer == ballLayer) {
            
        }
    }
}

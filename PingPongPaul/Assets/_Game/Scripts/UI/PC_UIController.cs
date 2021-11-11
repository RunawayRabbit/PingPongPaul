using UnityEngine;
using UnityEngine.SceneManagement;

public class PC_UIController : MonoBehaviour {

    private PC_Reset pc_reset;
    private PC_PortalGun pc_portalGun;

    public void ResetGame() {
        if (pc_reset == null) {
            pc_reset = FindObjectOfType<PC_Reset>();
        }

        pc_reset.ResetGame();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.N) == true) {
            var scene = SceneManager.GetActiveScene();
            int sceneIndex = scene.buildIndex;
            SceneManager.LoadScene(++sceneIndex);
        }
    }

    public void ShootBluePortal() {
        pc_portalGun.ShootBluePortal();
    }

    public void ShootOrangePortal() {
        pc_portalGun.ShootOrangePortal();
    }

}

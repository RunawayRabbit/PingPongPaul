using UnityEngine;
using UnityEngine.SceneManagement;

public class PC_UIController : MonoBehaviour {

    private PC_Reset pc_reset;
    [SerializeField] private PC_PortalGun pc_portalGun;
    [SerializeField] private PC_CameraController pc_cameraController;

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

        if (Input.GetKeyDown(KeyCode.H) == true) {
            pc_cameraController.transform.position = Ball.ball.transform.position;
        }
    }



    public void ShootBluePortal() {
        pc_portalGun.ShootBluePortal();
    }

    public void ShootOrangePortal() {
        pc_portalGun.ShootOrangePortal();
    }

}

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PC_UIController : MonoBehaviour {

    public static PC_UIController pc_uiController;

    private PC_Reset pc_reset;
    [SerializeField] private TextMeshProUGUI shotsLeftText;
    [SerializeField] private TextMeshProUGUI maxShotsText;
    [SerializeField] private PC_CameraController pc_cameraController;

    private void Start() {
        pc_uiController = this;
    }

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

    public void SetMaxShots(int maxShots) {
        maxShotsText.text = maxShots.ToString();
    }

    public void SetCurrentShots(int currentShots) {
        shotsLeftText.text = currentShots.ToString();
    }

}

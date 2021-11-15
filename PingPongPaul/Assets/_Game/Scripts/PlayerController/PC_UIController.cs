using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PC_UIController : MonoBehaviour {

    public static PC_UIController pc_uiController;

    private PC_Reset pc_reset;
    [SerializeField] private TextMeshProUGUI shotsLeftText;
    [SerializeField] private TextMeshProUGUI maxShotsText;
    [SerializeField] private PC_CameraController pc_cameraController;
    [SerializeField] private GameObject winScreen;

    private void Start() {
        pc_uiController = this;
        HideWinScreen();
    }

    public void ResetGame() {
        if (pc_reset == null) {
            pc_reset = FindObjectOfType<PC_Reset>();
        }

        pc_reset.ResetGame();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.N) == true) {
            NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.B) == true) {
            PreviousLevel();
        }

        if (Input.GetKeyDown(KeyCode.H) == true) {
            ResetCameraPosition();
        }
    }

    private void ResetCameraPosition() {
        pc_cameraController.transform.position = Ball.ball.transform.position;
    }

    public void NextLevel() {
        var scene = SceneManager.GetActiveScene();
        int sceneIndex = scene.buildIndex;
        SceneManager.LoadScene(++sceneIndex);
    }

    private void PreviousLevel() {
        var scene = SceneManager.GetActiveScene();
        int sceneIndex = scene.buildIndex;
        if (sceneIndex > 0) {
            SceneManager.LoadScene(--sceneIndex);
        }
    }

    public void SetMaxShots(int maxShots) {
        maxShotsText.text = maxShots.ToString();
    }

    public void SetCurrentShots(int currentShots) {
        shotsLeftText.text = currentShots.ToString();
    }

    public void ShowWinScreen() {
        winScreen.SetActive(true);
    }

    public void HideWinScreen() {
        winScreen.SetActive(false);
    }

}

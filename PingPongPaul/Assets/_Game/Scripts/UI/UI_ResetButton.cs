using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ResetButton : MonoBehaviour {

    private PC_Reset pc_reset;

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

}

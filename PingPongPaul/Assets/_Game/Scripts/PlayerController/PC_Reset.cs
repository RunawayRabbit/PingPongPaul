using UnityEngine;

public class PC_Reset : MonoBehaviour {
    [SerializeField] private Ball ball;
    private Vector3 startPosition;

    void Start() {
        startPosition = ball.transform.position;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R) == true) {
            ResetGame();
        }
    }

    public void ResetGame() {
        ball.transform.position = startPosition;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        if (BluePortal.bluePortal != null) {
            BluePortal.bluePortal.ResetPortal();
        }
        if (OrangePortal.orangePortal != null) {
            OrangePortal.orangePortal.ResetPortal();
        }
        DestructibleSurface.ResetAllDestructibleSurfaces();
        Paul.ResetAllPauls();
        Resettable.ResetAll();
        LevelInstance.levelInstance.ResetValues();
        PC_VelocityProgress.pc_velocityProgress.ResetShots();
        PC_UIController.pc_uiController.HideWinScreen();
    }

}

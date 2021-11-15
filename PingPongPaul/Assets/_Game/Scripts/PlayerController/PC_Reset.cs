using UnityEngine;
using MC_Utility;

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
        EventSystem<ResetEvent>.FireEvent(new ResetEvent());

        DestructibleSurface.ResetAllDestructibleSurfaces();
        Paul.ResetAllPauls();
        Resettable.ResetAll();
    }

}

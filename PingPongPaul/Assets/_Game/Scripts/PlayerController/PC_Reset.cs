using UnityEngine;

public class PC_Reset : MonoBehaviour {
    [SerializeField] private Ball ball;
    private Vector3 startPosition;

    void Start() {
        startPosition = ball.transform.position;
    }

    // Update is called once per frame. This means it will check it once every frame.
    void Update() {
        if (Input.GetKeyDown(KeyCode.R) == true) {
            ResetGame();
        }
    }

    public void ResetGame() {
        ball.transform.position = startPosition;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        DestructibleSurface.ResetAllDestructibleSurfaces();
        Paul.ResetAllPauls();
        Resettable.ResetAll();
    }
}

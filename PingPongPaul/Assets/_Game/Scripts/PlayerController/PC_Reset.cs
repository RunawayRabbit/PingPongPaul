using UnityEngine;

public class PC_Reset : MonoBehaviour {
    [SerializeField] private Ball ball;
    private Vector3 startPosition;

    void Start() {
        startPosition = ball.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R) == true) {
            ball.transform.position = startPosition;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ball.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        }
    }
}

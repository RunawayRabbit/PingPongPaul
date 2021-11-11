using UnityEngine;

public class PC_CameraController : MonoBehaviour {
    [Header("References")]
    [SerializeField] new private Transform transform;
    [SerializeField] new private Camera camera;

    [Header("Settings")]
    [SerializeField] private float maxAcceleration = 150f;
    [SerializeField] private float frictionCoefficient = 6.0f;

    [SerializeField] private float zoomStep = 6.0f;
    [SerializeField] private float minZoomValue = 3.0f;
    [SerializeField] private float maxZoomValue = 7.0f;
    [SerializeField] private float zoomSpeed = 4.0f;

    [SerializeField] private float cameraDistance = 5.5f;

    [Header("Debug")]
    [SerializeField] private Vector2 moveVelocity;

    void Update() {
        float deltaTime = Time.unscaledDeltaTime;
        UpdateCameraDistance(deltaTime);
        UpdateCameraMovement(deltaTime);
    }

    private void UpdateCameraDistance(float deltaTime) {
        float zoom = -Input.GetAxis("Mouse ScrollWheel");
        cameraDistance += zoom * zoomStep;
        cameraDistance = Mathf.Clamp(cameraDistance, minZoomValue, maxZoomValue);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraDistance, zoomSpeed * deltaTime);
    }

    private void UpdateCameraMovement(float deltaTime) {
        float upInput = Input.GetAxisRaw("Vertical");
        float rightInput = Input.GetAxisRaw("Horizontal");

        Vector2 upAcceleration = transform.up * upInput;
        Vector2 rightAcceleration = transform.right * rightInput;

        Vector2 moveAcceleration = (upAcceleration + rightAcceleration).normalized * maxAcceleration;
        Vector2 moveFriction = -moveVelocity * frictionCoefficient;

        moveVelocity += (moveAcceleration + moveFriction) * deltaTime;

        Vector2 position = transform.position;
        position += moveVelocity * deltaTime;

        transform.position = position;
    }

    public void ApplySettings(CameraSettings cameraSettings) {
        minZoomValue = cameraSettings.minZoomValue;
        maxZoomValue = cameraSettings.maxZoomValue;
        cameraDistance = Mathf.Clamp(cameraSettings.startZoomValue, minZoomValue, maxZoomValue);
        camera.orthographicSize = cameraDistance;
    }

    private void OnValidate() {
        cameraDistance = Mathf.Clamp(cameraDistance, minZoomValue, maxZoomValue);
        camera.orthographicSize = cameraDistance;
    }
}

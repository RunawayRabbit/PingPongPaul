using UnityEngine;

public class Receiver_AnimationComponent : SignalReceiverComponent {

    private Vector3 startPosition;
    [Header("Settings")]
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float animationTime = 0.5f;

    [Header("Debug")]
    [SerializeField] private float currentTime;
    [SerializeField] private bool isForwardAnimation = false;
    [SerializeField] private bool isAnimating;
    
    private void Start() {
        startPosition = transform.position;
    }

    void Update() {
        if (isAnimating == true) {
            if (isForwardAnimation == true) {
                AnimateForward();
            }
            else {
                AnimateBackwards();
            }
        }
    }

    public override void Interact() {
        StartAnimation();
    }

    public void StartAnimation() {
        isAnimating = true;
        isForwardAnimation = !isForwardAnimation;
    }

    private void AnimateForward() {
        currentTime += Time.deltaTime;
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, currentTime / animationTime);
        gameObject.transform.position = newPosition;
        if (currentTime > animationTime) {
            isAnimating = false;
            currentTime = animationTime;
        }
    }

    private void AnimateBackwards() {
        currentTime -= Time.deltaTime;
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, currentTime / animationTime);
        gameObject.transform.position = newPosition;
        if (currentTime <= 0.0f) {
            isAnimating = false;
            currentTime = 0.0f;
        }
    }

}

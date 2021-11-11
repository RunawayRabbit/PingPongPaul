using UnityEngine;

public class Receiver_ChangeGravityComponent : SignalReceiverComponent {

    private Vector2 oldGravity;
    [Header("Settings")]
    [SerializeField] private float newGravity;

    private bool isActive;

    private void Start() {
        oldGravity = Physics2D.gravity;
    }

    public override void Interact() {
        ChangeGravity();
    }

    public void ChangeGravity() {
        if (isActive == false) {
            Physics2D.gravity = new Vector2(0.0f, newGravity);
            isActive = true;
        } else {
            Physics2D.gravity = oldGravity;
            isActive = false;
        }
    }

}

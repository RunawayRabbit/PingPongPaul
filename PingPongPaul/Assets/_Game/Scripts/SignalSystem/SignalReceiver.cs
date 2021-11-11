using UnityEngine;

public class SignalReceiver : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SignalReceiverComponent[] receiverComponents;

    [Header("Debug")]
    [SerializeField] private bool isActive;
    [SerializeField] private bool forceSignal;

    public void ReceiveSignal() {
        isActive = !isActive;

        if (receiverComponents != null) {
            foreach (SignalReceiverComponent src in receiverComponents) {
                src.Interact();
            }
        }
    }

    private void OnValidate() {
        if (forceSignal == true) {
            ReceiveSignal();
            forceSignal = false;
        }
    }

}

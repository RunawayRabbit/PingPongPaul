using UnityEngine;

public class SignalSender : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private SignalReceiver[] receivers;
    [SerializeField] private bool onlyActivateOnce = false;

    [Header("Debug")]
    [SerializeField] private bool forceSendSignal;
    [SerializeField] private bool canActivate;
    [SerializeField] private float timer;
    [SerializeField] private float time = 0.2f;

    void Update() {
        if (canActivate == false) {
            timer -= Time.deltaTime;
            if (timer <= 0.0f) {
                timer = time;
                canActivate = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (canActivate == true) {

            foreach (SignalReceiver signalReceiver in receivers) {
                signalReceiver.ReceiveSignal();
            }

            canActivate = false;

            if (onlyActivateOnce == true) {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnValidate() {
        if (forceSendSignal == true) {
            foreach (SignalReceiver signalReceiver in receivers) {
                signalReceiver.ReceiveSignal();
            }
            forceSendSignal = false;
        }
    }
}

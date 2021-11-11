
using UnityEngine;

public class SignalSender : MonoBehaviour {

    [SerializeField] private SignalReceiver[] receivers;

    [SerializeField] private bool canActivate;
    [SerializeField] private float timer;
    [SerializeField] private float time = 0.2f;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
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
        }
    }
}

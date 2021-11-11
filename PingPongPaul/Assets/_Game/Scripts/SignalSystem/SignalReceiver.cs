
using UnityEngine;

public class SignalReceiver : MonoBehaviour
{

    [SerializeField] private bool isActive;

    [SerializeField] private ReceiverAnimation animation;

    [SerializeField] private bool testAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveSignal() {
        isActive = !isActive;

        if (animation != null) {
            animation.StartAnimation();
        }
    }

    private void OnValidate() {
        if (testAnimation == true) {
            ReceiveSignal();
            testAnimation = false;
        }
    }

}

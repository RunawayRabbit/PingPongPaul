using UnityEngine;
using UnityEngine.UI;

public class PC_VelocityProgress : MonoBehaviour {

    [SerializeField] private GameObject UI_VelocityProgress;

    private GameObject instanced_ui_velocityProgress;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Mouse0) == true) {
            if (instanced_ui_velocityProgress == null) {
                instanced_ui_velocityProgress = GameObject.Instantiate(UI_VelocityProgress);
            }

        }
    }
}

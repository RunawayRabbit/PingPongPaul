using MC_Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class PC_VelocityProgress : MonoBehaviour {
    public static PC_VelocityProgress pc_velocityProgress;

    [SerializeField] private GameObject UI_VelocityProgress;

    private GameObject instanced_ui_velocityProgress;

    [SerializeField] private int shotsLeft;
    [SerializeField] private int maxShots;

    private void Start() {
        ApplySettings();
    }

    private void OnEnable() {
        pc_velocityProgress = this;
        EventSystem<ResetEvent>.RegisterListener(ResetShots);
    }

    private void OnDisable() {
        EventSystem<ResetEvent>.UnregisterListener(ResetShots);
    }

    void Update() {

        if (shotsLeft > 0 && Input.GetKeyDown(KeyCode.Mouse0) == true && EventSystem.current.IsPointerOverGameObject() == false) {
            if (instanced_ui_velocityProgress == null) {
                instanced_ui_velocityProgress = GameObject.Instantiate(UI_VelocityProgress);
                instanced_ui_velocityProgress.GetComponentInChildren<UI_VelocityProgress>().SetPosition(transform);
            }
        }
    }

    public void Shoot() {
        if (maxShots != -1) {
            shotsLeft--;
            PC_UIController.pc_uiController.SetCurrentShots(shotsLeft);
        }
    }

    public void ResetShots(ResetEvent resetEvent) {
        shotsLeft = maxShots;
        PC_UIController.pc_uiController.SetMaxShots(maxShots);
        PC_UIController.pc_uiController.SetCurrentShots(shotsLeft);
        if (maxShots == -1) {
            shotsLeft = 9999;
            PC_UIController.pc_uiController.SetCurrentShots(shotsLeft);
            PC_UIController.pc_uiController.SetMaxShots(9999);
        }
    }

    public void ApplySettings() {
        BallSettings settings = LevelInstance.levelInstance.GetBallSettings();

        maxShots = settings.maxShots;
        shotsLeft = maxShots;
        PC_UIController.pc_uiController.SetMaxShots(maxShots);
        PC_UIController.pc_uiController.SetCurrentShots(shotsLeft);

        if (maxShots == -1) {
            shotsLeft = 9999;
            PC_UIController.pc_uiController.SetCurrentShots(shotsLeft);
            PC_UIController.pc_uiController.SetMaxShots(9999);
        }
    }

}

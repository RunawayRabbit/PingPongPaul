using UnityEngine;

public class LevelInstance : MonoBehaviour {

    public static LevelInstance levelInstance;

    [SerializeField] private SO_LevelDataSettings levelData;

    [Header("References")]
    [SerializeField] private PC_CameraController cameraController;
    [SerializeField] private Ball ball;
    [SerializeField] private PC_PortalGun portalGun;
    [SerializeField] private BluePortal bluePortal;
    [SerializeField] private OrangePortal orangePortal;

    [Header("Physics Settings")]
    [SerializeField] private float gravity = -9.81f;

    [Header("Ball Settings")]
    [SerializeField] private bool cancelVelocityOnShoot = true;

    [Header("Portal Settings")]
    [SerializeField] private bool canShootBluePortal = true;
    [SerializeField] private int numberOfBluePortalsAllowed = -1;
    [SerializeField] private bool canShootOrangePortal = true;
    [SerializeField] private int numberOfOrangePortalsAllowed = -1;

    [SerializeField] private PortalVisualizationStyle portalVisualization;

    [Space]
    [SerializeField] private bool canBluePortalReset = true;
    [SerializeField] private bool canOrangePortalReset = true;

    [Header("Win Condition Settings")]
    [SerializeField] private int maxShots = 3;
    [Space]
    [SerializeField] private int threeStar = 1;
    [SerializeField] private int twoStar = 2;
    [SerializeField] private int oneStar = 3;

    [Header("Camera Settings")]
    [SerializeField] private float startZoomValue = 5.5f;
    [SerializeField] private float minZoomValue = 3.0f;
    [SerializeField] private float maxZoomValue = 7.0f;

    [ExecuteInEditMode]
    public void ChangeSettingsRealTime() {
        print("IsValidating");
    }

    private void Start() {
        levelInstance = this;

        ApplySettings();
    }

    public void ResetValues() {
        ApplySettings();
    }

    private void ApplySettings() {
        cameraController.ApplySettings(GetCameraSettings());
        ball.ApplySettings(GetBallSettings());
        ApplyPortalSettings();
        PC_VelocityProgress.pc_velocityProgress.ApplySettings(GetBallSettings());
        Physics2D.gravity = new Vector2(0.0f, gravity);

    }

    private void OnValidate() {
        cameraController.ApplySettings(GetCameraSettings());
        //ApplyPortalSettings();
    }

    private void ApplyPortalSettings() {
        PortalSettings portalSettings = GetPortalSettings();
        bluePortal = FindObjectOfType<BluePortal>();
        if (bluePortal != null) {
            bluePortal.ApplySettings(portalSettings);
            bluePortal.ConfirmPortalPlacement();
        }

        orangePortal = FindObjectOfType<OrangePortal>();
        if (orangePortal != null) {
            orangePortal.ApplySettings(portalSettings);
            orangePortal.ConfirmPortalPlacement();
        }
        portalGun.ApplySettings(portalSettings);
    }

    private CameraSettings GetCameraSettings() {
        CameraSettings cameraSettings;
        cameraSettings.startZoomValue = startZoomValue;
        cameraSettings.minZoomValue = minZoomValue;
        cameraSettings.maxZoomValue = maxZoomValue;

        return cameraSettings;
    }

    private BallSettings GetBallSettings() {
        BallSettings settings;
        settings.MaxForce            = levelData.MaxForce;
        settings.PaulStickiness      = levelData.PaulStickiness;
        settings.MaxPaulDistance     = levelData.MaxPaulDistance;
        settings.ballMass            = levelData.ballMass;
        settings.linearDrag          = levelData.linearDrag;
        settings.angularDrag         = levelData.angularDrag;
        settings.stopVelocityOnShoot = cancelVelocityOnShoot;
        settings.maxShots            = maxShots;

        return settings;
    }

    private PortalSettings GetPortalSettings() {
        PortalSettings settings;
        settings.CanBluePortalReset           = canBluePortalReset;
        settings.CanShootBluePortal           = canShootBluePortal;
        settings.CanOrangePortalReset         = canOrangePortalReset;
        settings.CanShootOrangePortal         = canShootOrangePortal;
        settings.NumberOfBluePortalsAllowed   = numberOfBluePortalsAllowed;
        settings.NumberOfOrangePortalsAllowed = numberOfOrangePortalsAllowed;
        settings.visualizationStyle           = portalVisualization;
        return settings;
    }
}

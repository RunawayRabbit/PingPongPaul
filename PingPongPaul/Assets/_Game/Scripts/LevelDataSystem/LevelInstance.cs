using MC_Utility;
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

    private void OnEnable() {
        levelInstance = this;
        EventSystem<ResetEvent>.RegisterListener(ResetValues);
    }

    private void Start() {
        ApplySettings();
    }


    private void OnDisable() {
        EventSystem<ResetEvent>.UnregisterListener(ResetValues);
    }

    public void ResetValues(ResetEvent resetEvent) {
        ApplySettings();
    }

    private void ApplySettings() {
        Physics2D.gravity = new Vector2(0.0f, gravity);

        BluePortal.bluePortal     = bluePortal;
        OrangePortal.orangePortal = orangePortal;
    }


    public CameraSettings GetCameraSettings() {
        CameraSettings cameraSettings;
        cameraSettings.startZoomValue = startZoomValue;
        cameraSettings.minZoomValue = minZoomValue;
        cameraSettings.maxZoomValue = maxZoomValue;

        return cameraSettings;
    }

    public BallSettings GetBallSettings() {
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

    public PortalSettings GetPortalSettings() {
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

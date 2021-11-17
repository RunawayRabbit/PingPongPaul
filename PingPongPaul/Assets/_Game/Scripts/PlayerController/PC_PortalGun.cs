using UnityEngine;


public enum PortalVisualizationStyle {
    Nothing,
    DrawALine,
}

public class PC_PortalGun : MonoBehaviour {
    public static PC_PortalGun pc_portalGun;

    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask wallLayermask;
    [SerializeField] private string unportalableTag;

    [Header("Settings")]
    [SerializeField] private GameObject bluePortal;
    [SerializeField] private bool canShootBluePortal = true;
    [SerializeField] private int numberOfBlueShots = -1;

    [SerializeField] private GameObject orangePortal;
    [SerializeField] private bool canShootOrangePortal = true;
    [SerializeField] private int numberOfOrangeShots = -1;


    [SerializeField] private PortalVisualizationStyle VisualizationStyle;
    [Header("Debug")]
    [SerializeField] private PortalBase visualizedPortal;

    [SerializeField] private float portalGunDistance = 100f;

    private Transform portalVisualizer;

    private float angle;
    private RaycastHit2D raycast;

    private void Start() {
        pc_portalGun = this;

        if (lineRenderer == null) {
            lineRenderer = GetComponent<LineRenderer>();
        }

        ApplySettings();
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha1) && canShootBluePortal) {
            CancelPortalPlacement();
            InstantiateVisualizer(bluePortal);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && canShootOrangePortal) {
            CancelPortalPlacement();
            InstantiateVisualizer(orangePortal);
        }

        if (VisualizationStyle == PortalVisualizationStyle.DrawALine) {

            Vector2 position = transform.position;
            Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            raycast = Physics2D.Raycast(transform.position, direction, portalGunDistance, wallLayermask);

            if (raycast == true) {

                bool canShoot = !raycast.collider.CompareTag(unportalableTag);

                lineRenderer.enabled = true;
                Vector3[] lineToRender = { position, raycast.point };
                lineRenderer.SetPositions(lineToRender);

                angle = Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg;

                if (canShoot) {
                    visualizedPortal.Opacity = 0.7f;
                    VisualizePortal(new Vector3(raycast.point.x, raycast.point.y));
                }
                else {
                    visualizedPortal.Opacity = 0.0f;
                    VisualizePortal(new Vector3(999999f, raycast.point.y));
                }

                if (Input.GetKeyUp(KeyCode.Alpha1) == true
                    && canShootBluePortal == true) {

                    if (canShoot) {

                        if (numberOfBlueShots != 0) {
                            ShootPortal(bluePortal);
                            numberOfBlueShots--;
                        }
                    }
                    else {
                        CancelPortalPlacement();
                    }
                }

                if (Input.GetKeyUp(KeyCode.Alpha2) == true
                    && canShootOrangePortal == true) {
                    if (canShoot) {
                        if (numberOfOrangeShots != 0) {
                            ShootPortal(orangePortal);
                            numberOfOrangeShots--;
                        }
                    }
                    else {
                        CancelPortalPlacement();
                    }
                }
                //}
            }
        }

        else {
            lineRenderer.enabled = false;
        }

        if (VisualizationStyle == PortalVisualizationStyle.DrawALine && Input.GetKeyUp(KeyCode.Mouse1) == true) {
            CancelPortalPlacement();
        }

    }

    private void CancelPortalPlacement() {
        if (visualizedPortal != null) {
            visualizedPortal.CancelPortalPlacement();
            visualizedPortal = null;
            VisualizationStyle = PortalVisualizationStyle.Nothing;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    private void InstantiateVisualizer(GameObject thingy) {
        portalVisualizer = Instantiate(thingy).transform;
        visualizedPortal = portalVisualizer.GetComponent<PortalBase>();
        visualizedPortal.SetOriginalBounds();
        visualizedPortal.Opacity = 0.7f;

        VisualizationStyle = PortalVisualizationStyle.DrawALine;

        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void VisualizePortal(Vector3 raycastPoint) {
        portalVisualizer.position = raycastPoint + visualizedPortal.CheckPosition(raycast.point);
        portalVisualizer.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ShootPortal(GameObject prefab) {
        visualizedPortal.Opacity = 1.0f;
        visualizedPortal.ConfirmPortalPlacement();

        VisualizationStyle = PortalVisualizationStyle.Nothing;
        visualizedPortal = null;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void ApplySettings() {
        PortalSettings settings = LevelInstance.levelInstance.GetPortalSettings();

        canShootBluePortal = settings.CanShootBluePortal;
        canShootOrangePortal = settings.CanShootOrangePortal;
        numberOfBlueShots = settings.NumberOfBluePortalsAllowed;
        numberOfOrangeShots = settings.NumberOfOrangePortalsAllowed;
        VisualizationStyle = settings.visualizationStyle;

        BluePortal bluePortal = FindObjectOfType<BluePortal>();
        if (bluePortal != null) {
            bluePortal.ApplySettings(settings);
            bluePortal.ConfirmPortalPlacement();
        }

        OrangePortal orangePortal = FindObjectOfType<OrangePortal>();
        if (orangePortal != null) {
            orangePortal.ApplySettings(settings);
            orangePortal.ConfirmPortalPlacement();
        }
    }
}

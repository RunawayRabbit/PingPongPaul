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
    [SerializeField] private LayerMask layermaskPortal;

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
    private bool canShoot;

    private void Start() {
        pc_portalGun = this;

        if (lineRenderer == null) {
            lineRenderer = GetComponent<LineRenderer>();
        }
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

                lineRenderer.enabled = true;
                Vector3[] lineToRender = { position, raycast.point };
                lineRenderer.SetPositions(lineToRender);

                angle = Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg;

                VisualizePortal();

                if (Input.GetKeyUp(KeyCode.Alpha1) == true
                    && canShootBluePortal == true) {
                    if (numberOfBlueShots != 0) {
                        ShootPortal(bluePortal);
                        numberOfBlueShots--;
                    }
                }

                if (Input.GetKeyUp(KeyCode.Alpha2) == true
                    && canShootOrangePortal == true) {
                    if (numberOfOrangeShots != 0) {
                        ShootPortal(orangePortal);
                        numberOfOrangeShots--;
                    }
                }

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
        }
    }

    private void InstantiateVisualizer(GameObject thingy) {
        portalVisualizer = Instantiate(thingy).transform;
        visualizedPortal = portalVisualizer.GetComponent<PortalBase>();
        visualizedPortal.SetOriginalBounds();
        visualizedPortal.Opacity = 0.7f;

        VisualizationStyle = PortalVisualizationStyle.DrawALine;
    }

    private void VisualizePortal() {
        portalVisualizer.position = new Vector3(raycast.point.x, raycast.point.y) + visualizedPortal.CheckPosition(raycast.point);
        portalVisualizer.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ShootPortal(GameObject prefab) {
        visualizedPortal.Opacity = 1.0f;
        visualizedPortal.ConfirmPortalPlacement();

        VisualizationStyle = PortalVisualizationStyle.Nothing;
        visualizedPortal = null;
    }

    public void ApplySettings(PortalSettings settings) {
        canShootBluePortal = settings.CanShootBluePortal;
        canShootOrangePortal = settings.CanShootOrangePortal;
        numberOfBlueShots = settings.NumberOfBluePortalsAllowed;
        numberOfOrangeShots = settings.NumberOfOrangePortalsAllowed;
        VisualizationStyle = settings.visualizationStyle;
    }
}

using UnityEngine;

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

    [SerializeField] private float portalGunDistance = 100f;

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
        Vector2 position = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        raycast = Physics2D.Raycast(transform.position, direction, portalGunDistance, wallLayermask);

        if (raycast) {
            Vector3[] lineToRender = { position, raycast.point };
            lineRenderer.SetPositions(lineToRender);

            canShoot = true;
            angle = Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg;

            if (Input.GetKeyDown(KeyCode.Alpha1) == true && canShootBluePortal == true) {
                if (numberOfBlueShots > 0) {
                    numberOfBlueShots--;
                }
                if (numberOfBlueShots != 0) {
                    ShootPortal(bluePortal);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) == true && canShootOrangePortal == true) {
                if (numberOfOrangeShots > 0) {
                    numberOfOrangeShots--;
                }
                if (numberOfOrangeShots != 0) {
                    ShootPortal(orangePortal);
                }
            }
        }
        else {
            canShoot = false;
        }
    }

    public void ShootPortal(GameObject prefab) {
        if (canShoot == true) {
            Instantiate(prefab, raycast.point, Quaternion.AngleAxis(angle, Vector3.forward));
        }
    }

    public void ApplySettings(PortalSettings settings) {
        canShootBluePortal = settings.CanShootBluePortal;
        canShootOrangePortal = settings.CanShootOrangePortal;
        numberOfBlueShots = settings.NumberOfBluePortalsAllowed;
        numberOfOrangeShots = settings.NumberOfOrangePortalsAllowed;
    }

}

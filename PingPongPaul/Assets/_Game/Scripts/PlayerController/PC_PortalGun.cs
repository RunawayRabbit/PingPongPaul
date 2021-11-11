using UnityEngine;

public class PC_PortalGun : MonoBehaviour {
    public static PC_PortalGun pc_portalGun;

    [SerializeField] private LayerMask wallLayermask;
    [SerializeField] private LayerMask layermaskPortal;

    [Header("Settings")]
    [SerializeField] private GameObject bluePortal;
    [SerializeField] private bool canShootBluePortal = true;
    [SerializeField] private GameObject orangePortal;
    [SerializeField] private bool canShootOrangePortal = true;

    [SerializeField] private float portalGunDistance = 100f;

    private float angle;
    private RaycastHit2D raycast;
    private bool canShoot;

    private void Start() {
        pc_portalGun = this;
    }

    void Update() {
        Vector2 position = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        raycast = Physics2D.Raycast(transform.position, direction, portalGunDistance, wallLayermask);

        if (raycast) {
            Debug.DrawLine(position, raycast.point, Color.green);
            canShoot = true;
            angle = Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg;

            if (Input.GetKeyDown(KeyCode.Alpha1) == true && canShootBluePortal == true) {
                ShootBluePortal();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) == true && canShootOrangePortal == true) {
                ShootOrangePortal();
            }
        }
        else {
            canShoot = false;
        }
    }

    public void ShootBluePortal() {
        if (canShoot == true) {
            BluePortal portal = Instantiate(bluePortal, raycast.point, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<BluePortal>();
            portal.SetPortalNormal(raycast.normal);
        }
    }

    public void ShootOrangePortal() {
        if (canShoot == true) {
            OrangePortal portal = Instantiate(orangePortal, raycast.point, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<OrangePortal>();
            portal.SetPortalNormal(raycast.normal);
        }
    }

}

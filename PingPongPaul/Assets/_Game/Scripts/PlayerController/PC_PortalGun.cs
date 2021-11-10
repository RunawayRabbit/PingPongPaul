using UnityEngine;

public class PC_PortalGun : MonoBehaviour {
    [SerializeField] private LayerMask wallLayermask;
    [SerializeField] private LayerMask layermaskPortal;

    [Header("Settings")]
    [SerializeField] private GameObject bluePortal;
    [SerializeField] private bool canShootBluePortal = true;
    [SerializeField] private GameObject orangePortal;
    [SerializeField] private bool canShootOrangePortal = true;

    [SerializeField] private float portalGunDistance = 100f;

    void Update() {
        Vector2 position = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, portalGunDistance, wallLayermask);
        if (raycast) {
            Debug.DrawLine(position, raycast.point, Color.green);

            float angle = Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg;
            if (Input.GetKeyDown(KeyCode.Alpha1) == true && canShootBluePortal == true) {
                BluePortal portal = GameObject.Instantiate(bluePortal, raycast.point, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<BluePortal>();
                portal.SetPortalNormal(raycast.normal);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) == true && canShootOrangePortal == true) {
                OrangePortal portal = GameObject.Instantiate(orangePortal, raycast.point, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<OrangePortal>();
                portal.SetPortalNormal(raycast.normal);
            }
        }

        else {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_PortalGun : MonoBehaviour {
    [SerializeField] private LayerMask layermask;
    [SerializeField] private LayerMask layermaskPortal;

    [SerializeField] private GameObject bluePortal;
    [SerializeField] private GameObject orangePortal;

    void Update() {
        Vector2 position = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, 10f, layermask);
        if (raycast) {
            Debug.DrawLine(position, raycast.point, Color.green);

            float angle = Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg;
            if (Input.GetKeyDown(KeyCode.Alpha1) == true) {
                BluePortal portal = GameObject.Instantiate(bluePortal, raycast.point, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<BluePortal>();
                portal.SetPortalNormal(raycast.normal);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) == true) {
                OrangePortal portal = GameObject.Instantiate(orangePortal, raycast.point, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<OrangePortal>();
                portal.SetPortalNormal(raycast.normal);
            }
        }

        else {

        }
    }
}

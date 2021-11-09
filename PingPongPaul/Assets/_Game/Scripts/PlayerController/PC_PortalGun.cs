using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_PortalGun : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] private LayerMask layermask;

    [SerializeField] private GameObject bluePortal;
    [SerializeField] private GameObject orangePortal;


    void Start() {

    }

    // Update is called once per frame
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
            //Debug.DrawLine(position, position + (direction * 10f), Color.red);
        }

    }
}

using UnityEngine;
using UnityEngine.UI;

public class UI_VelocityProgress : MonoBehaviour {
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject parent;
    [SerializeField] private Slider slider;

    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    [SerializeField] private float sliderLength;

    [SerializeField] private float alpha;

    // Start is called before the first frame update
    void Start() {
        if (slider == null) {
            slider = GetComponentInChildren<Slider>();
        }

        if (canvas == null) {
            canvas = transform.parent.gameObject;
        }

        if (parent == null) {
            parent = gameObject;
        }

        Vector3 screenPoint = Input.mousePosition;
        parent.transform.position = screenPoint;

        direction = Vector3.forward;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.Mouse0) == true) {

            direction = Input.mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GetComponent<RectTransform>().rotation = rotation;


            distance = Vector3.Distance(Input.mousePosition, transform.position);

            alpha = Mathf.Lerp(0, 1, Mathf.Clamp(distance, 0, sliderLength) / sliderLength);
            slider.value = alpha;
            //Destroy(canvas);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            Destroy(canvas);
        }
    }

}
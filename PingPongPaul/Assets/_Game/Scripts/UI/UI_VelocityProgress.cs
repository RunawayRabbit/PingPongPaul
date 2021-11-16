using UnityEngine;
using UnityEngine.UI;

public class UI_VelocityProgress : MonoBehaviour
{
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private Transform ballTransform;

	new private Camera camera;

	[SerializeField] private GameObject canvas;
	[SerializeField] private GameObject parent;
	[SerializeField] private Slider slider;

	[SerializeField] private Vector3 direction;
	[SerializeField] private float distance;
	[SerializeField] private float sliderLength;

	[SerializeField] private float alpha;

	void Start()
	{
		if( slider == null ) { slider = GetComponentInChildren<Slider>(); }

		if( canvas == null ) { canvas = transform.parent.gameObject; }

		if( parent == null ) { parent = gameObject; }

		if( rectTransform == null ) { rectTransform = GetComponent<RectTransform>(); }

		camera = Camera.main;

		direction = Vector3.forward;

		Time.timeScale      = 0.05f;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;

		TrajectoryPrediction.instance.BeginTrajectory();
	}

	public void SetPosition( Transform transformToFollow ) { ballTransform = transformToFollow; }

	void Update()
	{
		if( Input.GetKey( KeyCode.Mouse1 ) == true )
		{
			StopExisting();
			return;
		}

		if( Input.GetKey( KeyCode.Mouse0 ) == true )
		{
			rectTransform.position = Camera.main.WorldToScreenPoint( ballTransform.position );

			direction = Input.mousePosition - transform.position;
			float      angle    = Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.AngleAxis( angle, Vector3.forward );
			rectTransform.rotation = rotation;

			distance = Vector3.Distance( Input.mousePosition, transform.position );

			alpha        = Mathf.Lerp( 0, 1, Mathf.Clamp( distance, 0, sliderLength ) / sliderLength );
			slider.value = alpha;

			Ball.ShowTrajectory.Invoke( direction, alpha );
		}

		if( Input.GetKeyUp( KeyCode.Mouse0 ) )
		{
			Ball.ShootBall.Invoke( direction, alpha );

			PC_VelocityProgress.pc_velocityProgress.Shoot();
			StopExisting();
		}
	}

	void StopExisting()
	{
		Destroy( canvas );

		TrajectoryPrediction.instance.EndTrajectory();

		Time.timeScale      = 1.0f;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
	}
}

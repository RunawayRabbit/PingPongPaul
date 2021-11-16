using UnityEngine;
using UnityEngine.Events;
using MC_Utility;

[System.Serializable] public class ShootBallEvent : UnityEvent<Vector2, float> { }
[System.Serializable] public class ShowTrajectoryEvent : UnityEvent<Vector2, float> { }


[RequireComponent( typeof(Rigidbody2D), typeof(CircleCollider2D) )]
public class Ball : MonoBehaviour
{
	public static ShootBallEvent ShootBall;
	public static ShowTrajectoryEvent ShowTrajectory;
	public static Ball ball;
	[SerializeField] private float maxForce = 14.0f;
	[SerializeField] private float paulStickiness = 14.0f;
	[SerializeField] private float maxPaulDistance = 3.0f;

	private TrajectoryPrediction trajectory;
	private RelativeJoint2D paulConnection;
	private Paul connectedPaul;
	private bool IsConnectedToPaul = false;
	private Vector3 startPosition;
	[SerializeField] public bool StopBeforeShooty { private set; get; } = false;

	private Rigidbody2D rb;
	private float radius;

	private void Awake()
	{
		ball = this;

		rb     = this.GetComponent<Rigidbody2D>();
		radius = this.GetComponent<CircleCollider2D>().radius;


		ShootBall = new ShootBallEvent();
		ShootBall.AddListener( OnShootBall );

		ShowTrajectory = new ShowTrajectoryEvent();
		ShowTrajectory.AddListener( OnShowTrajectory );
	}

	private void OnShowTrajectory( Vector2 direction, float t )
	{
		if (trajectory == null) {
			print("Trajectory is null");
        }
		trajectory.ShowTrajectory(direction.normalized,  maxForce * t);
	}

    public void Start() {
		trajectory = TrajectoryPrediction.instance;
		startPosition = transform.position;

	}

    private void OnEnable() {
		EventSystem<ResetEvent>.RegisterListener(ResetBall);
    }

    private void OnDisable() {
		EventSystem<ResetEvent>.UnregisterListener(ResetBall);
	}

	private void ResetBall(ResetEvent resetEvent) {
		transform.position = startPosition;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
	}

	private void Update()
	{
		if( IsConnectedToPaul && paulConnection.linearOffset.sqrMagnitude > maxPaulDistance * maxPaulDistance )
		{
			OnJointBreak2D( paulConnection );
		}

		if( Input.GetKeyDown( KeyCode.V ) )
		{
			StopBeforeShooty = !StopBeforeShooty;
			print( "Stop Before Shooty: " + StopBeforeShooty );
		}
	}

	public void OnShootBall( Vector2 Direction, float forcePercent )
	{
		if(StopBeforeShooty)
			rb.velocity = Vector2.zero;

		rb.AddForce( Direction.normalized * (maxForce * forcePercent), ForceMode2D.Impulse );
	}

	private void OnCollisionEnter2D( Collision2D other )
	{
		if( other.gameObject.layer == LayerMask.NameToLayer( "Paul" )
			&& connectedPaul == null )
		{
			connectedPaul = other.gameObject.GetComponentInParent<Paul>();
			connectedPaul.MakePaulRagDoll();

			paulConnection               = gameObject.AddComponent<RelativeJoint2D>();
			paulConnection.connectedBody = other.rigidbody;

			paulConnection.maxForce    = paulStickiness;
			paulConnection.breakForce  = paulStickiness;
			paulConnection.breakTorque = paulStickiness;

			IsConnectedToPaul = true;
		}
	}

	private void OnJointBreak2D( Joint2D brokenJoint )
	{
		if( brokenJoint == paulConnection )
		{
			Destroy( paulConnection );
			connectedPaul.BallLeavesPaul( gameObject );
			connectedPaul     = null;
			IsConnectedToPaul = false;
		}
	}


	public void ApplySettings() {
		BallSettings settings = LevelInstance.levelInstance.GetBallSettings();

		maxForce = settings.MaxForce;
		paulStickiness = settings.PaulStickiness;
		maxPaulDistance = settings.MaxPaulDistance;
		rb.mass = settings.ballMass;
		rb.drag = settings.linearDrag;
		rb.angularDrag = settings.angularDrag;
		StopBeforeShooty = settings.stopVelocityOnShoot;
	}

}

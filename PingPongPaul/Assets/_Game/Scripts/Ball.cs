using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class ShootBallEvent : UnityEvent<Vector2, float> { }


[RequireComponent( typeof(Rigidbody2D), typeof(CircleCollider2D) )]
public class Ball : MonoBehaviour
{
	public static ShootBallEvent ShootBall;
	public static Ball ball;
	[SerializeField] private float maxForce = 14.0f;
	[SerializeField] private float paulStickiness = 14.0f;
	[SerializeField] private float maxPaulDistance = 3.0f;

	private RelativeJoint2D paulConnection;
	private Paul connectedPaul;
	private bool IsConnectedToPaul = false;

	[SerializeField] private bool stopBeforeShooty = false;

	private Rigidbody2D rb;
	private float radius;

	private void Awake()
	{
		rb     = this.GetComponent<Rigidbody2D>();
		radius = this.GetComponent<CircleCollider2D>().radius;

		ShootBall = new ShootBallEvent();
		ShootBall.AddListener( OnShootBall );
	}

    public void Start() {
		ball = this;
    }

	private void Update()
	{
		if( IsConnectedToPaul && paulConnection.linearOffset.sqrMagnitude > maxPaulDistance * maxPaulDistance )
		{
			OnJointBreak2D( paulConnection );
		}

		if( Input.GetKeyDown( KeyCode.V ) )
		{
			stopBeforeShooty = !stopBeforeShooty;
			print( "Stop Before Shooty: " + stopBeforeShooty );
		}
	}

	public void OnShootBall( Vector2 Direction, float forcePercent )
	{
		if(stopBeforeShooty)
			rb.velocity = Vector2.zero;

		rb.AddForce( Direction.normalized * (maxForce * forcePercent), ForceMode2D.Impulse );
	}

	private void OnCollisionEnter2D( Collision2D other )
	{
		if( other.gameObject.layer == LayerMask.NameToLayer( "Paul" )
			&& connectedPaul == null )
		{
			connectedPaul = other.gameObject.GetComponentInParent<Paul>();
			connectedPaul.BallHitPaul( gameObject );

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
}

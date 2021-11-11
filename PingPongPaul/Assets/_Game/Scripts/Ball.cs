using UnityEngine;
using UnityEngine.Events;
using Random = Freya.Random;

[System.Serializable] public class ShootBallEvent : UnityEvent<Vector2, float> { }


[RequireComponent( typeof(Rigidbody2D), typeof(CircleCollider2D) )]
public class Ball : MonoBehaviour
{
	public static ShootBallEvent ShootBall;
	[SerializeField] private float maxForce = 14.0f;
	[SerializeField] private float paulStickiness = 14.0f;
	[SerializeField] private bool randomRotation = false;

	private RelativeJoint2D paulConnection;
	private Paul connectedPaul;

	private Rigidbody2D rb;
	private float radius;

	private void Awake()
	{
		rb     = this.GetComponent<Rigidbody2D>();
		radius = this.GetComponent<CircleCollider2D>().radius;

		ShootBall = new ShootBallEvent();
		ShootBall.AddListener( OnShootBall );
	}

	public void OnShootBall( Vector2 Direction, float forcePercent )
	{
		var Force = Direction.normalized * (maxForce * forcePercent);

		if( randomRotation )
		{
			rb.AddForceAtPosition( Direction.normalized * (maxForce * forcePercent),
								   transform.TransformPoint( Random.OnUnitCircle * radius ),
								   ForceMode2D.Impulse );
		}
		else
		{
			rb.AddForce( Force, ForceMode2D.Impulse );
		}
	}

	private void OnCollisionEnter2D( Collision2D other )
	{
		if( other.gameObject.layer == LayerMask.NameToLayer( "Paul" ) &&
			connectedPaul == null)
		{
			connectedPaul = other.gameObject.GetComponentInParent<Paul>();
			connectedPaul.BallHitPaul( gameObject );

			paulConnection               = gameObject.AddComponent<RelativeJoint2D>();
			paulConnection.connectedBody = other.rigidbody;

			paulConnection.maxForce      = paulStickiness;
			paulConnection.breakForce = paulStickiness;

		}
	}

	private void OnJointBreak2D( Joint2D brokenJoint )
	{
		if( brokenJoint == paulConnection )
		{
			Destroy( paulConnection );
			connectedPaul.BallLeavesPaul( gameObject );
			connectedPaul = null;
		}
	}
}

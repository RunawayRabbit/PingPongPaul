using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class ShootBallEvent : UnityEvent<Vector2, float> { }


[RequireComponent( typeof(Rigidbody2D), typeof(CircleCollider2D) )]
public class Paul : MonoBehaviour
{
	public static ShootBallEvent ShootBall;
	[SerializeField] private float maxForce = 14.0f;

	private Rigidbody2D rb;
	private float radius;

	private void Awake()
	{
		rb        = this.GetComponent<Rigidbody2D>();
		radius    = this.GetComponent<CircleCollider2D>().radius;
		ShootBall = new ShootBallEvent();
		ShootBall.AddListener( OnShootBall );
	}

	public void OnShootBall( Vector2 Direction, float forcePercent )
	{
		rb.AddForceAtPosition( Direction.normalized * (maxForce * forcePercent),
							   transform.TransformPoint( Random.insideUnitCircle * radius ),
							   ForceMode2D.Impulse );
	}
}

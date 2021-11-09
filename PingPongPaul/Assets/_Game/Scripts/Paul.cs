using UnityEngine;

[RequireComponent( typeof(Rigidbody2D), typeof(CircleCollider2D) )]
public class Paul : MonoBehaviour
{
	[SerializeField] private Vector2 direction;
	[SerializeField] private float force;

	private Rigidbody2D rb;
	private float radius;

	private void Awake()
	{
		rb     = this.GetComponent<Rigidbody2D>();
		radius = this.GetComponent<CircleCollider2D>().radius;
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			// Paul go wheee
			ShootPaul( direction, force );
		}
	}

	public void ShootPaul( Vector2 Direction, float Force )
	{
		rb.AddForceAtPosition( Direction * Force,
							   transform.TransformPoint( Random.insideUnitCircle * radius ),
							   ForceMode2D.Impulse );
	}
}

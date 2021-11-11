using UnityEngine;

public class PaulBalance : MonoBehaviour
{
	private Rigidbody2D rb;

	[SerializeField] private float strength = 20.0f;
	[SerializeField] private float targetAngle = 90;

	private void Awake()
	{
		rb = this.GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		rb.MoveRotation( Mathf.LerpAngle(rb.rotation, targetAngle, strength * Time.fixedDeltaTime ));
	}
}

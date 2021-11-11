using UnityEngine;

public class PaulBalance : MonoBehaviour
{
	private Rigidbody2D rb;

	[SerializeField] private float strength = 20.0f;
	[SerializeField] private float targetAngle = 0;
	[SerializeField] private float maximumAngle = 110.0f;

	private void Awake()
	{
		rb = this.GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if(Mathf.Abs(rb.rotation - targetAngle) < maximumAngle)
			rb.MoveRotation( Mathf.LerpAngle(rb.rotation, targetAngle, strength * Time.fixedDeltaTime * Time.timeScale ));
	}
}

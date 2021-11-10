using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulHands : MonoBehaviour
{
	private Rigidbody2D rb;

	[SerializeField] private bool isGrabbing = false;
	[SerializeField] private Transform grabTarget;
	[SerializeField] private float force;

	private void Awake()
	{
		rb = this.GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if( isGrabbing && grabTarget )
		{
			Vector3 delta = grabTarget.position - transform.position;
			float   angle = Mathf.Atan2( delta.x, -delta.y ) * Mathf.Rad2Deg;
			rb.MoveRotation( Mathf.LerpAngle(rb.rotation, angle, force * Time.deltaTime) );
		}
	}
}

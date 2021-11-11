using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody2D) )]
public class Resettable : MonoBehaviour
{
	private static readonly List<Resettable> allResettables = new List<Resettable>();

	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Rigidbody2D rb;

	private void OnEnable() { allResettables.Add( this ); }

	private void OnDisable() { allResettables.Remove( this ); }

	private void Awake()
	{
		rb = this.GetComponent<Rigidbody2D>();

		initialPosition = transform.position;
		initialRotation = transform.rotation;
	}


	public static void ResetAll()
	{
		foreach( var resettable in allResettables )
		{
			resettable.transform.position = resettable.initialPosition;
			resettable.transform.rotation = resettable.initialRotation;

			if( resettable.rb )
			{
				resettable.rb.velocity        = Vector2.zero;
				resettable.rb.angularVelocity = 0;
			}
		}
	}
}

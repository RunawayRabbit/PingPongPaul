using System;
using MC_Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paul : MonoBehaviour
{
	private static List<Paul> allPauls = new List<Paul>();

	private List<PaulBalance> balances;
	private List<Vector3> cachedPositions;
	private List<Quaternion> cachedRotations;

	private bool isAttachedToBall;

	private Rigidbody2D _rb;

	public bool hasPortalledRecently { get; private set; } = false;
	private readonly WaitForSeconds paulPortalWait = new WaitForSeconds( 1.0f );

	private void Awake()
	{
		balances         = new List<PaulBalance>( gameObject.GetComponentsInChildren<PaulBalance>() );
		cachedPositions  = new List<Vector3>( transform.childCount );
		cachedRotations  = new List<Quaternion>( transform.childCount );

		_rb = this.GetComponent<Rigidbody2D>();

		// Cache data for resetting later
		cachedPositions.Add( transform.position );
		cachedRotations.Add( transform.rotation );

		for( var i = 1; i < transform.childCount; ++i )
		{
			cachedPositions.Add( transform.GetChild( i ).position );
			cachedRotations.Add( transform.GetChild( i ).rotation );
		}
	}

	private void OnEnable() { allPauls.Add( this ); }

	private void OnDisable() { allPauls.Remove( this ); }

	private void Reset()
	{
		transform.position = cachedPositions[0];
		transform.rotation = cachedRotations[0];

		for( var i = 1; i < transform.childCount; ++i )
		{
			var part = transform.GetChild( i ).gameObject;
			part.transform.position = cachedPositions[i];
			part.transform.rotation = cachedRotations[i];

			if( part.TryGetComponent<Rigidbody2D>( out Rigidbody2D partRb ) )
			{
				partRb.velocity        = Vector2.zero;
				partRb.angularVelocity = 0;
			}
		}
	}

	public static void ResetAllPauls()
	{
		foreach( var paul in allPauls.ToArray() ) { paul.Reset(); }
	}

	public void PreparePaulForTeleportation()
	{
		foreach( var rb in gameObject.GetComponentsInChildren<Rigidbody2D>() )
		{
			if( rb == _rb ) continue;
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			rb.simulated   = false;
		}

	}

	private void UnfreezePaul()
	{
		foreach( var rb in gameObject.GetComponentsInChildren<Rigidbody2D>() )
		{
			if( rb == _rb ) continue;
			rb.constraints = RigidbodyConstraints2D.None;
			rb.simulated   = true;
		}
	}

	public void EndPaulsTeleportingAdventure()
	{
		UnfreezePaul();
		hasPortalledRecently = true;
		StartCoroutine(MakePaulGreatAgain());
	}

	private IEnumerator MakePaulGreatAgain()
	{
		yield return paulPortalWait;
		UnfreezePaul();
		hasPortalledRecently = false;
	}

	public void Update()
	{
		if( Input.GetKeyDown( KeyCode.F1 ) ) { PreparePaulForTeleportation(); }

		if( Input.GetKeyDown( KeyCode.F2 ) ) { EndPaulsTeleportingAdventure(); }
	}


	public void BallHitPaul( GameObject ball )
	{
		isAttachedToBall = true;

		foreach( var balancer in balances ) { balancer.enabled = false; }
	}

	private IEnumerator StandBackUp()
	{
		yield return new WaitForSeconds( 1.0f );

		if( !isAttachedToBall )
		{
			foreach( var balancer in balances ) { balancer.enabled = true; }
		}
	}

	public void BallLeavesPaul( GameObject ball )
	{
		isAttachedToBall = true;
		StartCoroutine( StandBackUp() );
	}
}

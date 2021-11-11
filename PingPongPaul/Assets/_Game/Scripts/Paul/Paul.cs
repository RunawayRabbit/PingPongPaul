using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Paul : MonoBehaviour
{
	private static List<Paul> allPauls = new List<Paul>();

	private Vector3 startingPosition;

	private List<PaulBalance> balances;
	private List<Vector3> cachedPositions;
	private List<Quaternion> cachedRotations;

	private bool isAttachedToBall;

	private void Awake()
	{
		startingPosition = transform.position;
		balances         = new List<PaulBalance>( gameObject.GetComponentsInChildren<PaulBalance>() );
		cachedPositions  = new List<Vector3>( transform.childCount );
		cachedRotations  = new List<Quaternion>( transform.childCount );

		// Cache data for resetting later
		cachedPositions.Add( transform.position );
		cachedRotations.Add( transform.rotation );
		for( var i = 1; i < transform.childCount; ++i )
		{
			cachedPositions.Add( transform.GetChild(i).position );
			cachedRotations.Add( transform.GetChild(i).rotation );
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
			transform.GetChild( i ).position = cachedPositions[i];
			transform.GetChild( i ).rotation = cachedRotations[i];
		}
	}

	public static void ResetAllPauls()
	{
		foreach( var paul in allPauls.ToArray() )
		{
			//Instantiate( PrefabUtility.LoadPrefabContents( paul.prefabPath ), paul.startingPosition, Quaternion.identity );
			//Destroy( paul.gameObject );

			paul.Reset();
		}
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

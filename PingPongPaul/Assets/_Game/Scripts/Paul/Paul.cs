using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Paul : MonoBehaviour
{
	private static List<Paul> allPauls = new List<Paul>();
	[SerializeField] private string prefabPath = "Assets/_Game/Prefab/Paul.prefab";

	private Vector3 startingPosition;

	private List<PaulBalance> balances;

	private bool isAttachedToBall;

	private void Awake()
	{
		startingPosition = transform.position;
		balances         = new List<PaulBalance>(this.GetComponentsInChildren<PaulBalance>());
	}

	private void OnEnable() { allPauls.Add( this ); }

	private void OnDisable() { allPauls.Remove( this ); }

	private void Reset()
	{
		Instantiate( gameObject, startingPosition, Quaternion.identity );
		Destroy( gameObject, 0.001f );
	}

	public static void ResetAllPauls()
	{
		foreach( var paul in allPauls.ToArray() )
		{
			//Instantiate( PrefabUtility.LoadPrefabContents( paul.prefabPath ), paul.startingPosition, Quaternion.identity );
			Instantiate( paul.gameObject, paul.startingPosition, quaternion.identity );
			Destroy( paul.gameObject );
		}
	}

	public void BallHitPaul( GameObject ball )
	{
		isAttachedToBall = true;
		foreach( var balancer in balances )
		{
			balancer.enabled = false;
		}
	}

	private IEnumerator StandBackUp()
	{
		yield return new WaitForSeconds( 1.0f );
		if(!isAttachedToBall)
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

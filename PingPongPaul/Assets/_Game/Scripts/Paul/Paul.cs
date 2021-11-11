using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Paul : MonoBehaviour
{
	private static List<Paul> allPauls = new List<Paul>();
	private static string paulPrefabPath = "Assets/_Game/Prefab/Paul.prefab";

	private Vector3 startingPosition;

	private void Awake()
	{
		startingPosition = transform.position;
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
		var paulPrefab = PrefabUtility.LoadPrefabContents( paulPrefabPath );
		foreach( var paul in allPauls.ToArray() )
		{
			Instantiate( paulPrefab, paul.startingPosition, Quaternion.identity );
			Destroy( paul.gameObject );
		}
	}

	public void BallHitPaul( GameObject ball )
	{

	}
}

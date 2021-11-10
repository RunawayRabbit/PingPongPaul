using System.Collections.Generic;
using UnityEngine;

public class DestructibleSurface : MonoBehaviour
{
	private static readonly List<DestructibleSurface> allDestructibleSurfaces = new List<DestructibleSurface>();

	private void OnDisable() { allDestructibleSurfaces.Add( this ); }


	public static void ResetAllDestructibleSurfaces()
	{
		print("boop");
		foreach( var surface in allDestructibleSurfaces ) { surface.gameObject.SetActive( true ); }
		allDestructibleSurfaces.Clear();
	}

	private void OnCollisionEnter2D( Collision2D other )
	{
		if( other.gameObject.layer == LayerMask.NameToLayer( "Ball" ) ) { gameObject.SetActive( false ); }
	}
}

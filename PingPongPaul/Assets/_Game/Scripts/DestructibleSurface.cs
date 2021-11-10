using System.Collections.Generic;
using UnityEngine;

public class DestructibleSurface : MonoBehaviour
{
	private static readonly List<DestructibleSurface> allDestructibleSurfaces = new List<DestructibleSurface>();

	private void OnEnable() { allDestructibleSurfaces.Add( this ); }

	private void OnDisable() { allDestructibleSurfaces.Remove( this ); }


	public void ResetAllDestructibleSurfaces()
	{
		foreach( var surface in allDestructibleSurfaces ) { surface.gameObject.SetActive( true ); }
	}

	private void OnCollisionEnter2D( Collision2D other )
	{
		if( other.gameObject.layer == LayerMask.NameToLayer( "Ball" ) ) { gameObject.SetActive( false ); }
	}
}

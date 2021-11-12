using _Game.Scripts.MovingPlatforms;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof(MovingPlatform) )]
public class MovingPlatformEditor : Editor
{
	private Collider2D _collider;
	private MovingPlatform _platform;

	private void OnEnable()
	{
		if( !this.target ) return;
		_platform = (MovingPlatform) this.target;

		_platform.MakeWaypointsArraySafe();
		_platform.waypoints[0].position = _platform.transform.position;

		_collider = _platform.GetComponent<Collider2D>();
	}

	private void OnSceneGUI()
	{
		_platform.MakeWaypointsArraySafe();

		if( !Application.isPlaying ) _platform.waypoints[0].position = _platform.transform.position;

		int numIterations = _platform.waypoints.Length - 1;

		if( _platform.movementStyle == MovingPlatform.LoopType.LoopBackToTheStart ) numIterations++;

		using( new Handles.DrawingScope() )
		{
			for( var i = 0; i < numIterations; i++ )
			{
				Handles.color = new Color( 0.8f, 0.0f, 0.0f, 0.6f );
				DrawWaypointLines( i );
			}

			for( var i = 0; i < _platform.waypoints.Length; i++ ) DrawWaypoints( i );
		}
	}

	private void DrawWaypointLines( int index )
	{
		Vector3 currentPosition = _platform.waypoints[index].position;
		int     next            = (index + 1) % _platform.waypoints.Length;

		Handles.DrawLine( currentPosition, _platform.waypoints[next].position );
	}

	private void DrawWaypoints( int index )
	{
		var style = new GUIStyle();
		style.fontStyle        = FontStyle.Bold;
		style.fontSize         = 12;
		style.alignment        = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;

		var startColor = new Color( 0.1f, 1.0f, 0.2f, 0.7f );
		var endColor   = new Color( 0.1f, 0.2f, 1.0f, 0.7f );

		Vector3 currentPosition = _platform.waypoints[index].position;


		Handles.color = Color.Lerp( startColor, endColor, (float) index / _platform.waypoints.Length );

		if( _collider is BoxCollider2D )
		{
			using( new Handles.DrawingScope( Matrix4x4.TRS( currentPosition,
															Quaternion.identity,
															_platform.transform.localScale ) ) )
			{
				Handles.DrawWireCube( Vector3.zero, Vector3.one );
			}
		}
		else if( _collider is CircleCollider2D )
		{
			var circleCollider = _collider as CircleCollider2D;
			Handles.DrawWireDisc( currentPosition, Vector3.forward, circleCollider.radius * _platform.transform.localScale.x );
		}

		string waypointText = index == 0 ? "Starting Position" : $"Waypoint {index}";

		Handles.Label(currentPosition, $"{waypointText}\nWait Time: {_platform.waypoints[index].waitTime} sec", style );

		// Early-out waypoint zero.
		if( index == 0
			&& !Application.isPlaying )
			return;

		EditorGUI.BeginChangeCheck();

		Vector3 newPoint = Handles.DoPositionHandle( currentPosition, Quaternion.identity );

		if( EditorGUI.EndChangeCheck() )
		{
			Undo.RecordObject( _platform, "Moved EnemyPath Point" );
			EditorUtility.SetDirty( _platform );

			_platform.waypoints[index].position = newPoint; // + platform.transform.position;
		}
	}
}

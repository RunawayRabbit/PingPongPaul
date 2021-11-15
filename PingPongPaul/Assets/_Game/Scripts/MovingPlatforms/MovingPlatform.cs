using System;
using Freya;
using UnityEngine;

namespace _Game.Scripts.MovingPlatforms
{
public class MovingPlatform : SignalReceiverComponent
{
	[Serializable]
	public struct PlatformWaypoint
	{
		public Vector2 position;
		public float waitTime;
	}

	private enum MovementType
	{
		NotSmooth,
		QuiteSmooth,
		VerySmooth,
	}

	public enum LoopType
	{
		LoopBackToTheStart,
		MoveBackAndForth,
	}

	[SerializeField] private bool isMoving = false;

	private int _currentPosition = 0;
	private int _nextPosition = 0;

	private float _fractionalTimeElapsed;

	private int _incrementOrDecrement = 1;

	private float _journeyDistance;
	private float _moveDuration;

	[SerializeField, Range( 0.1f, 5.0f ),
	 Tooltip(
		 "AVERAGE speed of the platform. Smoothing will make it slower at the start/end and faster in the middle." )]
	private float movementSpeed = 2.0f;

	[Space( 2 ), Header( "    Movement Stuff" ), SerializeField]
	public LoopType movementStyle;

	private Rigidbody2D _rigidbody;
	private Collider2D _boxCollider;

	[Header( "    Waypoint Stuff" ), SerializeField]
	private MovementType smoothing;

	private float waitTimer = 0.0f;
	[SerializeField] public PlatformWaypoint[] waypoints;

	private void Awake()
	{
		_rigidbody    = this.GetComponent<Rigidbody2D>();
		_boxCollider = this.GetComponent<Collider2D>();

		_journeyDistance = Vector3.Distance( waypoints[0].position, waypoints[1].position );
		_moveDuration    = _journeyDistance / movementSpeed;

		_rigidbody.isKinematic = true;
	}

	#if UNITY_EDITOR
	public void MakeWaypointsArraySafe()
	{
		if( waypoints != null
			&& waypoints.Length >= 2 )
			return;

		Vector3 position = this.transform.position;
		Vector3 forward  = this.transform.forward;

		waypoints = new PlatformWaypoint[2];

		for( var i = 0;
			 i < waypoints.Length;
			 i++ )
		{
			waypoints[i].position = position + i * 3.0f * forward;
			waypoints[i].waitTime = 1.0f;
		}
	}
#endif


	private void FixedUpdate()
	{
		if( !isMoving ) return;

		Vector3 playerPos         = Ball.ball.transform.position;

		Vector3 platformPosition  = _boxCollider.ClosestPoint( playerPos );
		Vector3 tolerancePosition = (playerPos - platformPosition).normalized;

		float playerIsAbovePlatform =
			Vector3.Dot( this.transform.up, tolerancePosition );
		_boxCollider.enabled = playerIsAbovePlatform > -0.01f;

		if( WaitForTimer() ) return;

		float currentDistance =
			Vector3.Distance( this.transform.position,
							  waypoints[_nextPosition].position );


		if( currentDistance < 0.01f )
		{
			SwitchTarget();

			return;
		}

		PerformMove();
	}

	private bool WaitForTimer()
	{
		waitTimer -= Time.deltaTime;

		return waitTimer > 0.0f;
	}

	private void SwitchTarget()
	{
		_fractionalTimeElapsed = 0.0f;
		_currentPosition       = _nextPosition;
		// First, move us to the *precise* location of the waypoint.
		_rigidbody.MovePosition( waypoints[_currentPosition].position );

		// Set up the timer for the next move.
		waitTimer = waypoints[_currentPosition].waitTime;

		// Establish where we're going next.
		int nextIndexCandidate = _currentPosition + _incrementOrDecrement;

		if( nextIndexCandidate < 0
			|| nextIndexCandidate > waypoints.Length - 1 )
		{
			// NextIndexCandidate is out of bounds. Make the right choice:
			switch( movementStyle )
			{
				case LoopType.LoopBackToTheStart:
					_nextPosition = 0;

					break;

				case LoopType.MoveBackAndForth:
					_incrementOrDecrement *= -1;
					_nextPosition         =  _currentPosition + _incrementOrDecrement;

					break;
			}
		}
		else
		{
			// Otherwise, everything is fine.
			_nextPosition = nextIndexCandidate;
		}

		_journeyDistance = Vector3.Distance( waypoints[_currentPosition].position,
											waypoints[_nextPosition].position );
		_moveDuration = _journeyDistance / movementSpeed;
	}

	private void PerformMove()
	{
		Vector3 from = waypoints[_currentPosition].position;
		Vector3 to   = waypoints[_nextPosition].position;
		_fractionalTimeElapsed += Time.deltaTime / _moveDuration;
		float t;

		switch( smoothing )
		{
			case MovementType.QuiteSmooth:
				t = Mathfs.Smooth01( _fractionalTimeElapsed );

				break;

			case MovementType.VerySmooth:
				t = Mathfs.Smoother01( _fractionalTimeElapsed );

				break;

			default: // MovementType.NotSmooth:
				t = _fractionalTimeElapsed;

				break;
		}

		_rigidbody.MovePosition( Vector3.Lerp( from, to, t ) );
	}

	public override void Interact()
	{
		isMoving = !isMoving;
	}
}
}

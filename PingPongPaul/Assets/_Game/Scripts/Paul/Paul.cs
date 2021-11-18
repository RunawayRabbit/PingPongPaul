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

	[SerializeField] public bool breakForDemonstrationPurposes = true;

	[SerializeField] private SpriteRenderer PaulFace;

	[SerializeField] private Sprite paulHappy;
	[SerializeField] private Sprite paulSurprised;
	[SerializeField] private Sprite paulPain;
	[SerializeField] private Sprite paulSad;
	[SerializeField] private Sprite paulDead;


	private Rigidbody2D _rb;

	public bool hasPortalledRecently { get; private set; } = false;
	private readonly WaitForSeconds paulPortalWait = new WaitForSeconds( 1.0f );

	private void Awake()
	{
		balances        = new List<PaulBalance>( gameObject.GetComponentsInChildren<PaulBalance>() );
		cachedPositions = new List<Vector3>( transform.childCount );
		cachedRotations = new List<Quaternion>( transform.childCount );

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

		foreach( var rb in gameObject.GetComponentsInChildren<Rigidbody2D>() )
		{
			rb.velocity        = Vector2.zero;
			rb.angularVelocity = 0;
		}

		for( var i = 1; i < transform.childCount; ++i )
		{
			var part = transform.GetChild( i ).gameObject;
			part.transform.position = cachedPositions[i];
			part.transform.rotation = cachedRotations[i];
		}

		MakePaulHappy();
		StandBackUp();
	}

	public static void ResetAllPauls()
	{
		foreach( var paul in allPauls.ToArray() ) { paul.Reset(); }
	}

	public void PreparePaulForTeleportation()
	{
		if( breakForDemonstrationPurposes )
		{
			hasPortalledRecently = false;
			MakePaulSad();
			return;
		}

		foreach( var rb in gameObject.GetComponentsInChildren<Rigidbody2D>() )
		{
			if( rb == _rb ) continue;
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			rb.simulated   = false;
		}
		SurprisePaul();

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
		StartCoroutine( MakePaulGreatAgain() );
	}

	private IEnumerator MakePaulGreatAgain()
	{
		yield return paulPortalWait;
		UnfreezePaul();
		hasPortalledRecently = false;

		MakePaulHappy();
	}

	public void MakePaulRagDoll()
	{
		isAttachedToBall = true;

		foreach( var balancer in balances ) { balancer.enabled = false; }

		HurtPaul();
	}

	private IEnumerator StandBackUpDelayed()
	{
		yield return new WaitForSeconds( 1.0f );

		StandBackUp();
	}

	private void StandBackUp()
	{
		if( !isAttachedToBall )
		{
			foreach( var balancer in balances ) { balancer.enabled = true; }
		}
	}

	public void BallLeavesPaul( GameObject ball )
	{
		isAttachedToBall = false;
		StartCoroutine( StandBackUpDelayed() );
	}

	public void SurprisePaul() { PaulFace.sprite = paulSurprised; }

	public void MakePaulHappy() { PaulFace.sprite = paulHappy; }

	public void MakePaulSad() { PaulFace.sprite = paulSad; }

	public void HurtPaul() { PaulFace.sprite = paulPain; }

	public void KillPaul()
	{
		print( "KillPaul" );
		MakePaulRagDoll();
		PaulFace.sprite = paulDead;
	}
}

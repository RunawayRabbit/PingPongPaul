using UnityEngine;

public class OrangePortal : PortalBase
{
	public static OrangePortal orangePortal;

	protected override void OnConfirmPortalPlacement()
	{
		if( orangePortal != null ) { Destroy( orangePortal.gameObject ); }

		orangePortal = this;

		canTeleport = true;
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		if( (portalableObjectLayerMask.value | (1 << other.gameObject.layer) ) != 0)
		{
			BluePortal bluePortal = BluePortal.bluePortal;

			if( canTeleport == true
				&& bluePortal != null
				&& bluePortal.CanTeleport() == true ) { EnterPortal( bluePortal, other.gameObject ); }
		}
	}

	private void OnTriggerExit2D( Collider2D other )
	{
		if( isGhost == false ) { canTeleport = true; }

		ExitPortal( orangePortal, other.gameObject );

	}

	protected override void OnReset() { Destroy( orangePortal.gameObject ); }

	public override void ApplySettings( PortalSettings settings ) { canBeReset = settings.CanOrangePortalReset; }
}

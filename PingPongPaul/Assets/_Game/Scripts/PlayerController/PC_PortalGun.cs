using UnityEngine;

public enum PortalVisualizationStyle
{
	DrawALine,
	ShowAtTarget,
}
public class PC_PortalGun : MonoBehaviour
{
	public static PC_PortalGun pc_portalGun;

	[Header( "References" )]
	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField] private LayerMask wallLayermask;
	[SerializeField] private LayerMask layermaskPortal;

	[Header( "Settings" )]
	[SerializeField] private GameObject bluePortal;
	[SerializeField] private bool canShootBluePortal = true;
	[SerializeField] private int numberOfBlueShots = -1;

	[SerializeField] private GameObject orangePortal;
	[SerializeField] private bool canShootOrangePortal = true;
	[SerializeField] private int numberOfOrangeShots = -1;

	[SerializeField] private PortalVisualizationStyle VisualizationStyle;

	[SerializeField] private float portalGunDistance = 100f;

	private Transform portalVisualizer;

	private float angle;
	private RaycastHit2D raycast;
	private bool canShoot;

	private void Start()
	{
		pc_portalGun = this;

		if( lineRenderer == null ) { lineRenderer = GetComponent<LineRenderer>(); }
	}

	void Update()
	{
		Vector2 position  = transform.position;
		Vector2 direction = (Camera.main.ScreenToWorldPoint( Input.mousePosition ) - transform.position).normalized;
		raycast = Physics2D.Raycast( transform.position, direction, portalGunDistance, wallLayermask );

		if( raycast )
		{
			if( VisualizationStyle == PortalVisualizationStyle.DrawALine )
			{
				Vector3[] lineToRender = { position, raycast.point };
				lineRenderer.SetPositions( lineToRender );
			}
			else { lineRenderer.enabled = false; }

			canShoot = true;
			angle    = Mathf.Atan2( raycast.normal.y, raycast.normal.x ) * Mathf.Rad2Deg;


			if( VisualizationStyle == PortalVisualizationStyle.ShowAtTarget )
			{
				if( Input.GetKeyDown( KeyCode.Alpha1 ) && canShootBluePortal ) { InstantiateVisualizer( bluePortal ); }

				if( Input.GetKeyDown( KeyCode.Alpha2 ) && canShootOrangePortal )
				{
					InstantiateVisualizer( orangePortal );
				}


				if( Input.GetKey( KeyCode.Alpha1 ) && canShootBluePortal ) { VisualizePortal(); }

				if( Input.GetKey( KeyCode.Alpha2 ) && canShootOrangePortal ) { VisualizePortal(); }
			}


			if( Input.GetKeyUp( KeyCode.Alpha1 ) == true
				&& canShootBluePortal == true )
			{
				if( numberOfBlueShots != 0 )
				{
					ShootPortal( bluePortal );
					numberOfBlueShots--;
				}
			}

			if( Input.GetKeyUp( KeyCode.Alpha2 ) == true
				&& canShootOrangePortal == true )
			{
				if( numberOfOrangeShots != 0 )
				{
					ShootPortal( orangePortal );
					numberOfOrangeShots--;
				}
			}
		}
		else { canShoot = false; }
	}


	private void InstantiateVisualizer( GameObject thingy )
	{
		portalVisualizer = Instantiate( thingy, raycast.point, Quaternion.AngleAxis( angle, Vector3.forward ) ).
			transform;

		var spriteRenderer = portalVisualizer.GetComponent<SpriteRenderer>();
		spriteRenderer.color *= new Color( 1.0f, 1.0f, 1.0f, 0.3f );
	}

	private void VisualizePortal()
	{
		portalVisualizer.position = raycast.point;
		portalVisualizer.rotation = Quaternion.AngleAxis( angle, Vector3.forward );
	}

	private void ShootPortal( GameObject prefab )
	{
		if( canShoot == true ) { Instantiate( prefab, raycast.point, Quaternion.AngleAxis( angle, Vector3.forward ) ); }
	}

	public void ApplySettings( PortalSettings settings )
	{
		canShootBluePortal   = settings.CanShootBluePortal;
		canShootOrangePortal = settings.CanShootOrangePortal;
		numberOfBlueShots    = settings.NumberOfBluePortalsAllowed;
		numberOfOrangeShots  = settings.NumberOfOrangePortalsAllowed;
		VisualizationStyle   = settings.visualizationStyle;
	}
}

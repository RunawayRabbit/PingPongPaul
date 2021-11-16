using UnityEngine;

public class DeathBox : MonoBehaviour
{
	[SerializeField] private int paulLayer = 7;
	[SerializeField] private int ballLayer = 10;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		var otherGameObject = collision.gameObject;
		if( otherGameObject.layer == paulLayer )
		{
			Paul paul = otherGameObject.GetComponentInParent<Paul>();
			if( paul || otherGameObject.TryGetComponent( out paul ) ){ paul.KillPaul(); }
			PC_UIController.pc_uiController.ShowWinScreen();
		}

		if( otherGameObject.layer == ballLayer ) { }
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		var otherGameObject = collision.gameObject;
		if( otherGameObject.layer == paulLayer )
		{
			Paul paul = otherGameObject.GetComponentInParent<Paul>();
			if( paul || otherGameObject.TryGetComponent( out paul ) ){ paul.KillPaul(); }

			PC_UIController.pc_uiController.ShowWinScreen();
		}

		if( otherGameObject.layer == ballLayer ) { }
	}
}

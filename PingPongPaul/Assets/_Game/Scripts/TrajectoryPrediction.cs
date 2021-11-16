using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent( typeof(LineRenderer) )]
public class TrajectoryPrediction : MonoBehaviour
{
	public static TrajectoryPrediction instance;

	private Scene mainScene;
	private Scene physicsScene;

	[SerializeField] float timeFactor = 16.0f;
	[SerializeField] private float predictionTime = 1.5f;

	private GameObject dummyBall;
	private Rigidbody2D dummyRigidbody;

	private GameObject actualBall;
	private Rigidbody2D ballRigidbody;

	private bool stopBeforeShooty;

	private LineRenderer lineRenderer;

	private void Awake()
	{
		lineRenderer             = this.GetComponent<LineRenderer>();
		Physics2D.simulationMode = SimulationMode2D.Script;

		mainScene = SceneManager.GetActiveScene();

		physicsScene =
			SceneManager.CreateScene( "TrajectorySim", new CreateSceneParameters( LocalPhysicsMode.Physics2D ) );

		lineRenderer         = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;

		if( instance == null ) 
			instance = this;
	}

	private void FixedUpdate() { mainScene.GetPhysicsScene2D().Simulate( Time.fixedDeltaTime ); }


	public void BeginTrajectory()
	{
		SceneManager.SetActiveScene( physicsScene );

		dummyBall = new GameObject( "dummyBall" )
		{
			layer = LayerMask.NameToLayer( "DUMMY_BALL" ),
		};
		SceneManager.SetActiveScene( mainScene );

		dummyRigidbody = dummyBall.AddComponent<Rigidbody2D>();

		actualBall       = Ball.ball.gameObject;
		ballRigidbody    = actualBall.GetComponent<Rigidbody2D>();
		stopBeforeShooty = Ball.ball.StopBeforeShooty;

		var col = dummyBall.AddComponent<CircleCollider2D>();
		col.radius = actualBall.GetComponent<CircleCollider2D>().radius;

		lineRenderer.positionCount = 0;
		lineRenderer.enabled       = true;
	}

	public void EndTrajectory()
	{
		Destroy( dummyBall.gameObject );
		lineRenderer.enabled = false;
	}

	public void ShowTrajectory( Vector2 direction, float force )
	{
		SceneManager.SetActiveScene( physicsScene );

		dummyRigidbody.MovePosition( actualBall.transform.position );
		dummyRigidbody.MoveRotation( actualBall.transform.rotation );

		dummyRigidbody.velocity        = stopBeforeShooty ? Vector2.zero : ballRigidbody.velocity;
		dummyRigidbody.angularVelocity = stopBeforeShooty ? 0 : ballRigidbody.angularVelocity;

		dummyRigidbody.AddForce( direction * force, ForceMode2D.Impulse );

		int numIterations = Mathf.FloorToInt( predictionTime / Time.fixedDeltaTime / timeFactor );
		lineRenderer.positionCount = numIterations;

		for( var i = 0; i < numIterations; ++i )
		{
			physicsScene.GetPhysicsScene2D().Simulate( Time.fixedDeltaTime * timeFactor );
			lineRenderer.SetPosition( i, dummyBall.transform.position );
		}

		SceneManager.SetActiveScene( mainScene );
	}
}

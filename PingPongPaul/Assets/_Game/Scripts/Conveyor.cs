using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Conveyor : MonoBehaviour
{
    private enum ConveyorDirection
    {
        Left, Right,
    }

    [SerializeField] private ConveyorDirection direction;
    [SerializeField] private float speed;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        this.GetComponent<SpriteRenderer>().flipX = direction == ConveyorDirection.Right;
    }
    #endif

    private void FixedUpdate()
    {
        Vector2 right        = transform.right  * speed * Time.fixedDeltaTime;
        Vector2 displacement = direction == ConveyorDirection.Right ? right : -right;

        rigidbody.position -= (Vector2) displacement;
        rigidbody.MovePosition( rigidbody.position + displacement);
    }
}

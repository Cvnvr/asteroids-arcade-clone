using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipMovementController : MonoBehaviour, IMoveable
{
    #region Variables
    private Rigidbody2D rigidbody;

    // Forward movement
    [SerializeField] private float forwardThrust;
    private float forwardInput;
    [SerializeField] private float maxSpeed;

    // Rotational movement
    [SerializeField] private float rotationalThrust;
    private float rotationalInput;
    [SerializeField] private float maxRotationalSpeed;

    public delegate void OnPlayerCollision();
    public event OnPlayerCollision onPlayerTakeDamage;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    #endregion Initialisation

    private void Update()
    {
        forwardInput = PlayerInputHandler.GetForwardInput();
        rotationalInput = PlayerInputHandler.GetRotationalInput();

        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        // MOVE
        rigidbody.AddForce(forwardInput * forwardThrust * Time.deltaTime * transform.up);

        // ROTATE
        float rotation = Mathf.Clamp(rotationalInput * rotationalThrust * Time.deltaTime, -maxRotationalSpeed, maxRotationalSpeed);
        rigidbody.AddTorque(-rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            onPlayerTakeDamage.Invoke();
        }
    }
}
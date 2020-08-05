using System;
using UnityEngine;

/// <summary>
/// Handles the player movement
/// </summary>
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

    public event Action OnPlayerTakesDamage;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    #endregion Initialisation

    private void Update()
    {
        // Retrieves input values from player
        forwardInput = PlayerInputHandler.GetForwardInput();
        rotationalInput = PlayerInputHandler.GetRotationalInput();
    }

    private void FixedUpdate()
    {
        // ROTATE
        float rotation = Mathf.Clamp(rotationalInput * rotationalThrust * Time.deltaTime, -maxRotationalSpeed, maxRotationalSpeed);
        rigidbody.AddTorque(-rotation);

        // MOVE
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);

        var direction = forwardInput * forwardThrust * Time.deltaTime * transform.up;
        Move(direction);
    }

    public void Move(Vector3 direction)
    {
        rigidbody.AddForce(direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kill the player if they collide with an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            OnPlayerTakesDamage?.Invoke();
        }
    }
}
using UnityEngine;

/// <summary>
/// Base class for the Asteroid objects.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IPooledObject, IMoveable, IShootable, IScoreGiver
{
    #region Variables
    protected AsteroidSpawner asteroidSpawner;

    // Each asteroid size has a different speed/score set in inspector
    [SerializeField] private float speed = 1f;
    [SerializeField] private int score = 100;

    private Rigidbody2D rigidbody;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        // Ignore collision with other Asteroids (layer 8 = "Asteroids")
        Physics2D.IgnoreLayerCollision(8, 8);
    }

    /// <summary>
    /// Initialises a reference to the 'AsteroidSpawner' class
    /// </summary>
    public void InitialiseAsteroid(AsteroidSpawner spawner)
    {
        asteroidSpawner = spawner;
    }
    #endregion Initialisation

    /// <summary>
    /// Called when spawned from the object pool
    /// </summary>
    public void OnObjectSpawn()
    {
        // Randomise the movement direction of the asteroid to introduce variety
        float rotation = Random.Range(0f, 361f);
        float currentRotation = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, currentRotation + rotation));
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        // Normalize velocity to maintain constant speed
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
        rigidbody.AddForce(transform.up * speed, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage();
        }
    }

    public virtual void TakeDamage()
    {
        GiveScore();
    }

    public void GiveScore()
    {
        GameScoreUpdater.Instance.UpdateScore(score);
    }
}
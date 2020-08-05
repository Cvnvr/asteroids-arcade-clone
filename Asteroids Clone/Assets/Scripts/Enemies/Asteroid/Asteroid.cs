using System;
using UnityEngine;

/// <summary>
/// Base class for the Asteroid objects.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IPooledObject, IMoveable, IDamageable, IScoreGiver
{
    #region Variables
    protected AsteroidSpawner asteroidSpawner;
    private Rigidbody2D rigidbody;

    [SerializeField] protected EnemyData asteroidData;
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
        float rotation = UnityEngine.Random.Range(0f, 361f);
        float currentRotation = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, currentRotation + rotation));
    }

    private void FixedUpdate()
    {
        // Normalize velocity to maintain constant speed
        rigidbody.velocity = rigidbody.velocity.normalized * asteroidData.MovementSpeed;
        var direction = transform.up * asteroidData.MovementSpeed;

        Move(direction);
    }

    public void Move(Vector3 direction)
    {
        rigidbody.AddForce(direction, ForceMode2D.Force);
    }

    public virtual void TakeDamage()
    {
        GiveScore(asteroidData.Score);
    }

    public void GiveScore(int score)
    {
        GameEvents.Instance.ProvideScore(score);
    }
}
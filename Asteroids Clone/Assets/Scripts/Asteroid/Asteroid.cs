using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IPooledObject, IMoveable, IShootable, IEnemy, IScoreGiver
{
    #region Variables
    protected AsteroidSpawner asteroidSpawner;

    private Rigidbody2D rigidbody;
    [SerializeField] private float speed = 1f;

    [SerializeField] private int score = 100;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        // Ignore collision with other Asteroids (layer 8 = "Asteroids")
        Physics2D.IgnoreLayerCollision(8, 8);
    }

    public void InitialiseAsteroid(AsteroidSpawner spawner)
    {
        asteroidSpawner = spawner;
    }
    #endregion Initialisation

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer();
        }
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
        // TODO add particle effects

        GiveScore();
    }

    public void DamagePlayer()
    {
        PlayerLifeHandler.Instance.TakeDamage();
    }

    public void GiveScore()
    {
        ScoreUpdater.Instance.UpdateScore(score);
    }
}
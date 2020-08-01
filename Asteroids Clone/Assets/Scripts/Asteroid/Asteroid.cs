using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IPooledObject, IMoveable, IShootable, IEnemy, IScoreGiver
{
    #region Variables
    private Rigidbody2D rigidbody;

    [Header("Movement Variables")]
    [SerializeField] private float speed = 1f;
    private Vector2 movementDirection;

    [Header("Score")]
    [SerializeField] private int score = 100;

    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        // Ignore collision with other Asteroids
        Physics2D.IgnoreLayerCollision(8, 8);
    }

    public void OnObjectSpawn()
    {
        // Randomise the movement direction of the asteroid to introduce variety
        movementDirection = new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
    }
    #endregion Initialisation

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()
    {
        // Normalize velocity to maintain constant speed
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
        rigidbody.AddForce(movementDirection, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer();
        }
    }

    public virtual void TakeDamage()
    {
        // TODO add particle effects
        // TODO play sound effect

        GiveScore();
    }

    public virtual void DamagePlayer()
    {
        // TODO update lives, etc

        // TODO destroy player
    }

    public virtual void GiveScore()
    {
        // throw new System.NotImplementedException();
    }
}
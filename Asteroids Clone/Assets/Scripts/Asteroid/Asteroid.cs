﻿using UnityEngine;

public class Asteroid : MonoBehaviour, IPooledObject, IMoveable, IShootable, IEnemy, IScoreGiver
{
    #region Variables
    private ScreenBoundsHandler screenBoundsHandler;

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
        ValidateRigidbody();

        // Ignore collisions with other asteroids (8 = 'Asteroids')
        Physics2D.IgnoreLayerCollision(8, 8);

        #region Local Functions
        void ValidateRigidbody()
        {
            if (GetComponent<Rigidbody2D>() == null)
            {
                gameObject.AddComponent(typeof(Rigidbody2D));
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }

            rigidbody = GetComponent<Rigidbody2D>();
        }
        #endregion Local Functions
    }

    private void Start()
    {
        // Cache singleton reference as it's called in Update repeatedly
        screenBoundsHandler = ScreenBoundsHandler.Instance;
    }

    public void OnObjectSpawn()
    {
        // Randomise the movement direction of the asteroid to introduce variety
        movementDirection = new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
    }
    #endregion Initialisation

    private void Update()
    {
        KeepObjectWithinScreenBounds();
    }

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

    private void KeepObjectWithinScreenBounds()
    {
        if (screenBoundsHandler == null)
        {
            Debug.LogWarning($"[WARNING] The object {this.gameObject.name} cannot access the Screen Bounds Handler.");
            return;
        }

        Vector2 newPosition = transform.position;

        if (transform.position.x > screenBoundsHandler.LeftSide)
        {
            newPosition.x = screenBoundsHandler.RightSide;
        }

        if (transform.position.x < screenBoundsHandler.RightSide)
        {
            newPosition.x = screenBoundsHandler.LeftSide;
        }

        if (transform.position.y > screenBoundsHandler.TopSide)
        {
            newPosition.y = screenBoundsHandler.BottomSide;
        }

        if (transform.position.y < screenBoundsHandler.BottomSide)
        {
            newPosition.y = screenBoundsHandler.TopSide;
        }

        // Set new inverted position
        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
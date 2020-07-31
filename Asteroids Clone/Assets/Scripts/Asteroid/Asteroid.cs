using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IMoveable, IShootable, IEnemy, IScoreGiver
{
    #region Variables
    private Rigidbody2D rigidbody;
   
    [Header("Movement Variables")]
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float forceMultiplier = 1f;
    private Vector2 movementDirection;

    // Screen size references
    private Vector2 screenBounds;
    private readonly float screenPadding = 1.25f;

    [Header("Score")]
    [SerializeField] protected int score = 100;
    #endregion

    #region Initialisation
    private void Awake()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent(typeof(Rigidbody2D));
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }

        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Determine screen bounds for updating asteroid position
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, Screen.height, Camera.main.transform.position.z));

        movementDirection = new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
    }
    #endregion

    private void Update()
    {
        KeepObjectWithinScreenBounds();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void KeepObjectWithinScreenBounds()
    {
        Vector2 newPosition = transform.position;

        // beyond LEFT of screen
        if (transform.position.x > (screenBounds.x * screenPadding))
        {
            newPosition.x = -screenBounds.x;
        }
        // beyond RIGHT of screen
        if (transform.position.x < (-screenBounds.x * screenPadding))
        {
            newPosition.x = screenBounds.x;
        }
        // beyond TOP of screen
        if (transform.position.y > (screenBounds.y * screenPadding))
        {
            newPosition.y = -screenBounds.y;
        }
        // beyond BOTTOM of screen
        if (transform.position.y < (-screenBounds.y * screenPadding))
        {
            newPosition.y = screenBounds.y;
        }

        // Set new inverted position
        transform.position = newPosition;
    }

    public virtual void Move()
    {
        // Normalize velocity to maintain constant speed
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
        rigidbody.AddForce(movementDirection, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer();
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            rigidbody.velocity *= -1;
        }
    }

    public virtual void DamagePlayer()
    {
        throw new System.NotImplementedException();
    }

    public virtual void TakeDamage()
    {
        GiveScore();

        // TODO add particle effects
        // TODO play sound effect

        DestroyImmediate(this.gameObject);
    }

    public virtual void GiveScore()
    {
        throw new System.NotImplementedException();
    }
}
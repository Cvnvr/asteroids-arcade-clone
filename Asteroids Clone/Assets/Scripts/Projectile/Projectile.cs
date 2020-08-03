using UnityEngine;

/// <summary>
/// Attached to the projectile prefab.
/// Handles movement of the projectile and destroying itself.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, IPooledObject
{
    #region Variables
    private Rigidbody2D rigidbody;

    [SerializeField] private AudioSource fireSound;

    [SerializeField] private float lifeSpan = 1f;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    #endregion Initialisation

    /// <summary>
    /// Called by the projectile pooler when the projectile is retrieved from the pool
    /// </summary>
    public void OnObjectSpawn()
    {
        // Start countdown until projectile should destroy itself
        Invoke("DestroySelf", lifeSpan);

        fireSound.Play();
    }

    private void DestroySelf()
    {
        // Returns itself to the pool
        ProjectilePooler.Instance.ReturnToPool(ProjectilePooler.Instance.DefaultProjectileTag, this);
    }

    /// <summary>
    /// Called by the ProjectileSpawner to apply the desired velocity
    /// </summary>
    public void Fire(Vector2 velocity)
    {
        rigidbody.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle the projectile destroying itself if it collides with an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            DestroySelf();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

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

    public void OnObjectSpawn()
    {
        Invoke("DestroySelf", lifeSpan);

        fireSound.Play();
    }
    #endregion Initialisation

    private void DestroySelf()
    {
        ProjectilePooler.Instance.ReturnToPool(ProjectilePooler.Instance.DefaultProjectileTag, this);
    }

    public void Fire(Vector2 velocity)
    {
        rigidbody.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            DestroySelf();
        }
    }
}
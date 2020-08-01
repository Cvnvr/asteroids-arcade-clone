using System;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    #region Variables
    private ProjectilePooler pool;
    private Transform nozzle;

    [SerializeField] private float fireDelay = 0.5f;
    private float fireElapsedTime = 0f;

    [SerializeField] private float projectileSpeed = 10f;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        SetNozzleRefference();

        #region Local Functions
        void SetNozzleRefference()
        {
            if (transform.GetChild(0) == null)
            {
                nozzle = this.transform;
            }

            nozzle = transform.GetChild(0);
        }
        #endregion Local Functions
    }

    private void Start()
    {
        pool = ProjectilePooler.Instance;
    }
    #endregion Initialisation

    private void Update()
    {
        if (PlayerInputHandler.IsFiringProjectiles())
        {
            fireElapsedTime += Time.deltaTime;

            if (fireElapsedTime >= fireDelay)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        // Instantiate projectile from pool
        Projectile projectile = pool.SpawnFromPool(pool.DefaultProjectileTag, nozzle.position, nozzle.rotation);

        if (projectile != null)
        {
            projectile.Fire(nozzle.up * projectileSpeed);
        }

        // Reset fire elapsed tiem
        fireElapsedTime = 0;
    }
}
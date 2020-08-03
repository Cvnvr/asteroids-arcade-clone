using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    #region Variables
    private ProjectilePooler pool;

    private Transform nozzle;

    [SerializeField] private float projectileSpeed = 10f;

    [SerializeField] private float fireDelay = 0.5f;
    private float fireElapsedTime = 0f;

    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        AssignNozzleReference();

        #region Local Functions
        void AssignNozzleReference()
        {
            // If a nozzle hasn't been created, just assign this object to act as the nozzle
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

            // Delay the fire rate of the projectiles
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
            // Apply desired velocity to the projectile
            projectile.Fire(nozzle.up * projectileSpeed);
        }

        // Reset fire elapsed tiem
        fireElapsedTime = 0;
    }
}
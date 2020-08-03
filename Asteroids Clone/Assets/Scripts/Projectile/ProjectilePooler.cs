using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the pooling of the projectiles.
/// </summary>
public class ProjectilePooler : Singleton<ProjectilePooler>
{
    /// <summary>
    /// Exposed class to allow for different types of projectiles.
    /// </summary>
    [System.Serializable]
    public class ProjectilePool
    {
        public string tag;
        public Projectile prefab;
        public Transform parent;
        public int size;
    }

    #region Variables
    [SerializeField] private ProjectilePool defaultProjectile;

    private Dictionary<string, Queue<Projectile>> poolDictionary;

    public string DefaultProjectileTag { get => defaultProjectile.tag; }
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<Projectile>>();

        // Initialise projectile pool
        poolDictionary.Add(defaultProjectile.tag, InitailisePoolQueue(defaultProjectile));
    }

    /// <summary>
    /// Initialises the queue of each pool by spawning the provided pool prefab
    ///     the specified number of times and immediately turning each object off.
    /// Queue is then added to the dictionary
    /// </summary>
    private Queue<Projectile> InitailisePoolQueue(ProjectilePool pool)
    {
        Queue<Projectile> objectPool = new Queue<Projectile>();

        // Validates whether a prefab/parent transform were added
        if (pool.prefab == null || pool.parent == null)
        {
            return objectPool;
        }

        for (int i = 0; i < pool.size; i++)
        {
            Projectile obj = Instantiate(pool.prefab, pool.parent, true);
            obj.gameObject.SetActive(false);

            objectPool.Enqueue(obj);
        }

        return objectPool;
    }
    #endregion Initialisation

    public Projectile SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Projectile pool with tag {tag} doesn't exist");
            return null;
        }

        Projectile spawnedProjectile = poolDictionary[tag].Dequeue();

        // Initialise spawned projectile
        spawnedProjectile.gameObject.SetActive(true);
        spawnedProjectile.transform.position = position;
        spawnedProjectile.transform.rotation = rotation;

        // Call the 'Start() substitute function on the pool object
        spawnedProjectile.OnObjectSpawn();

        return spawnedProjectile;
    }

    public void ReturnToPool(string tag, Projectile projectile)
    {
        projectile.gameObject.SetActive(false);

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Projectile pool with tag {tag} doesn't exist");
            return;
        }

        // Enqueue the projectile so it can be reused
        poolDictionary[tag].Enqueue(projectile);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePooler : Singleton<ProjectilePooler>
{
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
    public string DefaultProjectileTag { get => defaultProjectile.tag; }

    private Dictionary<string, Queue<Projectile>> poolDictionary;

    // TODO update this
    private Color playerColor = Color.yellow;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<Projectile>>();

        // Initialise projectile pool
        poolDictionary.Add(defaultProjectile.tag, InitailisePoolQueue(defaultProjectile));
    }

    private Queue<Projectile> InitailisePoolQueue(ProjectilePool pool)
    {
        Queue<Projectile> objectPool = new Queue<Projectile>();

        for (int i = 0; i < pool.size; i++)
        {
            Projectile obj = Instantiate(pool.prefab, pool.parent, true);
            obj.gameObject.SetActive(false);

            obj.GetComponent<SpriteRenderer>().color = playerColor;

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

        poolDictionary[tag].Enqueue(projectile);
    }
}
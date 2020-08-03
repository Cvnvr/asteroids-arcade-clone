using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for pooling the asteroids
/// </summary>
[RequireComponent(typeof(AsteroidSpawner))]
public class AsteroidPooler : MonoBehaviour
{
    /// <summary>
    /// Serializable so users can define new asteroid types in future.
    /// </summary>
    [System.Serializable]
    public class AsteroidPool
    {
        public string tag;
        public Asteroid prefab;
        public Transform parent;
        public List<Sprite> sprites;
        public int size;
    }

    #region Variables
    private AsteroidSpawner asteroidSpawner;

    // Default asteroid types
    [SerializeField] private AsteroidPool largePool;
    [SerializeField] private AsteroidPool mediumPool;
    [SerializeField] private AsteroidPool smallPool;

    // This dictionary contains each of the sets of pooled asteroids after being instantiated
    private Dictionary<string, Queue<Asteroid>> poolDictionary;

    // Properties for convenience
    public string LargePoolTag { get => largePool.tag; }
    public Transform LargeParent { get => largePool.parent; }

    public string MediumPoolTag { get => mediumPool.tag; }
    public Transform MediumParent { get => mediumPool.parent; }

    public string SmallPoolTag { get => smallPool.tag; }
    public Transform SmallParent { get => smallPool.parent; }
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        asteroidSpawner = GetComponent<AsteroidSpawner>();

        poolDictionary = new Dictionary<string, Queue<Asteroid>>();

        // Initialise each of the asteroid size pools
        poolDictionary.Add(largePool.tag, InitialisePoolQueue(largePool));
        poolDictionary.Add(mediumPool.tag, InitialisePoolQueue(mediumPool));
        poolDictionary.Add(smallPool.tag, InitialisePoolQueue(smallPool));
    }

    /// <summary>
    /// Initialises the queue of each pool by spawning the provided pool prefab
    ///     the specified number of times and immediately turning each object off.
    /// Queue is then added to the dictionary
    /// </summary>
    private Queue<Asteroid> InitialisePoolQueue(AsteroidPool pool)
    {
        Queue<Asteroid> objectPool = new Queue<Asteroid>();

        // Validates whether a prefab/parent transform were added
        if (pool.prefab == null || pool.parent == null)
        {
            return objectPool;
        }

        // Iterate over the specified size of the queue
        for (int i = 0; i < pool.size; i++)
        {
            // Instantiate the object and add it to the queue
            Asteroid obj = Instantiate(pool.prefab, pool.parent, true);
            obj.gameObject.SetActive(false);

            obj.GetComponent<Asteroid>().InitialiseAsteroid(asteroidSpawner);

            if (pool.sprites.Count == 0)
            {
                obj.GetComponent<SpriteRenderer>().sprite = pool.sprites[GetRandomSpriteIndex()];
            }

            objectPool.Enqueue(obj);
        }

        return objectPool;

        #region Local Functions
        int GetRandomSpriteIndex()
        {
            return Random.Range(0, pool.sprites.Count - 1);
        }
        #endregion Local Functions
    }
    #endregion Initialisation

    public Asteroid SpawnFromPool(string tag, Vector3 position, Color32 color)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Asteroid pool with tag {tag} doesn't exist");
            return null;
        }

        Asteroid spawnedAsteroid = poolDictionary[tag].Dequeue();

        // Initialise spawned Asteroid
        spawnedAsteroid.gameObject.SetActive(true);
        spawnedAsteroid.transform.localPosition = position;
        spawnedAsteroid.GetComponent<SpriteRenderer>().color = color;

        // Call the 'Start()' substitute function on the pooled object
        spawnedAsteroid.OnObjectSpawn();

        return spawnedAsteroid;
    }

    public void ReturnToPool(string tag, Asteroid asteroid)
    {
        // Disable the object once again
        asteroid.gameObject.SetActive(false);

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Asteroid pool with tag {tag} doesn't exist");
            return;
        }

        // Enqueue it so it can be reused in the future
        poolDictionary[tag].Enqueue(asteroid);
    }

    /// <summary>
    /// Called when the player loses.
    /// Returns all currently spawned objects back to the pool.
    /// </summary>
    public void ReturnAllObjectsToPool()
    {
        for (int i = 0; i < largePool.parent.childCount; i++)
        {
            ReturnToPool(largePool.tag, largePool.parent.GetChild(i).GetComponent<Asteroid>());
        }

        for (int i = 0; i < mediumPool.parent.childCount; i++)
        {
            ReturnToPool(mediumPool.tag, mediumPool.parent.GetChild(i).GetComponent<Asteroid>());
        }

        for (int i = 0; i < smallPool.parent.childCount; i++)
        {
            ReturnToPool(smallPool.tag, smallPool.parent.GetChild(i).GetComponent<Asteroid>());
        }
    }
}
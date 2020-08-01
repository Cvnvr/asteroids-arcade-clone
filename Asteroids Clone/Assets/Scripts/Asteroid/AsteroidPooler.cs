using System.Collections.Generic;
using UnityEngine;

public class AsteroidPooler : MonoBehaviour
{
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
    [SerializeField] private AsteroidPool largePool;
    [SerializeField] private AsteroidPool mediumPool;
    [SerializeField] private AsteroidPool smallPool;

    public string LargePoolTag { get => largePool.tag; }
    public Transform LargeParent { get => largePool.parent; }

    public string MediumPoolTag { get => mediumPool.tag; }
    public Transform MediumParent { get => mediumPool.parent; }

    public string SmallPoolTag { get => smallPool.tag; }
    public Transform SmallParent { get => smallPool.parent; }

    private Dictionary<string, Queue<Asteroid>> poolDictionary;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<Asteroid>>();

        // Initialise each of the asteroid size pools
        poolDictionary.Add(largePool.tag, InitialisePoolQueue(largePool));
        poolDictionary.Add(mediumPool.tag, InitialisePoolQueue(mediumPool));
        poolDictionary.Add(smallPool.tag, InitialisePoolQueue(smallPool));
    }

    private Queue<Asteroid> InitialisePoolQueue(AsteroidPool pool)
    {
        Queue<Asteroid> objectPool = new Queue<Asteroid>();

        // Iterate over the specified size of the queue
        for (int i = 0; i < pool.size; i++)
        {
            // Instantiate the object and add it to the queue
            Asteroid obj = Instantiate(pool.prefab, pool.parent, true);
            obj.gameObject.SetActive(false);

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
        asteroid.gameObject.SetActive(false);

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Asteroid pool with tag {tag} doesn't exist");
            return;
        }

        poolDictionary[tag].Enqueue(asteroid);
    }
}
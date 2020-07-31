using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    #region Variables
    private static AsteroidSpawner _instance;
    public static AsteroidSpawner Instance { get => _instance; }
    
    [Header("Astroid References")]
    [SerializeField] private GameObject largeAsteroidPrefab;
    [SerializeField] private Transform largeAsteroidParent;
    [Space, Space]
    [SerializeField] private GameObject mediumAsteroidPrefab;
    [SerializeField] private Transform mediumAsteroidParent;
    [Space, Space]
    [SerializeField] private GameObject smallAsteroidPrefab;
    [SerializeField] private Transform smallAsteroidParent;

    [Header("Spawner Preferences")]
    [SerializeField] private List<Transform> spawnerLocations;
    private readonly int largeAsteroidSpawnCount = 5;
    private readonly int asteroidSplitCount = 2;

    private Color32 asteroidLevelColor;
    #endregion

    #region Initialisation
    private void Awake()
    {
        GenerateInstance();

        #region Local Functions
        void GenerateInstance()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        } 
        #endregion
    } 
    #endregion

    private void Start()
    {
        InstantiateLargeAstroids();
    }

    private void InstantiateLargeAstroids()
    {
        // Ensure all astroids have been destroyed before instantiating the next set
        ClearAllAstroids();

        // Randomise the colour of the astroids each level
        asteroidLevelColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        int[] spawnIndecies = GetSpawnLocations();
        for (int i = 0; i < largeAsteroidSpawnCount; i++)
        {
            InstantiateAsteroid(largeAsteroidPrefab, largeAsteroidParent,
                spawnerLocations[spawnIndecies[i]].localPosition);
        }

        #region Local Functions
        int[] GetSpawnLocations()
        {
            int[] indecies = new int[largeAsteroidSpawnCount];

            for (int i = 0; i < indecies.Length; i++)
            {
                int spawnIndex = Random.Range(0, spawnerLocations.Count - 1);

                // Only produce unique values if there are enough spawn locations, otherwise just accept the index
                if (indecies.Length < spawnerLocations.Count)
                {
                    // Keep generating an index until it's unique
                    while (indecies.Contains(spawnIndex))
                    {
                        spawnIndex = Random.Range(0, spawnerLocations.Count - 1);
                    }
                }

                indecies[i] = spawnIndex;
            }

            return indecies;
        } 
        #endregion
    }

    private void ClearAllAstroids()
    {
        ClearAllChildren(largeAsteroidParent);
        ClearAllChildren(mediumAsteroidParent);
        ClearAllChildren(smallAsteroidParent);

        #region Local Functions
        void ClearAllChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        } 
        #endregion
    }

    public void SplitLargeAsteroid(Vector2 position)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            InstantiateAsteroid(mediumAsteroidPrefab, mediumAsteroidParent, position);
        }
    }

    public void SplitMediumAsteroid(Vector2 position)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            InstantiateAsteroid(smallAsteroidPrefab, smallAsteroidParent, position);
        }
    }

    private void InstantiateAsteroid(GameObject prefab, Transform parent, Vector2 position)
    {
        GameObject astroid = Instantiate(prefab, parent, true);
        astroid.transform.localPosition = position;

        astroid.GetComponent<SpriteRenderer>().color = asteroidLevelColor;
    }

    public void ValidateRemainingAsteroids()
    {
        if (AreAllAsteroidsDestroyed())
        {
            // TODO increment level
        }

        #region Local Functions
        bool AreAllAsteroidsDestroyed()
        {
            if ((largeAsteroidParent.childCount == 0) && (mediumAsteroidParent.childCount == 0)
                && (smallAsteroidParent.childCount == 0))
            {
                return true;
            }

            return false;
        } 
        #endregion
    }
}

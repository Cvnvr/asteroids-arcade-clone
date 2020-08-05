using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles instantiating large asteroids at the start of each level and splitting asteroids
///     when destroyed by the player.
/// </summary>
[RequireComponent(typeof(AsteroidPooler))]
public class AsteroidSpawner : MonoBehaviour
{
    #region Variables
    [Header("Script References")]
    private AsteroidPooler asteroidPooler;
    [SerializeField] private LevelManager levelManager;

    [Header("Spawn Locations")]
    [SerializeField] private List<Transform> spawnerLocations;

    private int largeAsteroidSpawnCount;
    private int defaultAsteroidSpawnCount = 5;

    private int asteroidSplitCount = 2;

    [Header("Explosion SFX")]
    [SerializeField] private AudioSource explosion;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        asteroidPooler = GetComponent<AsteroidPooler>();
    }

    private void Start()
    {
        // Initialise the spawn count variable
        largeAsteroidSpawnCount = defaultAsteroidSpawnCount;
    }
    #endregion Initialisation

    #region Event Subscription
    private void OnEnable()
    {
        GameStateHandler.OnSetGameOverState += DisableAllAsteroids;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetGameOverState -= DisableAllAsteroids;
    }
    #endregion Event Subscription

    /// <summary>
    /// Called each new level.
    /// Retrieves a new set of large asteroids from the asteroid pool.
    /// </summary>
    public void EnableNewBatchOfLargeAsteroids()
    {
        int[] spawnIndecies = GetSpawnLocations();
        for (int i = 0; i < largeAsteroidSpawnCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.LargePoolTag,
                spawnerLocations[spawnIndecies[i]].localPosition);
        }

        #region Local Functions
        // Gets as unique as possible spawn locations from the list of spawns provided
        int[] GetSpawnLocations()
        {
            int[] indecies = new int[largeAsteroidSpawnCount];

            for (int i = 0; i < indecies.Length; i++)
            {
                // Generate a random index
                int spawnIndex = Random.Range(0, spawnerLocations.Count - 1);

                // Only produce unique values if there are enough spawn locations, otherwise just accept the index
                if (i < spawnerLocations.Count)
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
        #endregion Local Functions
    }

    /// <summary>
    /// Splits the large asteroid provided into n number of medium asteroids
    /// </summary>
    public void SplitLargeAsteroid(LargeAsteroid asteroid)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.MediumPoolTag, asteroid.transform.position);
        }

        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.LargePoolTag, asteroid);

        explosion.Play();
    }

    /// <summary>
    /// Splits the medium asteroid provided into n number of small asteroids
    /// </summary>
    public void SplitMediumAsteroid(MediumAsteroid asteroid)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.SmallPoolTag, asteroid.transform.position);
        }

        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.MediumPoolTag, asteroid);

        explosion.Play();
    }

    /// <summary>
    /// Destroys the small asteroid and validates how many are remaining this level
    /// </summary>
    public void DestroySmallAsteroid(SmallAsteroid asteroid)
    {
        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.SmallPoolTag, asteroid);

        explosion.Play();

        ValidateRemainingAsteroids();
    }

    private void ValidateRemainingAsteroids()
    {
        if (AllAsteroidsHaveBeenDestroyed())
        {
            levelManager.GoToNextLevel();
        }
    }

    private bool AllAsteroidsHaveBeenDestroyed()
    {
        int largeActiveCount = ReturnActiveCount(asteroidPooler.LargeParent);
        int mediumActiveCount = ReturnActiveCount(asteroidPooler.MediumParent);
        int smallActiveCount = ReturnActiveCount(asteroidPooler.SmallParent);

        if ((largeActiveCount == 0) && (mediumActiveCount == 0) && (smallActiveCount == 0))
        {
            return true;
        }

        return false;

        #region Local Functions
        int ReturnActiveCount(Transform parent)
        {
            int activeCount = 0;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).gameObject.activeInHierarchy)
                {
                    activeCount++;
                }
            }

            return activeCount;
        }
        #endregion Local Functions
    }

    /// <summary>
    /// Called each by the level manager when the game gets harder
    /// </summary>
    public void IncreaseNumberOfAsteroids()
    {
        largeAsteroidSpawnCount++;
    }

    /// <summary>
    /// Called when the player loses all of their lives
    /// </summary>
    public void DisableAllAsteroids()
    {
        asteroidPooler.ReturnAllObjectsToPool();

        // Reset asteroid spawn count back to default
        largeAsteroidSpawnCount = defaultAsteroidSpawnCount;
    }
}
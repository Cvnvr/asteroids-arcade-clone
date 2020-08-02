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
    private AsteroidPooler asteroidPooler;

    [Header("Script References")]
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

    public void EnableNewBatchOfLargeAsteroids()
    {
        int[] spawnIndecies = GetSpawnLocations();
        for (int i = 0; i < largeAsteroidSpawnCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.LargePoolTag,
                spawnerLocations[spawnIndecies[i]].localPosition, levelManager.LevelColor);
        }

        #region Local Functions
        int[] GetSpawnLocations()
        {
            int[] indecies = new int[largeAsteroidSpawnCount];

            for (int i = 0; i < indecies.Length; i++)
            {
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

    public void SplitLargeAsteroid(Asteroid asteroid)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.MediumPoolTag, asteroid.transform.position, levelManager.LevelColor);
        }

        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.LargePoolTag, asteroid);

        explosion.Play();
    }

    public void SplitMediumAsteroid(Asteroid asteroid)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.SmallPoolTag, asteroid.transform.position, levelManager.LevelColor);
        }

        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.MediumPoolTag, asteroid);

        explosion.Play();
    }

    public void DestroySmallAsteroid(Asteroid asteroid)
    {
        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.SmallPoolTag, asteroid);

        explosion.Play();

        ValidateRemainingAsteroids();
    }

    private void ValidateRemainingAsteroids()
    {
        if (AreAsteroidsStillThere())
        {
            levelManager.GoToNextLevel();
        }
    }

    private bool AreAsteroidsStillThere()
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

    public void DisableAllAsteroids()
    {
        asteroidPooler.ReturnAllObjectsToPool();

        // Reset asteroid spawn count back to default
        largeAsteroidSpawnCount = defaultAsteroidSpawnCount;
    }

    public void IncreaseNumberOfAsteroids()
    {
        largeAsteroidSpawnCount++;
    }
}
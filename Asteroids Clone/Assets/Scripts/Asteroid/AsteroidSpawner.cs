using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles instantiating large asteroids at the start of each level and splitting asteroids
///     when destroyed by the player.
/// </summary>
[RequireComponent(typeof(AsteroidPooler))]
public class AsteroidSpawner : Singleton<AsteroidSpawner>
{
    #region Variables
    private AsteroidPooler asteroidPooler;

    [SerializeField] private List<Transform> spawnerLocations;

    private readonly int largeAsteroidSpawnCount = 5;
    private readonly int asteroidSplitCount = 2;

    private Color32 asteroidLevelColor;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        asteroidPooler = GetComponent<AsteroidPooler>();
    }

    private void Start()
    {
        // TODO delete this
        EnableNewBatchOfLargeAsteroids();
    }
    #endregion Initialisation

    private void EnableNewBatchOfLargeAsteroids()
    {
        // Randomise the colour of the astroids each level
        asteroidLevelColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        int[] spawnIndecies = GetSpawnLocations();
        for (int i = 0; i < largeAsteroidSpawnCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.LargePoolTag,
                spawnerLocations[spawnIndecies[i]].localPosition, asteroidLevelColor);
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
        #endregion Local Functions
    }

    public void SplitLargeAsteroid(Asteroid asteroid)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.MediumPoolTag, asteroid.transform.position, asteroidLevelColor);
        }

        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.LargePoolTag, asteroid);
    }

    public void SplitMediumAsteroid(Asteroid asteroid)
    {
        for (int i = 0; i < asteroidSplitCount; i++)
        {
            asteroidPooler.SpawnFromPool(asteroidPooler.SmallPoolTag, asteroid.transform.position, asteroidLevelColor);
        }

        // Return 'destroyed' asteroid to the pool
        asteroidPooler.ReturnToPool(asteroidPooler.MediumPoolTag, asteroid);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    #region Variables
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
    #endregion

    private void Start()
    {
        InstantiateLargeAstroids();
    }

    private void InstantiateLargeAstroids()
    {
        // Ensure all astroids have been destroyed before instantiating the next set
        ClearAllAstroids();

        int[] spawnIndecies = GetSpawnLocations();
        for (int i = 0; i < largeAsteroidSpawnCount; i++)
        {
            GameObject astroid = Instantiate(largeAsteroidPrefab, largeAsteroidParent, true);
            astroid.transform.localPosition = spawnerLocations[spawnIndecies[i]].transform.localPosition;

            // TODO set colour based on level
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
}

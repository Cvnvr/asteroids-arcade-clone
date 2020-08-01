using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    #region Variables
    [SerializeField] private GameUiHandler uiHandler;

    private int currentLevel = 1;

    public Color32 LevelColor { get; private set; }
    #endregion Variables

    private void Start()
    {
        InitialiseFirstLevelState();
    }

    public void InitialiseFirstLevelState()
    {
        currentLevel = 0;
        GoToNextLevel();
    }

    public void GoToNextLevel()
    {
        currentLevel++;
        uiHandler.UpdateLevel(currentLevel);

        // Randomise the colour of the astroids each level
        LevelColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        // Spawn next set of asteroids
        AsteroidSpawner.Instance.EnableNewBatchOfLargeAsteroids();
    }
}
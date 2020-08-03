using TMPro;
using UnityEngine;

/// <summary>
/// Handles the active level and transitioning to next one
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Variables
    [Header("Script References")]
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private LevelInformation levelInformation;

    public Color32 LevelColor { get => levelInformation.LevelColor; }

    [SerializeField] private TMP_Text levelLabel;
    #endregion Variables

    #region Event Subscriptions
    private void OnEnable()
    {
        GameStateHandler.OnSetGameState += InitialiseFirstLevelState;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetGameState -= InitialiseFirstLevelState;
    }
    #endregion Event Subscriptions

    /// <summary>
    /// Resets the current level to zero to then fire 'Go To Next Level' so the player starts at level 1
    /// </summary>
    public void InitialiseFirstLevelState()
    {
        levelInformation.CurrentLevel = 0;
        GoToNextLevel();
    }

    /// <summary>
    /// Transitions to the next level and handles triggering the reset of asteroids
    /// </summary>
    public void GoToNextLevel()
    {
        levelInformation.CurrentLevel++;

        IncreaseDifficultyOfLevel();

        GenerateNewRandomLevelColor();

        asteroidSpawner.EnableNewBatchOfLargeAsteroids();

        levelLabel.text = levelInformation.CurrentLevel.ToString();

        #region Local Functions
        void GenerateNewRandomLevelColor()
        {
            // Randomise the colour of the astroids each level
            if (levelInformation.CurrentLevel == 1)
            {
                // Just return new random colour if level 1
                levelInformation.LevelColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
            else
            {
                // Generate a distinct colour each subsequent round
                levelInformation.LevelColor = ColorGenerator.GenerateDistinctRandomColor(levelInformation.LevelColor);
            }
        }
        #endregion Local Functions
    }

    private void IncreaseDifficultyOfLevel()
    {
        // Increase the number of spawned large asteroids every 2 levels
        if (levelInformation.CurrentLevel % 2 == 0)
        {
            asteroidSpawner.IncreaseNumberOfAsteroids();
        }
    }
}
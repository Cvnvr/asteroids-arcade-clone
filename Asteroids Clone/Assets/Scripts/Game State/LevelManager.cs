using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private LevelInformation levelInformation;

    public delegate void OnLevelChangeHandler();
    public static event OnLevelChangeHandler OnLevelChange;

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

    public void InitialiseFirstLevelState()
    {
        levelInformation.CurrentLevel = 0;
        GoToNextLevel();
    }

    public void GoToNextLevel()
    {
        // Increment the current level
        levelInformation.CurrentLevel++;

        DetermineIfDifficultyShouldIncrease();

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

        // Spawn next set of asteroids
        asteroidSpawner.EnableNewBatchOfLargeAsteroids();

        // Update in-game level label
        levelLabel.text = levelInformation.CurrentLevel.ToString();
    }

    private void DetermineIfDifficultyShouldIncrease()
    {
        // Increase the number of spawned large asteroids every 2 levels
        if (levelInformation.CurrentLevel % 2 == 0)
        {
            asteroidSpawner.IncreaseNumberOfAsteroids();
        }
    }
}
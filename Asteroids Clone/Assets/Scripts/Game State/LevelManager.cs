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
        levelInformation.CurrentLevel++;

        // Randomise the colour of the astroids each level
        levelInformation.LevelColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        // Spawn next set of asteroids
        asteroidSpawner.EnableNewBatchOfLargeAsteroids();

        levelLabel.text = levelInformation.CurrentLevel.ToString();
    }
}
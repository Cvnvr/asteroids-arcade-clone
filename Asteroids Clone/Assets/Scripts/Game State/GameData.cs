using UnityEngine;

/// <summary>
/// Used to contain key game data which needs to be accessed in multiple places
/// </summary>
public static class GameData
{
    #region Variables
    public static int CurrentLives { get; private set; }
    public static int CurrentLevel { get; private set; }
    public static int CurrentScore { get; private set; }
    public static Color LevelColor { get; private set; }

    private static int defaultLives = 3;
    private static int defaultLevel = 0;
    private static int defaultScore = 0;
    #endregion Variables

    /// <summary>
    /// Resets the current data to initial state.
    /// </summary>
    public static void ResetGameData()
    {
        CurrentLives = defaultLives;
        CurrentLevel = defaultLevel;
        CurrentScore = defaultScore;
    }

    #region Level
    public static void IncrementLevel()
    {
        CurrentLevel++;
    }
    #endregion Level

    #region Player Lives
    public static void AddPlayerLife()
    {
        CurrentLives++;
    }

    public static void RemovePlayerLife()
    {
        CurrentLives--;
    }
    #endregion Player Lives

    #region Score
    public static void AddToScore(int score)
    {
        CurrentScore += score;
    }
    #endregion Score

    #region Level Color
    public static void GenerateInitialColor()
    {
        LevelColor = ColorGenerator.GenerateNewRandomColor();
    }

    public static void GenerateNewDistinctColor()
    {
        LevelColor = ColorGenerator.GenerateDistinctRandomColor(LevelColor);
    }
    #endregion Level Color
}
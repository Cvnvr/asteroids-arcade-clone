using System;

public class GameEvents : Singleton<GameEvents>
{
    #region Variables
    public static event Action<int> OnScoreGiven;
    #endregion Variables

    public void ProvideScore(int score)
    {
        OnScoreGiven?.Invoke(score);
    }
}
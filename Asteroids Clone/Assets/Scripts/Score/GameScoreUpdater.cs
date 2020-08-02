using TMPro;
using UnityEngine;

public class GameScoreUpdater : Singleton<GameScoreUpdater>
{
    #region Variables
    [SerializeField] private LevelInformation levelInformation;

    [SerializeField] private TMP_Text scoreLabel;
    #endregion Variables

    #region Event Subscribing
    private void OnEnable()
    {
        GameStateHandler.OnSetGameState += InitialiseScoreState;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetGameState -= InitialiseScoreState;
    }
    #endregion Event Subscribing

    public void InitialiseScoreState()
    {
        levelInformation.CurrentScore = 0;
        scoreLabel.text = levelInformation.CurrentScore.ToString();
    }

    public void UpdateScore(int score)
    {
        levelInformation.CurrentScore += score;
        scoreLabel.text = levelInformation.CurrentScore.ToString();
    }
}
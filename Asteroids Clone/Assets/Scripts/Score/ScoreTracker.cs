using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class ScoreTracker : Singleton<ScoreTracker>
{
    #region Variables
    [SerializeField] private GameUiHandler uiHandler;

    private int currentScore;
    #endregion Variables

    private void Start()
    {
        InitialiseScoreState();
    }

    public void InitialiseScoreState()
    {
        currentScore = 0;

        UpdateScoreLabel();
    }

    public void UpdateScore(int score)
    {
        currentScore += score;

        UpdateScoreLabel();
    }

    private void UpdateScoreLabel() => uiHandler.UpdateScore(currentScore);
}
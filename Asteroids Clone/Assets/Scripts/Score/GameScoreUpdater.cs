using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles the player's score
/// </summary>
[RequireComponent(typeof(ScoreDataHandler))]
public class GameScoreUpdater : MonoBehaviour
{
    #region Variables
    private ScoreDataHandler dataHandler;
    private GameStateHandler gameStateHandler;

    [Header("In Game Score UI")]
    [SerializeField] private TMP_Text scoreLabel;

    [Header("Game Over Input UI")]
    [SerializeField] private TMP_InputField tagInputField;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        dataHandler = GetComponent<ScoreDataHandler>();
    }

    private void Start()
    {
        gameStateHandler = GameStateHandler.Instance;
    }
    #endregion Initialisation

    #region Event Subscription
    private void OnEnable()
    {
        GameStateHandler.OnSetGameState += InitialiseScoreState;
        GameEvents.OnScoreGiven += UpdateScore;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetGameState -= InitialiseScoreState;
        GameEvents.OnScoreGiven -= UpdateScore;
    }
    #endregion Event Subscription

    public void InitialiseScoreState()
    {
        // Reset the current score
        scoreLabel.text = GameData.CurrentScore.ToString();

        // Ensure the tag input field is cleared each game
        tagInputField.text = "";
    }

    /// <summary>
    /// Called each time an asteroid is destroyed to append it's given score
    /// </summary>
    private void UpdateScore(int score)
    {
        GameData.AddToScore(score);
        scoreLabel.text = GameData.CurrentScore.ToString();
    }

    /// <summary>
    /// Determines if the current player's score beat one of the existing high scores.
    /// </summary>
    public bool ValidateHighScore()
    {
        // If no high scores exist, automatically add the player's score
        if (dataHandler.highScores.scores.Count < 10)
        {
            return true;
        }

        // Iterate over existing high scores to determine if the current value is greater
        //     than any of the existing scores
        for (int i = 0; i < dataHandler.highScores.scores.Count; i++)
        {
            if (GameData.CurrentScore > dataHandler.highScores.scores[i].score)
            {
                return true;
            }
        }

        // If it reaches this point, the player didn't beat a high score
        return false;
    }

    #region New High Score Buttons
    public void ReturnToMenu()
    {
        gameStateHandler.SetGameState(GameState.MainMenu);
    }

    /// <summary>
    /// Called by the 'Confirm' button in the UI.
    /// Initialises a new Score() object and adds it to the existing high scores.
    /// Returns to the main menu.
    /// </summary>
    public void ConfirmNewHighScore()
    {
        // Validate whether a tag was entered
        string inputtedTag;
        if (string.IsNullOrEmpty(tagInputField.text))
        {
            inputtedTag = "tag";
        }
        else
        {
            inputtedTag = tagInputField.text;
        }

        // Initialise a new high score and add it to the list of existing ones
        Score newHighScore = new Score(GameData.CurrentScore, inputtedTag);
        dataHandler.AddToHighscore(newHighScore);

        ReturnToMenu();
    }
    #endregion New High Score Buttons
}
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.XR;

[RequireComponent(typeof(ScoreDataHandler))]
public class GameScoreUpdater : Singleton<GameScoreUpdater>
{
    #region Variables
    private ScoreDataHandler dataHandler;
    private GameStateHandler gameStateHandler;

    [SerializeField] private LevelInformation levelInformation;
    [SerializeField] private TMP_Text scoreLabel;

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

        tagInputField.text = "";
    }

    public void UpdateScore(int score)
    {
        levelInformation.CurrentScore += score;
        scoreLabel.text = levelInformation.CurrentScore.ToString();
    }

    public bool ValidateHighScore()
    {
        // If no high scores exist, the player automatically gets added
        if (dataHandler.highScores.scores.Count < 10)
        {
            return true;
        }

        // Iterate over existing high scores to determine if the current value is greater
        // than any
        for (int i = 0; i < dataHandler.highScores.scores.Count; i++)
        {
            if (levelInformation.CurrentScore > dataHandler.highScores.scores[i].score)
            {
                return true;
            }
        }

        return false;
    }

    #region New High Score Buttons
    public void ReturnToMenu()
    {
        gameStateHandler.SetGameState(GameState.MainMenu);
    }

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
        Score newHighScore = new Score(levelInformation.CurrentScore, inputtedTag);
        dataHandler.AddToHighscore(newHighScore);

        ReturnToMenu();
    }
    #endregion New High Score Buttons
}
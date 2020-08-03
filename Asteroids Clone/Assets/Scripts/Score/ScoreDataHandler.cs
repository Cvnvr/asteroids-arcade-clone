using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Handles the reading/writing of score data to the json file
/// </summary>
[RequireComponent(typeof(MenuScoreUpdater))]
public class ScoreDataHandler : MonoBehaviour
{
    #region Variables
    private MenuScoreUpdater menuScoreUpdater;

    private readonly string fileName = "High Scores";
    private readonly string fileExtension = ".json";
    private string TotalPath { get => $"{Application.streamingAssetsPath}/{fileName}{fileExtension}"; }

    // Object list of high scores managed throughout the game
    public ScoreList highScores { get; private set; }
    private readonly int maxHighscoreCount = 10;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        menuScoreUpdater = GetComponent<MenuScoreUpdater>();
    }

    private async void Start()
    {
        // Ensure the data reading finishes before trying to access the object list
        await ReadScoreData();

        SortHighScoresByValue();

        // Update the score UI
        menuScoreUpdater.UpdateHighScoresUI();
    }

    private async Task ReadScoreData()
    {
        // Create json file if it doesn't already exist
        if (!File.Exists(TotalPath))
        {
            WriteToFile();
            return;
        }

        // Read from high score data from json file
        using (StreamReader reader = new StreamReader(TotalPath))
        {
            string jsonString = reader.ReadToEnd();

            if (string.IsNullOrEmpty(jsonString))
            {
                highScores = new ScoreList();
                return;
            }

            // Deserialize json data
            highScores = JsonUtility.FromJson<ScoreList>(jsonString);
        }
    }
    #endregion Initialisation

    /// <summary>
    /// Called if the player has a new high score when they lose the game.
    /// </summary>
    public void AddToHighscore(Score score)
    {
        // Add the score to the object list
        highScores.scores.Add(score);

        // Sort it in desc value
        SortHighScoresByValue();

        // Remove values at the bottom that exceed the max count
        TrimNumberOfHighScores();

        // Write the new high scores back to the file
        WriteToFile();

        // Update the UI again
        menuScoreUpdater.UpdateHighScoresUI();

        #region Local Functions
        void TrimNumberOfHighScores()
        {
            // Only keep track of a certain amount of scores
            if (highScores.scores.Count > maxHighscoreCount)
            {
                for (int i = highScores.scores.Count - 1; i >= maxHighscoreCount; i--)
                {
                    highScores.scores.RemoveAt(i);
                }
            }
        }
        #endregion Local Functions
    }

    private void SortHighScoresByValue()
    {
        if (highScores.scores.Count == 0)
        {
            return;
        }

        // Sorts the high scores according to their 'score' in descending value
        highScores.scores = highScores.scores.OrderByDescending(item => item.score).ToList();
    }

    // Writes the object data back to the json file
    private void WriteToFile()
    {
        string jsonString = JsonUtility.ToJson(highScores, true);

        File.WriteAllText(TotalPath, jsonString);
    }
}
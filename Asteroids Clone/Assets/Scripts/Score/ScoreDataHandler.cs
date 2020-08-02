using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MenuScoreUpdater))]
public class ScoreDataHandler : MonoBehaviour
{
    #region Variables
    private MenuScoreUpdater menuScoreUpdater;

    private readonly string fileName = "High Scores";
    private readonly string fileExtension = ".json";
    private string TotalPath { get => $"{Application.streamingAssetsPath}/{fileName}{fileExtension}"; }

    [HideInInspector] public ScoreList highScores;
    private readonly int maxHighscoreCount = 10;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        menuScoreUpdater = GetComponent<MenuScoreUpdater>();
    }

    private async void Start()
    {
        await ReadScoreData();

        SortHighScoresByValue();

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

    public void AddToHighscore(Score score)
    {
        highScores.scores.Add(score);
        SortHighScoresByValue();

        TrimNumberOfHighScores();
        WriteToFile();

        menuScoreUpdater.UpdateHighScoresUI();

        #region Local Functions
        void TrimNumberOfHighScores()
        {
            // Only keep track of a certain amount of scores
            if (highScores.scores.Count > maxHighscoreCount)
            {
                for (int i = highScores.scores.Count; i > maxHighscoreCount; i--)
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

        highScores.scores = highScores.scores.OrderByDescending(item => item.score).ToList();
    }

    private void WriteToFile()
    {
        string jsonString = JsonUtility.ToJson(highScores, true);

        File.WriteAllText(TotalPath, jsonString);
    }
}
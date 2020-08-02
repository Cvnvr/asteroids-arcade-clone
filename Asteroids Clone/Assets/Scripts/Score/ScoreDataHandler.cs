using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScoreDataHandler : MonoBehaviour
{
    #region Variables
    private readonly string fileName = "High Scores";
    private readonly string fileExtension = ".json";

    private string TotalPath { get => $"{Application.streamingAssetsPath}/{fileName}{fileExtension}"; }

    public List<Score> HighScores { get; private set; }
    private readonly int maxHighscoreCount = 3;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        ReadScoreData();
        SortHighScoresByValue();
    }

    private void ReadScoreData()
    {
        // Create json file if it doesn't already exist
        if (!File.Exists(TotalPath))
        {
            HighScores = new List<Score>();
            WriteToFile();
            return;
        }

        // Read from high score data from json file
        using (StreamReader reader = new StreamReader(TotalPath))
        {
            string jsonString = reader.ReadToEnd();

            HighScores = DeserializeJson(jsonString);
        }
    }

    private List<Score> DeserializeJson(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            return new List<Score>();
        }

        ScoreList scoreList = JsonUtility.FromJson<ScoreList>(jsonString);
        return scoreList.scores;
    }
    #endregion Initialisation

    public void AddToHighscore(Score score)
    {
        HighScores.Add(score);
        SortHighScoresByValue();

        TrimNumberOfHighScores();

        WriteToFile();

        // TODO update ui somewhere with latest high scores

        #region Local Functions
        void TrimNumberOfHighScores()
        {
            // Only keep track of a certain amount of scores
            if (HighScores.Count > maxHighscoreCount)
            {
                for (int i = HighScores.Count; i > maxHighscoreCount; i--)
                {
                    HighScores.RemoveAt(i);
                }
            }
        }
        #endregion Local Functions
    }

    private void SortHighScoresByValue()
    {
        HighScores = HighScores.OrderBy(item => item.highScore).ToList();
    }

    private void WriteToFile()
    {
        string jsonString = JsonUtility.ToJson(HighScores);

        File.WriteAllText(TotalPath, jsonString);
    }
}
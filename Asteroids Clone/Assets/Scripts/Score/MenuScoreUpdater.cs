using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScoreDataHandler))]
public class MenuScoreUpdater : MonoBehaviour
{
    #region Variables
    private ScoreDataHandler dataHandler;

    [SerializeField] private List<ScoreUIElement> scoreUIElements;

    [SerializeField] private GameObject noScoresLabel;
    [SerializeField] private GameObject highScoresContainer;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        dataHandler = GetComponent<ScoreDataHandler>();
    }
    #endregion Initialisation

    public void UpdateHighScoresUI()
    {
        if (dataHandler.highScores.scores.Count == 0)
        {
            DisplayScoreList(false);
            return;
        }

        for (int i = 0; i < scoreUIElements.Count; i++)
        {
            if (i >= dataHandler.highScores.scores.Count)
            {
                scoreUIElements[i].gameObject.SetActive(false);
                continue;
            }

            scoreUIElements[i].SetLabels(dataHandler.highScores.scores[i].score, dataHandler.highScores.scores[i].tag);
            scoreUIElements[i].gameObject.SetActive(true);
        }

        DisplayScoreList(true);

        #region Local Functions
        void DisplayScoreList(bool value)
        {
            noScoresLabel.SetActive(!value);
            highScoresContainer.SetActive(value);
        }
        #endregion Local Functions
    }
}
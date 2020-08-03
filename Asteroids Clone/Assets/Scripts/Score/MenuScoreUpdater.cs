using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles updating the high score UI elements in the main menu.
/// </summary>
[RequireComponent(typeof(ScoreDataHandler))]
public class MenuScoreUpdater : MonoBehaviour
{
    #region Variables
    private ScoreDataHandler dataHandler;

    [Header("UI References")]
    // Score containers in the main menu
    [SerializeField] private List<ScoreUIElement> scoreUIElements;
    [Space, Space]
    // Two different contaiers displayed depending on whether any high scores exist
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
            // Displays the "No high scores" message
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

        // Displays the list of high scores if there is data
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
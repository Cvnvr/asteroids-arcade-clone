using UnityEngine;

public class ActiveMenuStateHandler : MonoBehaviour
{
    #region Variables
    [Header("Title")]
    [SerializeField] private GameObject titleContainer;

    [Header("Screen References")]
    [SerializeField] private GameObject introScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject scoreScreen;

    [Header("Main Menu Containers")]
    [SerializeField] private GameObject menuButtonsContainer;
    [SerializeField] private GameObject highScoresContainer;

    [Header("Score Containers")]
    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private GameObject noNewHighScoreContainer;
    [SerializeField] private GameObject newHighScoreContainer;
    #endregion Variables

    #region Event Subscriptions
    private void OnEnable()
    {
        GameStateHandler.OnSetIntroState += DisplayIntroScreen;
        GameStateHandler.OnSetMenuState += DisplayMainMenu;
        GameStateHandler.OnSetGameState += DisplayGameMenu;
        GameStateHandler.OnSetGameOverState += DisplayScoreMenu;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetIntroState -= DisplayIntroScreen;
        GameStateHandler.OnSetMenuState -= DisplayMainMenu;
        GameStateHandler.OnSetGameState -= DisplayGameMenu;
        GameStateHandler.OnSetGameOverState -= DisplayScoreMenu;
    }
    #endregion Event Subscriptions

    public void DisplayIntroScreen()
    {
        titleContainer.SetActive(true);

        ToggleActiveMenu(introScreen);
    }

    public void DisplayMainMenu()
    {
        titleContainer.SetActive(true);

        ToggleActiveMenu(menuScreen);
        DisplayMenuButtons();
    }

    public void DisplayGameMenu()
    {
        titleContainer.SetActive(false);

        ToggleActiveMenu(gameScreen);
    }

    public void DisplayScoreMenu()
    {
        ToggleActiveMenu(scoreScreen);
        DisplayGameOver();
    }

    private void ToggleActiveMenu(GameObject screen)
    {
        if (introScreen != null)
        {
            introScreen.SetActive(introScreen == screen ? true : false);
        }

        if (menuScreen != null)
        {
            menuScreen.SetActive(menuScreen == screen ? true : false);
        }

        if (gameScreen != null)
        {
            gameScreen.SetActive(gameScreen == screen ? true : false);
        }

        if (scoreScreen != null)
        {
            scoreScreen.SetActive(scoreScreen == screen ? true : false);
        }
    }

    #region Menu Containers
    public void DisplayHighScores()
    {
        menuButtonsContainer.SetActive(false);
        highScoresContainer.SetActive(true);
    }

    public void DisplayMenuButtons()
    {
        menuButtonsContainer.SetActive(true);
        highScoresContainer.SetActive(false);
    }
    #endregion Menu Containers

    #region Game Over Containers
    public void DisplayGameOver()
    {
        gameOverContainer.SetActive(true);
        noNewHighScoreContainer.SetActive(false);
        newHighScoreContainer.SetActive(false);
    }

    public void DisplayNoNewHighScoreContainer()
    {
        gameOverContainer.SetActive(false);
        noNewHighScoreContainer.SetActive(true);
        newHighScoreContainer.SetActive(false);
    }

    public void DisplayNewHighScoreContainer()
    {
        gameOverContainer.SetActive(false);
        noNewHighScoreContainer.SetActive(false);
        newHighScoreContainer.SetActive(true);
    }
    #endregion Game Over Containers
}
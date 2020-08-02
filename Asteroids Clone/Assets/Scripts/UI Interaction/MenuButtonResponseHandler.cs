using UnityEngine;

public class MenuButtonResponseHandler : MonoBehaviour
{
    #region Variables
    private GameStateHandler gameStateHandler;
    [SerializeField] private ActiveMenuStateHandler menuStateHandler;

    [SerializeField] private AudioSource uiInteraction;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        gameStateHandler = GameStateHandler.Instance;
    }
    #endregion Initialisation

    public void Play()
    {
        PlayAudioClip();
        gameStateHandler.SetGameState(GameState.Game);
    }

    public void Score()
    {
        PlayAudioClip();
        menuStateHandler.DisplayHighScores();
    }

    public void Controls()
    {
        PlayAudioClip();
        menuStateHandler.DisplayControls();
    }

    public void BackToMenu()
    {
        PlayAudioClip();
        menuStateHandler.DisplayMenuButtons();
    }

    public void Quit()
    {
        PlayAudioClip();
        Application.Quit();
    }

    private void PlayAudioClip() => uiInteraction.Play();
}
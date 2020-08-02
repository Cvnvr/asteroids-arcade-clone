public enum GameState { Intro, MainMenu, Game, GameOver }

public delegate void OnStateChangeHandler();

public class GameStateHandler : Singleton<GameStateHandler>
{
    #region Variables
    public GameState gameState { get; private set; }

    public static event OnStateChangeHandler OnSetIntroState;
    public static event OnStateChangeHandler OnSetMenuState;
    public static event OnStateChangeHandler OnSetGameState;
    public static event OnStateChangeHandler OnSetGameOverState;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        SetGameState(GameState.Intro);
    }
    #endregion Initialisation

    public void SetGameState(GameState state)
    {
        gameState = state;

        switch (gameState)
        {
            case GameState.Intro:
                OnSetIntroState?.Invoke();
                break;
            case GameState.MainMenu:
                OnSetMenuState?.Invoke();
                break;
            case GameState.Game:
                OnSetGameState?.Invoke();
                break;
            case GameState.GameOver:
                OnSetGameOverState?.Invoke();
                break;
        }
    }
}
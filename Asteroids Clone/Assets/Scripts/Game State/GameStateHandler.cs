public enum GameState { Intro, MainMenu, Game, GameOver }

public delegate void OnStateChangeHandler();

/// <summary>
/// Manages the current state of the game.
/// </summary>
public class GameStateHandler : Singleton<GameStateHandler>
{
    #region Variables
    // Enum to manage the game state
    public GameState gameState { get; private set; }

    // Declared events for each of the different game states
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

    /// <summary>
    /// Fires the corresponding event after the state of the game changes.
    /// </summary>
    public void SetGameState(GameState state)
    {
        gameState = state;

        switch (gameState)
        {
            case GameState.Intro:
                OnSetIntroState.Invoke();
                break;
            case GameState.MainMenu:
                OnSetMenuState.Invoke();
                break;
            case GameState.Game:
                OnSetGameState.Invoke();
                break;
            case GameState.GameOver:
                OnSetGameOverState.Invoke();
                break;
        }
    }
}
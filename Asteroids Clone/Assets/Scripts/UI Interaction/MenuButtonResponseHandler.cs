using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonResponseHandler : MonoBehaviour
{
    #region Variables
    private GameStateHandler gameStateHandler;
    #endregion Variables

    private void Start()
    {
        gameStateHandler = GameStateHandler.Instance;
    }

    public void Play()
    {
        gameStateHandler.SetGameState(GameState.Game);
    }

    public void Score()
    {
    }

    public void Quit()
    {
        Application.Quit();
    }
}
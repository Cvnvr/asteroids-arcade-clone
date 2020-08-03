using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the transition sequence from game -> game over -> highscore?
/// </summary>
public class GameOverSequence : MonoBehaviour
{
    #region Variables
    [Header("Script References")]
    private GameStateHandler gameStateHandler;
    [SerializeField] private ActiveMenuStateHandler menuStateHandler;

    [SerializeField] private CanvasGroup containerCG;

    [SerializeField] private AudioSource gameOverSound;

    private Coroutine fadingCoroutine;
    #endregion Variables

    #region Event Subscription
    private void OnEnable()
    {
        GameStateHandler.OnSetGameOverState += DisplayGameOverScreen;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetGameOverState -= DisplayGameOverScreen;

        if (fadingCoroutine != null)
        {
            StopCoroutine(fadingCoroutine);
        }
    }
    #endregion Event Subscription

    /// <summary>
    /// Triggers the fade coroutine
    /// </summary>
    private void DisplayGameOverScreen() => fadingCoroutine = StartCoroutine(StartFadingSequence());

    /// <summary>
    /// Initialises the game over screen, plays the losing audio and then fades in "GAME OVER".
    /// Validates whether the user beat a high score or not to determine which screen to display.
    /// </summary>
    private IEnumerator StartFadingSequence()
    {
        containerCG.alpha = 0;
        gameOverSound.Play();

        yield return new WaitForSeconds(1f);

        // Lerp CanvasGroup on
        yield return StartCoroutine(CanvasGroupFader.FadeCanvas(containerCG, 0, 1, 2.5f));

        yield return new WaitForSeconds(2f);

        // Validate whether the player beat a current high score
        if (GameScoreUpdater.Instance.ValidateHighScore())
        {
            menuStateHandler.DisplayNewHighScoreContainer();
        }
        else
        {
            menuStateHandler.DisplayNoNewHighScoreContainer();
        }
    }
}
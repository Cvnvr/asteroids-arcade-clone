using System.Collections;
using UnityEngine;

public class GameOverSequence : MonoBehaviour
{
    #region Variables
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

    private void DisplayGameOverScreen() => fadingCoroutine = StartCoroutine(StartFadingSequence());

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
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
        StartCoroutine(FadeCanvas(containerCG, 0, 1, 2.5f));

        // todo validate whether score beat a high score
    }

    private IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;
        float elapsedTime = 0f;

        canvas.alpha = startAlpha;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime;
            var percentage = 1 / (duration / elapsedTime);
            if (startAlpha > endAlpha)
            {
                canvas.alpha = startAlpha - percentage;
            }
            else
            {
                canvas.alpha = startAlpha + percentage;
            }

            yield return new WaitForEndOfFrame();
        }
        canvas.alpha = endAlpha;
    }
}
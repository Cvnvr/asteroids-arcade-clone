using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasGroupFader
{
    /// <summary>
    /// Fades a CanvasGroup element from a starting alpha to desired alpha over a set duration.
    /// </summary>
    public static IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float desiredAlpha, float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;
        float elapsedTime = 0f;

        canvas.alpha = startAlpha;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime;
            float percentage = 1 / (duration / elapsedTime);

            if (startAlpha > desiredAlpha)
            {
                canvas.alpha = (startAlpha - percentage);
            }
            else
            {
                canvas.alpha = (startAlpha + percentage);
            }

            yield return new WaitForEndOfFrame();
        }
        canvas.alpha = desiredAlpha;
    }
}
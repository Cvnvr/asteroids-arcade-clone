using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorGenerator
{
    /// <summary>
    /// Returns a new random color
    /// </summary>
    public static Color GenerateNewRandomColor()
    {
        return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    /// <summary>
    /// Returns a color that is distinctly different to the color provided.
    /// </summary>
    public static Color GenerateDistinctRandomColor(Color previousColor)
    {
        // Extract HSV from provided color
        float oldHue, oldSat, oldVal;
        Color.RGBToHSV(previousColor, out oldHue, out oldSat, out oldVal);

        // Generate new random color for comparison
        Color newRandomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        // Extract HSV from newly generated color
        float newHue, newSat, newVal;
        Color.RGBToHSV(newRandomColor, out newHue, out newSat, out newVal);

        // Keep randomising the new color until it is distict enough from the previous color
        while (!AreHueValuesDistinct(oldHue, newHue, 0.2f))
        {
            newRandomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            Color.RGBToHSV(newRandomColor, out newHue, out newSat, out newVal);
        }

        return newRandomColor;

        #region Local Functions
        // Validate whether the difference between hues is greater than the difference provided
        bool AreHueValuesDistinct(float hueOne, float hueTwo, float difference)
        {
            // Check which value is greater to save working with minus numbers
            if (hueOne >= hueTwo)
            {
                if ((hueOne - hueTwo) <= difference)
                {
                    // Return false if the difference between the two values isn't greater
                    return false;
                }
            }
            else
            {
                // If the first value is greater, swap them round
                if ((hueTwo - hueOne) <= difference)
                {
                    // Return false if the difference between the two values isn't greater
                    return false;
                }
            }

            return true;
        }
        #endregion Local Functions
    }
}
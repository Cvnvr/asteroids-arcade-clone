using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    #region Variables
    private ScreenBoundsCalculator screenBoundsHandler;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        screenBoundsHandler = ScreenBoundsCalculator.Instance;
    }
    #endregion Initialisation

    private void Update()
    {
        KeepObjectWithinScreenBounds();
    }

    private void KeepObjectWithinScreenBounds()
    {
        if (screenBoundsHandler == null)
        {
            Debug.LogWarning($"[WARNING] The object {this.gameObject.name} cannot access the Screen Bounds Handler.");
            return;
        }

        Vector2 newPosition = transform.position;

        // beyond LEFT of screen
        if (transform.position.x > screenBoundsHandler.LeftSide)
        {
            newPosition.x = screenBoundsHandler.RightSide;
        }
        // beyond RIGHT of screen
        if (transform.position.x < screenBoundsHandler.RightSide)
        {
            newPosition.x = screenBoundsHandler.LeftSide;
        }
        // beyond TOP of screen
        if (transform.position.y > screenBoundsHandler.TopSide)
        {
            newPosition.y = screenBoundsHandler.BottomSide;
        }
        // beyond BOTTOM of screen
        if (transform.position.y < screenBoundsHandler.BottomSide)
        {
            newPosition.y = screenBoundsHandler.TopSide;
        }

        // Set new inverted position
        transform.position = newPosition;
    }
}
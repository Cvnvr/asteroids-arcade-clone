using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundsHandler : Singleton<ScreenBoundsHandler>
{
    #region Variables
    private Vector2 screenBounds;
    private readonly float screenPadding = 1.15f;

    // References to each side of the screen
    public float LeftSide { get => screenBounds.x * screenPadding; }
    public float RightSide { get => -screenBounds.x * screenPadding; }
    public float TopSide { get => screenBounds.y * screenPadding; }
    public float BottomSide { get => -screenBounds.y * screenPadding; }
    #endregion Variables

    #region Initialisation

    private void Start()
    {
        DefineScreenBounds();

        #region Local Functions
        void DefineScreenBounds()
        {
            // Determine screen bounds for updating asteroid position
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(
                Screen.width, Screen.height, Camera.main.transform.position.z));
        }
        #endregion Local Functions
    }
    #endregion Initialisation
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculates the extents of the screen bounds.
/// </summary>
public class ScreenBoundsCalculator : Singleton<ScreenBoundsCalculator>
{
    #region Variables
    private Vector2 screenBounds;
    [SerializeField] private float screenPadding = 1.15f;

    // References to each side of the screen
    public float LeftSide { get => screenBounds.x * screenPadding; }
    public float RightSide { get => -screenBounds.x * screenPadding; }
    public float TopSide { get => screenBounds.y * screenPadding; }
    public float BottomSide { get => -screenBounds.y * screenPadding; }
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        // Determine screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,
            Screen.height, Camera.main.transform.position.z));
    }
    #endregion Initialisation
}
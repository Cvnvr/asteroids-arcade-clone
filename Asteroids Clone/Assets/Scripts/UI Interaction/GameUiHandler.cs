using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUiHandler : MonoBehaviour
{
    #region Variables
    [Header("Life References")]
    [SerializeField] private List<GameObject> lifeSprites;

    [Header("Level References")]
    [SerializeField] private TMP_Text levelLabel;

    [Header("Score References")]
    [SerializeField] private TMP_Text scoreLabel;
    #endregion Variables

    public void UpdateLifeSprites(int remainingLives)
    {
        // Disable all life sprites
        for (int i = 0; i < lifeSprites.Count; i++)
        {
            lifeSprites[i].SetActive(false);
        }

        if (remainingLives == 0)
        {
            return;
        }

        // Enable remaining lives
        for (int i = 0; i < remainingLives; i++)
        {
            if (i < lifeSprites.Count)
            {
                lifeSprites[i].SetActive(true);
            }
        }
    }

    public void UpdateLevel(int level)
    {
        levelLabel.text = level.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreLabel.text = score.ToString();
    }
}
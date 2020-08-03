using TMPro;
using UnityEngine;

/// <summary>
/// Sits on the score UI containers.
/// Used to initialise the text labels of each of the score UI elements.
/// </summary>
public class ScoreUIElement : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text tagLabel;
    #endregion Variables

    public void SetLabels(int score, string tag)
    {
        scoreLabel.text = score.ToString();
        tagLabel.text = tag;
    }
}
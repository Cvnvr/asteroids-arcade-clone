using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the visual effects of the menu UI buttons when interacted with by the player.
/// </summary>
public class ButtonAppearanceUpdater : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Variables
    private TMP_Text label;
    #endregion Variables

    #region Initialisation
    private void Awake()
    {
        label = GetComponentInChildren<TMP_Text>();
    }
    #endregion Initialisation

    /// <summary>
    /// Reset the button when it's reactivated in case the screen was
    ///     disabled while the button was styled
    /// </summary>
    private void OnEnable()
    {
        label.fontStyle = FontStyles.Bold;
        this.transform.localScale = new Vector3(1f, 1f);
    }

    // HOVER
    public void OnPointerEnter(PointerEventData eventData)
    {
        label.fontStyle = FontStyles.Underline | FontStyles.Bold;
    }

    // LEAVE
    public void OnPointerExit(PointerEventData eventData)
    {
        label.fontStyle ^= FontStyles.Underline;
    }

    // MOUSE DOWN
    public void OnPointerDown(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(0.95f, 0.95f);
    }

    // MOUSE UP
    public void OnPointerUp(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1f, 1f);
    }
}
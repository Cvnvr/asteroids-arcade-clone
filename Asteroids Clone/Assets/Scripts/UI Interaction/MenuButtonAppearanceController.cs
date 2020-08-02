using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonAppearanceController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
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

    private void OnEnable()
    {
        // Reset styles when button is set active again
        label.fontStyle = FontStyles.Bold;
        this.transform.localScale = new Vector3(1f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        label.fontStyle = FontStyles.Underline | FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        label.fontStyle ^= FontStyles.Underline;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(0.95f, 0.95f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1f, 1f);
    }
}
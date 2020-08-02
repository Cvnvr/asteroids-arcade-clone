using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroInteractionHandler : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    private GameStateHandler gameStateHandler;

    [SerializeField] private GameObject introText;

    [SerializeField] private AudioSource uiInteractionSound;

    private Coroutine flashCoroutine;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        gameStateHandler = GameStateHandler.Instance;

        if (flashCoroutine == null)
        {
            FlashIntroScreen();
        }
    }
    #endregion Initialisation

    #region Event Subscriptions
    private void OnEnable()
    {
        introText.SetActive(true);

        GameStateHandler.OnSetIntroState += FlashIntroScreen;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetIntroState -= FlashIntroScreen;

        StopCoroutine(flashCoroutine);
    }
    #endregion Event Subscriptions

    private void FlashIntroScreen() => flashCoroutine = StartCoroutine(FlashScreen());

    private IEnumerator FlashScreen()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            introText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            introText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    #region UI Interaction
    private void Update()
    {
        if (gameStateHandler.gameState == GameState.Intro)
        {
            if (Input.anyKey)
            {
                LoadMainMenu();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        uiInteractionSound.Play();
        gameStateHandler.SetGameState(GameState.MainMenu);
    } 
    #endregion
}
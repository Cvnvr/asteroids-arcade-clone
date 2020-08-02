using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroInteractionHandler : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    private GameStateHandler gameStateHandler;

    [SerializeField] private GameObject introText;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        gameStateHandler = GameStateHandler.Instance;
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
    }
    #endregion Event Subscriptions

    private void Update()
    {
        if (gameStateHandler.gameState == GameState.Intro)
        {
            if (Input.anyKey)
            {
                gameStateHandler.SetGameState(GameState.MainMenu);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameStateHandler.SetGameState(GameState.MainMenu);
    }

    private void FlashIntroScreen() => StartCoroutine(FlashScreen());

    private IEnumerator FlashScreen()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            introText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            introText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
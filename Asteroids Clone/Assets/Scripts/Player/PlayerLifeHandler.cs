using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeHandler : Singleton<PlayerLifeHandler>
{
    #region Variables
    [Header("Script References")]
    private GameStateHandler gameStateHandler;
    [SerializeField] private LevelInformation levelInformation;

    [Header("Player References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerParent;
    private GameObject player;
    [Space, Space]
    [SerializeField] private AudioSource playerExplosion;

    private readonly int maxPlayerLives = 3;
    private readonly float tempInvincibilityTimer = 2f;

    [SerializeField] private List<GameObject> lifeSprites;
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
        GameStateHandler.OnSetGameState += InitialisePlayerState;
    }

    private void OnDisable()
    {
        GameStateHandler.OnSetGameState -= InitialisePlayerState;
    }
    #endregion Event Subscriptions

    public void InitialisePlayerState()
    {
        // Reset player lives back to default
        levelInformation.CurrentLives = maxPlayerLives;
        UpdateLifeSprites();

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerParent, true);

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
    }

    #region Player Death
    public void TakeDamage()
    {
        // Destroy the player
        Destroy(player.gameObject);
        playerExplosion.Play();

        // Update lives counter
        levelInformation.CurrentLives -= 1;
        UpdateLifeSprites();

        if (levelInformation.CurrentLives >= 1)
        {
            // Respawn the player if they still have some lives left
            Invoke("RespawnPlayerAfterDeath", 1f);
        }
        else
        {
            gameStateHandler.SetGameState(GameState.GameOver);
        }
    }

    private void RespawnPlayerAfterDeath()
    {
        SpawnPlayer();

        // Make invincible temporarily
        StartCoroutine(TriggerTemporaryInvincibility());
    }

    private IEnumerator TriggerTemporaryInvincibility()
    {
        TogglePlayerControls(false);
        Coroutine spriteBlinkCoroutine = StartCoroutine(MakeSpriteBlink());

        yield return new WaitForSeconds(tempInvincibilityTimer);

        TogglePlayerControls(true);
        StopCoroutine(spriteBlinkCoroutine);

        // Ensure sprite renderer comes back on
        player.GetComponent<SpriteRenderer>().enabled = true;

        #region Local Functions
        void TogglePlayerControls(bool value)
        {
            player.GetComponent<BoxCollider2D>().enabled = value;
            player.GetComponent<ProjectileSpawner>().enabled = value;
            player.GetComponent<ShipMovementController>().enabled = value;
            player.GetComponent<ShipFlareHandler>().enabled = value;
        }

        IEnumerator MakeSpriteBlink()
        {
            SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();

            if (renderer != null)
            {
                while (true)
                {
                    renderer.enabled = false;
                    yield return new WaitForSeconds(0.15f);
                    renderer.enabled = true;
                    yield return new WaitForSeconds(0.15f);
                }
            }
        }
        #endregion Local Functions
    }
    #endregion Player Death

    private void UpdateLifeSprites()
    {
        // Disable all life sprites
        for (int i = 0; i < lifeSprites.Count; i++)
        {
            lifeSprites[i].SetActive(false);
        }

        if (levelInformation.CurrentLives == 0)
        {
            return;
        }

        // Enable remaining lives
        for (int i = 0; i < levelInformation.CurrentLives; i++)
        {
            if (i < lifeSprites.Count)
            {
                lifeSprites[i].SetActive(true);
            }
        }
    }
}
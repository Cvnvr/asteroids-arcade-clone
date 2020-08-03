using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the player's life/death mechanics
/// </summary>
public class PlayerLifeHandler : MonoBehaviour
{
    #region Variables
    [Header("Script References")]
    private GameStateHandler gameStateHandler;

    [Header("Level Data")]
    [SerializeField] private LevelInformation levelInformation;

    [Header("Player References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerParent;
    [SerializeField] private AudioSource playerExplosion;

    private GameObject player;

    [SerializeField] private List<GameObject> lifeSprites;
    private readonly int maxPlayerLives = 3;

    private readonly float tempInvincibilityTimer = 2f;
    private Coroutine spriteBlinkCoroutine;
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
        // Resets player lives back to default
        levelInformation.CurrentLives = maxPlayerLives;
        UpdateLifeSprites();

        SpawnPlayer();
    }

    /// <summary>
    /// Instantiates the player into the level
    /// </summary>
    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerParent, true);

        // Initialise the player's position and rotation to the center of the game
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;

        // Subscribe to the player damage event
        player.GetComponent<ShipMovementController>().onPlayerTakeDamage += TakeDamage;
    }

    #region Player Death
    /// <summary>
    /// Called via event when the player collides with an asteroid
    /// </summary>
    public void TakeDamage()
    {
        // Unsubscribe to the event before the player is destroyed
        player.GetComponent<ShipMovementController>().onPlayerTakeDamage -= TakeDamage;

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

    /// <summary>
    /// Re-instantiates the player
    /// </summary>
    private void RespawnPlayerAfterDeath()
    {
        SpawnPlayer();

        // Make invincible temporarily
        StartCoroutine(TriggerTemporaryInvincibility());
    }

    /// <summary>
    /// Provides the player with temporary invincibility after spawning
    /// </summary>
    private IEnumerator TriggerTemporaryInvincibility()
    {
        // Disable player functionality while invicible
        TogglePlayerControls(false);
        spriteBlinkCoroutine = StartCoroutine(MakeSpriteBlink());

        yield return new WaitForSeconds(tempInvincibilityTimer);

        // Re-enable player functionality after invicibility ends
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

    /// <summary>
    /// Updates the UI life sprites with the corresponding life count
    /// </summary>
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
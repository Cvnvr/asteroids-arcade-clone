using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeHandler : Singleton<PlayerLifeHandler>
{
    #region Variables
    [Header("Script References")]
    [SerializeField] private GameUiHandler uiHandler;

    [Header("Player References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerParent;
    private GameObject player;
    [Space, Space]
    [SerializeField] private AudioSource playerExplosion;

    private readonly int maxPlayerLives = 3;
    private int currentPlayerLives;

    private readonly float tempInvincibilityTimer = 2f;
    #endregion Variables

    private void Start()
    {
        InitialisePlayerState();
    }

    public void InitialisePlayerState()
    {
        // Reset player lives back to default
        currentPlayerLives = maxPlayerLives;
        uiHandler.UpdateLifeSprites(currentPlayerLives);

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
        currentPlayerLives -= 1;

        if (currentPlayerLives >= 1)
        {
            // Respawn the player if they still have some lives left
            Invoke("RespawnPlayerAfterDeath", 1f);
        }
        else
        {
            // todo game over
            // todo play game over sound with menu
        }

        uiHandler.UpdateLifeSprites(currentPlayerLives);
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
}
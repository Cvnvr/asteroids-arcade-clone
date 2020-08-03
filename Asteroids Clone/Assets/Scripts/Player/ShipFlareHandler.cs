using UnityEngine;

/// <summary>
/// Controls the visibility of the flare particle effect
/// </summary>
public class ShipFlareHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private ParticleSystem flare;
    #endregion Variables

    #region Initialisation
    private void Start()
    {
        // Ensure the flare isn't active when the player is spawned
        flare.Stop();
    }
    #endregion Initialisation

    private void Update()
    {
        // Play the particle effect only when the player is moving forward
        if (PlayerInputHandler.GetForwardInput() > 0)
        {
            if (flare.isStopped)
            {
                flare.Play();
            }
        }
        else
        {
            if (flare.isPlaying)
            {
                flare.Stop();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFlareHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private ParticleSystem flare;
    #endregion Variables

    private void Start()
    {
        flare.Stop();
    }

    private void Update()
    {
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
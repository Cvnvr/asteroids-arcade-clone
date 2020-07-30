using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        // TODO split into 2x medium astroids

        base.TakeDamage();
    }
}
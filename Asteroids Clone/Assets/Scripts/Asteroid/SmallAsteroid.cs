using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        asteroidSpawner.DestroySmallAsteroid(this);

        base.TakeDamage();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        // Split large asteroid into specified number of medium sized asteroids
        asteroidSpawner.SplitLargeAsteroid(this);

        base.TakeDamage();
    }
}
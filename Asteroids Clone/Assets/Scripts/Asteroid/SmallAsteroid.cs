using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        // Check how many asteroids are still alive
        AsteroidSpawner.Instance.ValidateRemainingAsteroids();

        base.TakeDamage();
    }
}
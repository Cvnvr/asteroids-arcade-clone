using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        AsteroidSpawner.Instance.DestroySmallAsteroid(this);

        // Validate remaining asteroid's count to know when to start next level
        AsteroidSpawner.Instance.ValidateRemainingAsteroids();

        base.TakeDamage();
    }
}
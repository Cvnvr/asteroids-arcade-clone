using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        // Split medium asteroid into specified number of small sized asteroids
        AsteroidSpawner.Instance.SplitMediumAsteroid(this);

        base.TakeDamage();
    }
}
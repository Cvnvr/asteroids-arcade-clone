using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        AsteroidSpawner.Instance.SplitLargeAsteroid(this.transform.position);

        base.TakeDamage();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        AsteroidSpawner.Instance.SplitMediumAsteroid(this.transform.position);

        base.TakeDamage();
    }
}
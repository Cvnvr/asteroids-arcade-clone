using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        // TODO contact astroid spawner to validate how many small astroids are left

        base.TakeDamage();
    }
}
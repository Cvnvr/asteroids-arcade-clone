public class MediumAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        base.TakeDamage();

        // Splits medium asteroid into specified number of small sized asteroids
        asteroidSpawner.SplitMediumAsteroid(this);
    }
}
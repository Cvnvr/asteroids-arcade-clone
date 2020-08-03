public class LargeAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        base.TakeDamage();

        // Splits large asteroid into specified number of medium sized asteroids
        asteroidSpawner.SplitLargeAsteroid(this);
    }
}
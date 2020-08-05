public class SmallAsteroid : Asteroid
{
    public override void TakeDamage()
    {
        base.TakeDamage();

        // Destorys this asteroid
        asteroidSpawner.DestroySmallAsteroid(this);
    }
}
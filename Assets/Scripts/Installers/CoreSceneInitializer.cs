using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class CoreSceneInitializer : MonoInstaller
    {
        [SerializeField] private WorldController worldController;
        [SerializeField] private Asteroid bigAsteroid;
        [SerializeField] private Asteroid mediumAsteroid;
        [SerializeField] private Asteroid smallAsteroid;
        [SerializeField] private Bullet spaceshipBullet;
        
        private Transform _asteroidsParent;        
        private Transform _bulletsParent;        
        
        public override void InstallBindings()
        {
            _asteroidsParent= new GameObject("AsteroidsParent").transform;
            _bulletsParent = new GameObject("BulletsParent").transform;

            Container.Bind<CoreGameController>().FromNew().AsSingle();
            Container.Bind<WorldController>().FromInstance(worldController);
            Container.BindInterfacesAndSelfTo<AsteroidsSpawner>().FromNew().AsSingle();
            
            Container.BindFactory<Vector2, AsteroidSize, Asteroid, Asteroid.Factory>().WithId(AsteroidSize.Big)
                .FromComponentInNewPrefab(bigAsteroid).UnderTransform(_asteroidsParent);
            Container.BindFactory<Vector2, AsteroidSize, Asteroid, Asteroid.Factory>().WithId(AsteroidSize.Medium)
                .FromComponentInNewPrefab(mediumAsteroid).UnderTransform(_asteroidsParent);
            Container.BindFactory<Vector2, AsteroidSize, Asteroid, Asteroid.Factory>().WithId(AsteroidSize.Small)
                .FromComponentInNewPrefab(smallAsteroid).UnderTransform(_asteroidsParent);

            Container.BindFactory<Vector3, float, Bullet, Bullet.Factory>().FromComponentInNewPrefab(spaceshipBullet)
                .UnderTransform(_bulletsParent);
        }
    }
}
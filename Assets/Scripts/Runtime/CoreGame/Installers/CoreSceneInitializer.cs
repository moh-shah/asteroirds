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
        [SerializeField] private SpaceShipController spaceShipController;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CoreGameController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldController>().FromInstance(worldController);
            Container.BindInterfacesAndSelfTo<AsteroidsSpawner>().FromNew().AsSingle();

         
            Container.BindMemoryPool<Asteroid, Asteroid.Pool>().WithId(AsteroidSize.Big)
                .FromComponentInNewPrefab(bigAsteroid).UnderTransformGroup("BigAsteroidsParent");
            Container.BindMemoryPool<Asteroid, Asteroid.Pool>().WithId(AsteroidSize.Medium)
                .FromComponentInNewPrefab(mediumAsteroid).UnderTransformGroup("MediumAsteroidsParent");
            Container.BindMemoryPool<Asteroid, Asteroid.Pool>().WithId(AsteroidSize.Small)
                .FromComponentInNewPrefab(smallAsteroid).UnderTransformGroup("SmallAsteroidsParent");
            
            Container.BindMemoryPool<Bullet, Bullet.Pool>().WithInitialSize(20)
                .FromComponentInNewPrefab(spaceshipBullet).UnderTransformGroup("BulletsParent");
            
            Container.Bind<SpaceShipController>().FromInstance(spaceShipController);
        }
    }
}
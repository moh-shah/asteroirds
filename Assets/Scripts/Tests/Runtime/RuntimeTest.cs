#if UNITY_EDITOR

using Moq;
using Moshah.Asteroids.Base;
using Moshah.Asteroids.Gameplay;
using Moshah.Asteroids.Models;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Tests.RunTime
{
    [TestFixture]
    public class AsteroidTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Init()
        {
            var mockGameConfig = ScriptableObject.CreateInstance<GameConfig>();
            Container.Bind<GameConfig>().FromInstance(mockGameConfig);

            var mockWorldController = new Mock<IWorldController>();
            Container.Bind<IWorldController>().FromInstance(mockWorldController.Object);

            var mockAudioManager = new Mock<IAudioManager>();
            Container.Bind<IAudioManager>().FromInstance(mockAudioManager.Object);

            var mockAsteroidSpawner = new Mock<IAsteroidsSpawner>();
            Container.Bind<IAsteroidsSpawner>().FromInstance(mockAsteroidSpawner.Object);

            var mockCoregameController = new Mock<ICoreGameController>();
            Container.Bind<ICoreGameController>().FromInstance(mockCoregameController.Object);
        }

        [Test]
        public void AsteroidHp_ShouldBeDecreased_WhenTakeDamageMethodIsCalled()
        {
            var initHp = 10;
            var damageAmount = 2;
            var asteroidObj = new GameObject();
            asteroidObj.AddComponent<Rigidbody2D>();
            var asteroid = Container.InstantiateComponent<Asteroid>(asteroidObj);
            asteroid.Hp = initHp;


            asteroid.TakeDamage(damageAmount);


            Assert.That(asteroid.Hp == initHp - damageAmount);
        }
    }
}
#endif
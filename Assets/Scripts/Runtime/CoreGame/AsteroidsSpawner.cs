using Moshah.Asteroids.Base;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Moshah.Asteroids.Gameplay
{
    public class AsteroidsSpawner : ITickable, IInitializable
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameConfig _gameConfig;
        [Inject] private WorldController _worldController;
        [Inject(Id = AsteroidSize.Big)]
        private Asteroid.Pool _bigAsteroidPool;
        [Inject(Id = AsteroidSize.Medium)]
        private Asteroid.Pool _mediumAsteroidPool;
        [Inject(Id = AsteroidSize.Small)] private Asteroid.Pool _smallAsteroidPool;
        
        private float _waitTime;
        private float _waitTimer;
        private float _asteroidsSpawnedSoFar;
        
        public void Initialize()
        {
            _waitTime = _gameConfig.asteroidSpawnIntervalMax;
            _gameStateManager.OnStateChanged += delegate(GameState state)
            {
                if (state == GameState.MainMenu)
                {
                    foreach (var asteroid in Object.FindObjectsOfType<Asteroid>())
                    {
                        switch (asteroid.Size)
                        {
                            case AsteroidSize.Big:
                                _bigAsteroidPool.Despawn(asteroid);
                                break;
                         
                            case AsteroidSize.Medium:
                                _mediumAsteroidPool.Despawn(asteroid);
                                break;
                     
                            case AsteroidSize.Small:
                                _smallAsteroidPool.Despawn(asteroid);
                                break;
                        }
                    }
                }
            };
        }
        
        public void Tick()
        {
            if (_gameStateManager.CurrentState != GameState.Gameplay) 
                return;
            
            _waitTimer += Time.deltaTime;
            if (_waitTimer < _waitTime)
                return;
            
            _waitTimer = 0;
            SpawnAsteroid(AsteroidSize.Big,new Vector3(_worldController.maxX + 1, _worldController.maxY + 1));
            _asteroidsSpawnedSoFar++;
            if (_asteroidsSpawnedSoFar > _gameConfig.decreaseAsteroidSpawnIntervalAfterSpawningThisAmountOfBigAsteroids)
            {
                if(_waitTime>_gameConfig.asteroidSpawnIntervalMin)
                    _waitTime -= _gameConfig.asteroidSpawnIntervalDecreaseValue;
            }
        }

        public void SpawnAsteroid(AsteroidSize asteroidSize, Vector2 position)
        {
            switch (asteroidSize)
            {
                case AsteroidSize.Big:
                    _bigAsteroidPool.Spawn(position, asteroidSize, _bigAsteroidPool);
                    break;
                
                case AsteroidSize.Medium:
                    _mediumAsteroidPool.Spawn(position, asteroidSize, _bigAsteroidPool);
                    break;
                
                case AsteroidSize.Small:
                    _smallAsteroidPool.Spawn(position, asteroidSize, _bigAsteroidPool);
                    break;
            }
        }
    }
}
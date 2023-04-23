using System;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class AsteroidsSpawner : ITickable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private WorldController _worldController;
        
        [Inject(Id = AsteroidSize.Big)]
        private Asteroid.Pool _bigAsteroidPool;
        [Inject(Id = AsteroidSize.Medium)]
        private Asteroid.Pool _mediumAsteroidPool;
        [Inject(Id = AsteroidSize.Small)]
        private Asteroid.Pool _smallAsteroidPool;
        
        private float _waitTime = 0;
        
        public void Tick()
        {
            _waitTime += Time.deltaTime;
            if (_waitTime < _gameConfig.asteroidSpawnInterval)
                return;
            
            _waitTime = 0;
            SpawnAsteroid(AsteroidSize.Big,new Vector3(_worldController.maxX + 1, _worldController.maxY + 1));
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
using System;
using System.Collections;
using System.Collections.Generic;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Moshah.Asteroids.Gameplay
{
    public class AsteroidsSpawner : ITickable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private WorldController _worldController;
        
        [Inject(Id = AsteroidSize.Big)] private Asteroid.Factory _asteroidFactory;

        private readonly List<Asteroid> _currentAsteroids = new List<Asteroid>();
       
        private float waitTime = 0;
        
        public void Tick()
        {
            waitTime += Time.deltaTime;
            if (waitTime>2)
            {
                waitTime = 0;
                SpawnAsteroid(AsteroidSize.Big,new Vector3(_worldController.maxX + 1, _worldController.maxY + 1));

            }
        }

        public void SpawnAsteroid(AsteroidSize asteroidSize, Vector2 position)
        {
            var asteroid = _asteroidFactory.Create(position, asteroidSize);
            _worldController.RegisterFloatingObject(asteroid);
            _currentAsteroids.Add(asteroid);
        }
    }
}
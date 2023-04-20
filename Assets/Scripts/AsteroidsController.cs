using System;
using System.Collections;
using System.Collections.Generic;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Moshah.Asteroids.Gameplay
{
    public class Asteroid : IFloatingObject
    {
        public Vector2 Position
        {
            get => rigidbody.transform.position;
            set => rigidbody.transform.position = new Vector3(value.x,value.y);
        }
        
        public float Rotation
        {
            get => rigidbody.rotation;
            set => rigidbody.rotation = value;
        }
        public Rigidbody2D rigidbody;
    }

    public class AsteroidsController : MonoBehaviour
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private WorldController _worldController;

        private List<Asteroid> _currentAsteroids = new List<Asteroid>();


        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                SpawnAsteroid();
            }
        }

        private void SpawnAsteroid()
        {
            var asteroidWorldObject = Instantiate<GameObject>(_gameConfig.bigAsteroid);
            asteroidWorldObject.transform.position = new Vector3(_worldController.maxX + 1, _worldController.maxY + 1);
            var newAsteroid = new Asteroid()
            {
                rigidbody = asteroidWorldObject.GetComponent<Rigidbody2D>()
            };
            newAsteroid.rigidbody.AddForce(new Vector2(Random.Range(-100,100),Random.Range(-100,100)));
            
            _worldController.RegisterFloatingObject(newAsteroid);
            _currentAsteroids.Add(newAsteroid);
        }
    }
}
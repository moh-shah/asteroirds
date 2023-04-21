using System;
using DG.Tweening;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Moshah.Asteroids.Gameplay
{
    public enum AsteroidSize
    {
        Undefined,
        Big,
        Medium,
        Small
    }
    public class Asteroid : MonoBehaviour, IFloatingObject, IAttackableEntity
    {
        [SerializeField] private new Rigidbody2D rigidbody;
        
        [Inject] private AsteroidSize _size;
        [Inject] private Vector2 _position;
        [Inject] private GameConfig _gameConfig;
        [Inject] private AsteroidsSpawner _asteroidsSpawner;
        [Inject] private WorldController _worldController;
        
        public int Hp { get; set; }
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x,value.y);
        }
        
        public float Rotation
        {
            get => rigidbody.rotation;
            set => rigidbody.rotation = value;
        }
        
        private void Awake()
        {
            Hp = _gameConfig.GetAsteroidHp(_size);
            transform.position = _position;
            var force = new Vector2(_gameConfig.GetAsteroidVelocity(_size), _gameConfig.GetAsteroidVelocity(_size));
            var rotation = Random.Range(0,360f);
            AddForce(force);
            Rotate(rotation);
        }

        public void Rotate(float angle)
        {
            rigidbody.SetRotation(angle);
        }

        public void AddForce(Vector2 forceVector)
        {
            rigidbody.AddForce(forceVector);
        }


        public void GetDamage(int amount)
        {
            Hp -= amount;
            if (Hp <= 0)
                OnHpReachedZero();
        }

        public void OnHpReachedZero()
        {
            Destroy(gameObject);
            switch (_size)
            {
                case AsteroidSize.Big:
                    _asteroidsSpawner.SpawnAsteroid(AsteroidSize.Medium, transform.position);
                    _asteroidsSpawner.SpawnAsteroid(AsteroidSize.Medium, transform.position);
                    break;
                
                case AsteroidSize.Medium:
                    _asteroidsSpawner.SpawnAsteroid(AsteroidSize.Small, transform.position);
                    _asteroidsSpawner.SpawnAsteroid(AsteroidSize.Small, transform.position);
                    _asteroidsSpawner.SpawnAsteroid(AsteroidSize.Small, transform.position);
                    break;
                
                case AsteroidSize.Small:
                    
                    break;
            }
        }

        private void OnDestroy()
        {
            _worldController.RemoveFloatingObject(this);
        }

        public class Factory : PlaceholderFactory<Vector2,AsteroidSize, Asteroid>
        {
        }
    }
}
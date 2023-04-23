﻿using Moshah.Asteroids.Models;
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
    public class Asteroid : MonoBehaviour, IFloatingEntity, IAttackableEntity, IPoolable<Vector2, AsteroidSize, IMemoryPool>
    {
        [SerializeField] private new Rigidbody2D rigidbody;

        [Inject] private GameConfig _gameConfig;
        [Inject] private AsteroidsSpawner _asteroidsSpawner;
        [Inject] private WorldController _worldController;
        [Inject] private CoreGameController _coreGameController;
        
        private IMemoryPool _pool;
        private AsteroidSize _size;

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
        

        public void Rotate(float angle)
        {
            transform.Rotate(transform.forward, angle);
        }

        public void AddForce(Vector2 forceVector)
        {
            rigidbody.AddForce(forceVector);
        }
        
        public void TakeDamage(int amount)
        {
            Hp -= amount;
            if (Hp <= 0)
                OnHpReachedZero();
        }

        public void OnHpReachedZero()
        {
            _coreGameController.AsteroidDestroyed(_size);
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
            
            _pool.Despawn(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var spaceship = other.GetComponent<SpaceShipController>();
            if (spaceship != null)
                spaceship.TakeDamage(_gameConfig.asteroidDamage);
        }

        public void OnSpawned(Vector2 position, AsteroidSize size,IMemoryPool pool)
        {
            _pool = pool;
            _size = size;
            _worldController.RegisterFloatingObject(this);
            transform.position = position;
            Hp = _gameConfig.GetAsteroidHp(_size);
            Rotate(Random.Range(0,360f));
            AddForce(transform.up.normalized * _gameConfig.GetAsteroidVelocity(_size));
        }
        
        public void OnDespawned()
        {
            _worldController.RemoveFloatingObject(this);
        }

        public class Pool : MonoPoolableMemoryPool<Vector2, AsteroidSize,IMemoryPool, Asteroid>
        {
        }
    }
}
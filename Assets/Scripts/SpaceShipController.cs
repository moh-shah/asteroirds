using System;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class SpaceShipController : MonoBehaviour, IVehicleController, IAttackableEntity
    {
        [SerializeField] private Rigidbody2D spaceshipRigidbody;

        [Inject] private WorldController _worldController;
        [Inject] private GameConfig _gameConfig;
        [Inject] private Bullet.Pool _bulletPool;

        private float _attackIntervalTimer;
        
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x,value.y);
        }
        
        public float Rotation
        {
            get => spaceshipRigidbody.rotation;
            set => spaceshipRigidbody.rotation = value;
        }
        public int Hp { get; set; }

        
        private void Start()
        {
            Hp = _gameConfig.spaceshipHp;
            spaceshipRigidbody.drag = _gameConfig.drag;
            _worldController.RegisterFloatingObject(this);
        }

        private void OnDestroy()
        {
            _worldController.RemoveFloatingObject(this);
        }

        private void Update()
        {
            _attackIntervalTimer += Time.deltaTime;
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKey(_gameConfig.rightRotationKey))
                Rotate(-_gameConfig.rotationAngle);

            if (Input.GetKey(_gameConfig.leftRotationKey))
                Rotate(_gameConfig.rotationAngle);

            if (Input.GetKey(_gameConfig.addForceKey))
                AddForce(_gameConfig.forceVector);
            
            if (Input.GetKeyDown(_gameConfig.shootBulletKey))
                Shoot();
        }

        public void Rotate(float angle)
        {
            spaceshipRigidbody.SetRotation(spaceshipRigidbody.rotation + angle * Time.deltaTime);
        }

        public void AddForce(Vector2 forceVector)
        {
            spaceshipRigidbody.AddRelativeForce(forceVector);
        }

        private void Shoot()
        {
            if (_attackIntervalTimer < _gameConfig.attackInterval)
                return;

            var bullet = _bulletPool.Spawn(transform.position, spaceshipRigidbody.rotation); 
            bullet.AddForce(transform.up.normalized * _gameConfig.bulletVelocity);
            _attackIntervalTimer = 0;
        }

        public void TakeDamage(int amount)
        {
            Hp -= amount;
            if (Hp<=0)
                OnHpReachedZero();
        }

        public void OnHpReachedZero()
        {
            Debug.LogError("spaceship is destroyed");
            _worldController.RemoveFloatingObject(this);
            Destroy(gameObject);
        }
    }
}
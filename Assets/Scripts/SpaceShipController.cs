using System;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class SpaceShipController : MonoBehaviour, IVehicleController
    {
        [SerializeField] private Rigidbody2D spaceshipRigidbody;

        [Inject] private WorldController _worldController;
        [Inject] private GameConfig _gameConfig;
        [Inject] private Bullet.Factory _bulletFactory;

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
        
        private void Start()
        {
            spaceshipRigidbody.drag = _gameConfig.drag;
            _worldController.RegisterFloatingObject(this);
        }

        private void OnDestroy()
        {
            _worldController.RemoveFloatingObject(this);
        }

        private void Update()
        {
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
            var bullet =_bulletFactory.Create(transform.position, spaceshipRigidbody.rotation); 
            bullet.AddForce(transform.up.normalized * _gameConfig.bulletVelocity);
            _worldController.RegisterFloatingObject(bullet);
        }
    }
}
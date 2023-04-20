using System;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{

    public interface IFloatingObject
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
    }

    public class SpaceShipController : MonoBehaviour, IVehicleController
    {
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

        [SerializeField] private Rigidbody2D spaceshipRigidbody;

        [Inject] private WorldController _worldController;
        [Inject] private GameConfig _gameConfig;

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
            if (Input.GetKey(_gameConfig.rightRotationKey))
                Rotate(-_gameConfig.rotationAngle);
            
            if (Input.GetKey(_gameConfig.leftRotationKey))
                Rotate(_gameConfig.rotationAngle);
            
            if (Input.GetKey(_gameConfig.addForceKey))
                AddForce(_gameConfig.forceVector);
        }

        public void Rotate(float angle)
        {
            spaceshipRigidbody.SetRotation(spaceshipRigidbody.rotation + angle * Time.deltaTime);
        }

        public void AddForce(Vector2 forceVector)
        {
            spaceshipRigidbody.AddRelativeForce(forceVector);
        }
    }
}
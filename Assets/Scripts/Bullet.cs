using System;
using System.Collections;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class Bullet : MonoBehaviour , IFloatingObject
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private WorldController _worldController;
        [Inject] private float _rotation;
        [Inject] private Vector3 _position;
        
        [SerializeField] private Rigidbody2D bulletRigidbody;
        
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x,value.y);
        }
        
        public float Rotation { get; set; }

        private void Start()
        {
            transform.position = _position;
            Rotate(_rotation);
            StartCoroutine(Suicide());
        }

        private IEnumerator Suicide()
        {
            yield return new WaitForSeconds(_gameConfig.bulletLifeTime);
            Destroy(gameObject);
        }
        
        public void Rotate(float angle)
        {
            bulletRigidbody.rotation = angle;
        }

        public void AddForce(Vector2 forceVector)
        {
            bulletRigidbody.AddForce(forceVector);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"[OnTriggerEnter]: {other.name}");
            var asteroid = other.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.GetDamage(_gameConfig.bulletDamage);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            _worldController.RemoveFloatingObject(this);
        }

        public class Factory : PlaceholderFactory<Vector3, float, Bullet>
        {
            
        }
    }
}
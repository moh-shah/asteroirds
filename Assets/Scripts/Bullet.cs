using System;
using System.Collections;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class Bullet : MonoBehaviour, IFloatingEntity, IPoolable<Vector3, float>
    {
        [SerializeField] private Rigidbody2D bulletRigidbody;
        
        [Inject] private GameConfig _gameConfig;
        [Inject] private WorldController _worldController;

        [Inject]  private Pool _bulletPool;
       
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x, value.y);
        }

        public float Rotation { get; set; }


        private IEnumerator Suicide()
        {
            yield return new WaitForSeconds(_gameConfig.bulletLifeTime);
            _bulletPool.Despawn(this);
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
            if (gameObject.activeInHierarchy == false)
                return;
            
            var asteroid = other.GetComponent<Asteroid>();
            if (asteroid == null)
                return;
            
            asteroid.TakeDamage(_gameConfig.bulletDamage);
            StopAllCoroutines();
            _bulletPool.Despawn(this);
        }
        
        public void OnSpawned(Vector3 pos, float rot)
        {
            _worldController.RegisterFloatingObject(this);
            transform.position = pos;
            Rotate(rot);
            StartCoroutine(Suicide());
        }
        
        public void OnDespawned()
        {
            _worldController.RemoveFloatingObject(this);
        }

        public class Pool : MonoPoolableMemoryPool<Vector3, float, Bullet>
        {
        }
    }
}
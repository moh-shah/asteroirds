using System;
using System.Collections;
using DG.Tweening;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class SpaceShipController : MonoBehaviour, IAttackableEntity, IFloatingEntity
    {
        public event Action<int> OnHpChanged = delegate(int hp) {  }; 
        
        [SerializeField] private Rigidbody2D spaceshipRigidbody;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Inject] private WorldController _worldController;
        [Inject] private GameConfig _gameConfig;
        [Inject] private Bullet.Pool _bulletPool;

        private float _attackIntervalTimer;
        private bool _isVulnerable;
        
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
        
        private void OnEnable()
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
        
        private void Shoot()
        {
            if (_attackIntervalTimer < _gameConfig.attackInterval)
                return;

            var bullet = _bulletPool.Spawn(transform.position, spaceshipRigidbody.rotation); 
            bullet.AddForce(transform.up.normalized * _gameConfig.bulletVelocity);
            _attackIntervalTimer = 0;
        }

        private IEnumerator MakeMeVulnerable()
        {
            yield return new WaitForSeconds(_gameConfig.invulnerabilityTime);
            _isVulnerable = true;
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
            spaceshipRigidbody.velocity = Vector2.zero;
            spaceshipRigidbody.rotation = 0;
            transform.position = Vector3.zero;
            _isVulnerable = false;
            spriteRenderer.DOFade(0, _gameConfig.invulnerabilityTime / 3f).SetLoops(3, LoopType.Yoyo).onComplete+= () =>
            {
                spriteRenderer.color = Color.white;
            };
            StartCoroutine(MakeMeVulnerable());
        }
        

        public void Rotate(float angle)
        {
            spaceshipRigidbody.SetRotation(spaceshipRigidbody.rotation + angle * Time.deltaTime);
        }

        public void AddForce(Vector2 forceVector)
        {
            spaceshipRigidbody.AddRelativeForce(forceVector);
        }
        
        public void TakeDamage(int amount)
        {
            if (!_isVulnerable)
                return;
            
            Hp -= amount;
            OnHpChanged.Invoke(Hp);
            if (Hp <= 0)
                OnHpReachedZero();
            else
                Spawn();
        }

        public void OnHpReachedZero()
        {
            _worldController.RemoveFloatingObject(this);
            gameObject.SetActive(false);
        }
    }
}
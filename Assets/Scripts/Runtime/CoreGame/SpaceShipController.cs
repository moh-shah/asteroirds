using System;
using System.Collections;
using DG.Tweening;
using Moshah.Asteroids.Base;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShipController : MonoBehaviour, IAttackableEntity, IFloatingEntity
    {
        public event Action<int> OnHpChanged = delegate(int hp) {  };
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Inject] private GameConfig _gameConfig;
        [Inject] private IWorldController _worldController;
        [Inject] private IAudioManager _audioManager;
        [Inject] private Bullet.Pool _bulletPool;

        private float _attackIntervalTimer;
        private bool _isVulnerable;
        private Rigidbody2D _spaceshipRigidbody;
        
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x,value.y);
        }
        
        public float Rotation
        {
            get => _spaceshipRigidbody.rotation;
            set => _spaceshipRigidbody.rotation = value;
        }
        public int Hp { get; set; }
        
        private void OnEnable()
        {
            Hp = _gameConfig.spaceshipHp;
            _spaceshipRigidbody = GetComponent<Rigidbody2D>();
            _spaceshipRigidbody.drag = _gameConfig.drag;
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

            var bullet = _bulletPool.Spawn(transform.position, _spaceshipRigidbody.rotation); 
            bullet.AddForce(transform.up.normalized * _gameConfig.bulletVelocity);
            _attackIntervalTimer = 0;
            _audioManager.PlaySfx(SfxType.Shoot);
        }

        private IEnumerator MakeMeVulnerable()
        {
            yield return new WaitForSeconds(_gameConfig.invulnerabilityTime);
            _isVulnerable = true;
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
            _spaceshipRigidbody.velocity = Vector2.zero;
            _spaceshipRigidbody.rotation = 0;
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
            _spaceshipRigidbody.SetRotation(_spaceshipRigidbody.rotation + angle * Time.deltaTime);
        }

        public void AddForce(Vector2 forceVector)
        {
            _spaceshipRigidbody.AddRelativeForce(forceVector);
        }
        
        public void TakeDamage(int amount)
        {
            if (!_isVulnerable)
                return;
            
            _audioManager.PlaySfx(SfxType.GameOver);
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
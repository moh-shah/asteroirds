using System;
using System.Collections.Generic;
using System.Linq;
using Moshah.Asteroids.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Moshah.Asteroids.Models
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Asteroids/Configs/GameConfigs")]
    public class GameConfig : ScriptableObject
    {
        [Header("world")]
        public float drag;
        public float boundaryThreshold;
        
        [Space]
        [Header("spaceship")]
        public float rotationAngle;
        public int spaceshipHp;
        public Vector2 forceVector;
        public KeyCode rightRotationKey;
        public KeyCode leftRotationKey;
        public KeyCode addForceKey;

        [Space]
        [Header("asteroids")] 
        public List<AsteroidSizeConfig> asteroidSizeConig;
        public int asteroidDamage;
        
        [Space]
        
        public int bulletDamage;
        public float bulletLifeTime;
        public float bulletVelocity;
        public KeyCode shootBulletKey;


        public int GetAsteroidHp(AsteroidSize asteroidSize)
        {
            var definedConfig = asteroidSizeConig.FirstOrDefault(a => a.size == asteroidSize);
            return definedConfig?.hp ?? 1;
        }
        
        public int GetAsteroidVelocity(AsteroidSize asteroidSize)
        {
            var definedConfig = asteroidSizeConig.FirstOrDefault(a => a.size == asteroidSize);
            return definedConfig!=null ? Random.Range(definedConfig.minVelocity, definedConfig.maxVelocity) : 100;
        }

        public int GetAsteroidScoreAfterGettingDestroyed(AsteroidSize asteroidSize)
        {
            var definedConfig = asteroidSizeConig.FirstOrDefault(a => a.size == asteroidSize);
            return definedConfig?.scoreAfterGettingDestroyed ?? 1;
        }
    }

    [Serializable]
    public class AsteroidSizeConfig
    {
        public AsteroidSize size;
        public int hp;
        public int minVelocity;
        public int maxVelocity;
        public int scoreAfterGettingDestroyed;
    }
}
using UnityEngine;

namespace Moshah.Asteroids.Models
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Asteroids/Configs/GameConfigs")]
    public class GameConfig : ScriptableObject
    {
        [Header("world")]
        public float drag;
        
        [Space]
        [Header("spaceship")]
        public float rotationAngle;
        public KeyCode rightRotationKey;
        public KeyCode leftRotationKey;
        
        [Space]
        public Vector2 forceVector;
        public KeyCode addForceKey;
        
        [Space]
        public float bulletSpeed;
        public KeyCode shootBulletKey;
    }
}
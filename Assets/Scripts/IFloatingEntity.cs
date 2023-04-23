using UnityEngine;

namespace Moshah.Asteroids.Gameplay
{
    public interface IFloatingEntity
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        void Rotate(float angle);
        void AddForce(Vector2 forceVector);
    }
}
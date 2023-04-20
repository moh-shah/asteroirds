using UnityEngine;

namespace Moshah.Asteroids.Gameplay
{
    public interface IVehicleController : IFloatingObject
    {
        void Rotate(float angle);
        void AddForce(Vector2 forceVector);
    }
}
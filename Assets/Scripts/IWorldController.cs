namespace Moshah.Asteroids.Gameplay
{
    public interface IWorldController
    {
        void WrapFloatingObjectsPositionsIfOutsideBoundaries();
        void RegisterFloatingObject(IFloatingObject floatingObject);
        void RemoveFloatingObject(IFloatingObject floatingObject);
    }
}
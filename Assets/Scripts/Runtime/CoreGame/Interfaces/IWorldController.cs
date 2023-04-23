namespace Moshah.Asteroids.Gameplay
{
    public interface IWorldController
    {
        void WrapFloatingObjectsPositionsIfOutsideBoundaries();
        void RegisterFloatingObject(IFloatingEntity floatingEntity);
        void RemoveFloatingObject(IFloatingEntity floatingEntity);
    }
}
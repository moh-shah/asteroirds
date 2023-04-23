namespace Moshah.Asteroids.Gameplay
{
    public interface IAttackableEntity
    {
        public int Hp { get; set; }
        void TakeDamage(int amount);
        void OnHpReachedZero();
    }
}
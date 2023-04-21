namespace Moshah.Asteroids.Gameplay
{
    public interface IAttackableEntity
    {
        public int Hp { get; set; }
        void GetDamage(int amount);
        void OnHpReachedZero();
    }
}
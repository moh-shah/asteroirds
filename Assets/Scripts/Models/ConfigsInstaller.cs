using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Models
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Asteroids/ConfigsInstaller")]
    public class ConfigsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameConfig gameConfig;
        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(gameConfig);
        }
    }
}
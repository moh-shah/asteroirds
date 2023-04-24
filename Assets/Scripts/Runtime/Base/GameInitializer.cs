using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Base
{
    //this class can be considered as the starting point of the application
    public class GameInitializer : MonoInstaller
    {
        [SerializeField] private MonoBehaviourHelper monoBehaviourHelper;
        [SerializeField] private AudioManager audioManager;
        public override void InstallBindings()
        {
            Debug.Log($"[{GetType().Name}]: game is getting initialized.");
            
            Container.Bind<MonoBehaviourHelper>().FromInstance(monoBehaviourHelper);
            Container.BindInterfacesAndSelfTo<AudioManager>().FromInstance(audioManager);
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPrefsScoreDataPort>().AsSingle();
            
            MonoUtils.InjectMonoHelper(monoBehaviourHelper);
        }
    }
}
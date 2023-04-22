using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Moshah.Asteroids.Base
{
    //this class can be considered as the starting point of the application
    public class GameInitializer : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerPrefsScoreDataPort>().AsSingle();
        }
    }
}
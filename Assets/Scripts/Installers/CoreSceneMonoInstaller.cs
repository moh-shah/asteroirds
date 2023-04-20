using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class CoreSceneMonoInstaller : MonoInstaller
    {
        [SerializeField] private WorldController worldController;
        public override void InstallBindings()
        {
            Container.Bind<WorldController>().FromInstance(worldController);
        }
    }
}
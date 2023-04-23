using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Base
{
    public enum GameState
    {
        Undefined,
        Initializing,
        MainMenu,
        Gameplay,
        ResultScreen
    }
    
    public class GameStateManager : IInitializable
    {
        public event Action<GameState> OnStateChanged = delegate(GameState currentState) {  };
        public GameState CurrentState { get; private set; } = GameState.Initializing;

        [Inject] private MonoBehaviourHelper _monoBehaviourHelper;

        public void Initialize()
        {
            _monoBehaviourHelper.StartCoroutine(FakeDelayForInitialization()); 
        }

        private IEnumerator FakeDelayForInitialization()
        {
            yield return new WaitForSeconds(2);
            ChangeState(GameState.MainMenu);
        }
        
        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            OnStateChanged.Invoke(CurrentState);
        }
    }
}
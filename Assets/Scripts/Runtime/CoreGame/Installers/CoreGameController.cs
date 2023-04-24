using System;
using Moshah.Asteroids.Base;
using Moshah.Asteroids.Models;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public interface ICoreGameController
    {
        event Action<int, int> OnScoreIncreased;
        int TotalScore { get; set; }
        void AsteroidDestroyed(AsteroidSize asteroidSize);
    }
    public class CoreGameController : IInitializable, ICoreGameController
    {
        public event Action<int, int> OnScoreIncreased = delegate(int prevScore, int newScore) { };
        public int TotalScore
        {
            get => _totalScore;
            set
            {
                var prevScore = _totalScore;
                _totalScore = value;
                OnScoreIncreased.Invoke(prevScore, TotalScore);
            } 
        }
        
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private IScoreDataPort _scoreDataPort;
        [Inject] private GameConfig _gameConfig;
        [Inject] private SpaceShipController _spaceShipController;

        private int _totalScore;
        
        public void Initialize()
        {
            _spaceShipController.gameObject.SetActive(false);
            _gameStateManager.OnStateChanged += OnStateChanged;
            _spaceShipController.OnHpChanged += OnSpaceShipHpChanged;
        }

        public void AsteroidDestroyed(AsteroidSize asteroidSize)
        {
            TotalScore += _gameConfig.GetAsteroidScoreAfterGettingDestroyed(asteroidSize);
        }

        private void OnStateChanged(GameState gameState)
        {
            if (gameState == GameState.Gameplay)
            {
                _spaceShipController.Spawn();
            }
            else
            {
                _spaceShipController.gameObject.SetActive(false);
            }
        }

        private void OnSpaceShipHpChanged(int hp)
        {
            if (hp <= 0)
                DoEndGameStuff();
        }
        
        private void DoEndGameStuff()
        {
            var currentHighScore = _scoreDataPort.GetHighScore();
            if (TotalScore > currentHighScore)
                _scoreDataPort.SubmitHighScore(TotalScore);
            
            _gameStateManager.ChangeState(GameState.ResultScreen);
        }
    }
}
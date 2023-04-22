using System;
using Moshah.Asteroids.Base;
using Moshah.Asteroids.Models;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class CoreGameController
    {
        public event Action<int, int> OnScoreIncreased = delegate(int increasedAmount, int totalScore) { };
        
        [Inject] private IScoreDataPort _scoreDataPort;
        [Inject] private GameConfig _gameConfig;
        
        private int _totalScore;
        
        public void EndGame()
        {
            var currentHighScore = _scoreDataPort.GetHighScore();
            if (_totalScore > currentHighScore)
                _scoreDataPort.SubmitHighScore(_totalScore);
            
            //pause the game
            //show menu
            //
        }

        public void AsteroidDestroyed(AsteroidSize asteroidSize)
        {
            var increasedAmount = _gameConfig.GetAsteroidScoreAfterGettingDestroyed(asteroidSize);
            _totalScore += increasedAmount;
            OnScoreIncreased.Invoke(increasedAmount, _totalScore);
        }
        
    }
}
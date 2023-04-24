using Moshah.Asteroids.Base;
using NUnit.Framework;

namespace Moshah.Asteroids.Tests.Editor
{
    public class PlayerPrefsHighscoreDataPortTests
    {
        private readonly IScoreDataPort _dataPort = new PlayerPrefsScoreDataPort();
        
        [SetUp]
        public void Setup()
        {
            _dataPort.ClearData();
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        public void ShouldSubmitHighscore_OnPlayerPrefs_WhenUsingWriteMethod(int highScore)
        {
            _dataPort.SubmitHighScore(highScore);


            var savedScore = _dataPort.GetHighScore();
            
            
            Assert.That(savedScore == highScore);
        }
    }
}
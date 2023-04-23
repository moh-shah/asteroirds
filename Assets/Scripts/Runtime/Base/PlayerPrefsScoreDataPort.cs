using UnityEngine;

namespace Moshah.Asteroids.Base
{
    public class PlayerPrefsScoreDataPort : IScoreDataPort
    {
        private const string HighScoreKey = "HighScore";
        public void SubmitHighScore(int score)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
        }

        public int GetHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public void ClearData()
        {
            PlayerPrefs.DeleteKey(HighScoreKey);
        }
    }
}
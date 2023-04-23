namespace Moshah.Asteroids.Base
{
    public interface IScoreDataPort
    {
        void SubmitHighScore(int score);
        int GetHighScore();

        void ClearData();
    }
}
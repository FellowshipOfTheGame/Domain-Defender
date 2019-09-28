[System.Serializable]
public class HighScore
{
    public int score;
    public string username;
}

[System.Serializable]
public class HighScores
{
    public HighScore[] highScores;
}
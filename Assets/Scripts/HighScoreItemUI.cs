using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI username, score, placement;

    public void SetScore(HighScore highScore, int placement)
    {
        username.text = highScore.username;
        score.text = highScore.score.ToString();
        this.placement.text = placement.ToString();
    }
}

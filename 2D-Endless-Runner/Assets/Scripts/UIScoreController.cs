using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour
{
    [Header("UI")]
    public Text score;
    public Text highscore;

    [Header("Score")]
    public ScoreController scoreController;

    private void Update()
    {
        score.text = scoreController.GetCurrentScore().ToString();
        highscore.text = ScoreData.highScore.ToString();
    }
}

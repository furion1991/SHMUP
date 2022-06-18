using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreCounter : MonoBehaviour
{
    
    public static int score = 0;
    public Text scoreText;
    public static int[] highScores;
    private void Awake()
    {
        highScores = new int[] { PlayerPrefs.GetInt("HighScore1"),
        PlayerPrefs.GetInt("HighScore2"),
        PlayerPrefs.GetInt("HighScore3")};
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name  == "_Scene_0")
        {
            scoreText.text = $"Your score is: {score}";
        }

        if (SceneManager.GetActiveScene().name == "Scene_HighScores")
        {
            scoreText.text = $"High score 1 is: {highScores[0]}\n" +
                $"High score 2 is: {highScores[1]}\n" +
                $"High score 3 is: {highScores[2]}\n";
        }
        
    }

    public static void SetScoreTable()
    {
        for (int i = 0; i < highScores.Length; i++)
        {
            if (highScores[i] < score)
            {
                if (i == 0)
                {
                    highScores[i + 1] = PlayerPrefs.GetInt($"HighScore{i + 1}");
                    highScores[i + 2] = PlayerPrefs.GetInt($"HighScore{i + 2}");
                    PlayerPrefs.SetInt($"HighScore{i + 1}", score);
                    PlayerPrefs.SetInt($"HighScore{i + 2}", highScores[i + 1]);
                    PlayerPrefs.SetInt($"HighScore{i + 3}", highScores[i + 2]);
                }
                else if (i == 1)
                {
                    highScores[i + 1] = PlayerPrefs.GetInt($"HighScore{i + 1}");
                    PlayerPrefs.SetInt($"HighScore{i + 1}", score);
                    PlayerPrefs.SetInt($"HighScore{i + 2}", highScores[i + 1]);
                }
                else if (i == 2)
                {
                    highScores[i] = score;
                    PlayerPrefs.SetInt($"HighScore{i + 1}", score);
                }
                break;
            }
        }
        
    }
}

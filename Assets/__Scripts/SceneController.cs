using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [Header("Set in inspector")]
    public List<Button> buttons;
    public Hero heroStarting;
    
    [Header("Set dynamically")]
    public bool gameStarting = false;

    public BoundsCheck bndCheck;

    private void Awake()
    {
        if (heroStarting != null)
        {
            bndCheck = heroStarting.GetComponent<BoundsCheck>();
        }
        
        foreach (Button button in buttons)
        {
            Text name = button.GetComponentInChildren<Text>();
            if (name.text == "Start Game")
            {
                button.onClick.AddListener(StartGame);
            }
            if (name.text == "Exit Game")
            {
                button.onClick.AddListener(Application.Quit);
            }
            if (name.text == "High Scores")
            {
                button.onClick.AddListener(GoToHighScoreMenu);
            }
            if (name.text == "Back to menu")
            {
                button.onClick.AddListener(GoBackToMenu);
            }
        }
    }

    private void Update()
    {
        if (heroStarting != null)
        {
            Vector3 pos = heroStarting.transform.position;

            if (gameStarting)
            {
                pos.y += 0.4f;
                heroStarting.transform.position = pos;
                if (bndCheck.offUp)
                {
                    SceneManager.LoadScene("_Scene_0");
                }
            }
        }
        
    }
    public void StartGame()
    {
        gameStarting = true;
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Scene_start");
    }
    public void GoToHighScoreMenu()
    {
        SceneManager.LoadScene ("Scene_HighScores");
    }
    
}

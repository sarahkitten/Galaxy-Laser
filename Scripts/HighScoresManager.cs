using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresManager : MonoBehaviour
{
    int score;
    GameSession gameSession;

    [Header("Text")]
    public Text highScore1;
    public Text highScore2;
    public Text highScore3;
    public Text highScore4;
    public Text highScore5;
    public Text heading;
    public Text scoreText;

    [Header("Effects")]
    [SerializeField] Color32 newHighScoreColor = new Color32(0xFF, 0x2E, 0x00, 0xFF);
    [SerializeField] AudioClip top5Sound;
    [SerializeField] [Range(0,1)] float top5SoundVolume = 0.7f;
    [SerializeField] AudioClip newHighScoreSound;
    [SerializeField] [Range(0,1)] float newHighScoreSoundVolume = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        score = gameSession.GetScore();
        UpdateHighScores(score);
        DisplayHighScores();
    }

    void UpdateHighScores(int score)
    {
        if (score > PlayerPrefs.GetInt("HighScore1", 0))
        {
            PlayerPrefs.SetInt("HighScore5", PlayerPrefs.GetInt("HighScore4", 0));
            PlayerPrefs.SetInt("HighScore4", PlayerPrefs.GetInt("HighScore3", 0));
            PlayerPrefs.SetInt("HighScore3", PlayerPrefs.GetInt("HighScore2", 0));
            PlayerPrefs.SetInt("HighScore2", PlayerPrefs.GetInt("HighScore1", 0));
            PlayerPrefs.SetInt("HighScore1", score);
            NewHighScore(highScore1);
        }
        else if (score > PlayerPrefs.GetInt("HighScore2", 0))
        {
            PlayerPrefs.SetInt("HighScore5", PlayerPrefs.GetInt("HighScore4", 0));
            PlayerPrefs.SetInt("HighScore4", PlayerPrefs.GetInt("HighScore3", 0));
            PlayerPrefs.SetInt("HighScore3", PlayerPrefs.GetInt("HighScore2", 0));
            PlayerPrefs.SetInt("HighScore2", score);
            NewHighScore(highScore2);
        }
        else if (score > PlayerPrefs.GetInt("HighScore3", 0))
        {
            PlayerPrefs.SetInt("HighScore5", PlayerPrefs.GetInt("HighScore4", 0));
            PlayerPrefs.SetInt("HighScore4", PlayerPrefs.GetInt("HighScore3", 0));
            PlayerPrefs.SetInt("HighScore3", score);
            NewHighScore(highScore3);
        }
        else if (score > PlayerPrefs.GetInt("HighScore4", 0))
        {
            PlayerPrefs.SetInt("HighScore5", PlayerPrefs.GetInt("HighScore4", 0));
            PlayerPrefs.SetInt("HighScore4", score);
            NewHighScore(highScore4);
        }
        else if (score > PlayerPrefs.GetInt("HighScore5", 0))
        {
            PlayerPrefs.SetInt("HighScore2", score);
            NewHighScore(highScore5);
        }
    }

    void NewHighScore(Text HighScore)
    { // audio/visual effect
        if (HighScore == highScore1)
        {
            heading.text = "New High Score!";
            AudioSource.PlayClipAtPoint(newHighScoreSound, Camera.main.transform.position, newHighScoreSoundVolume);
        }
        else
        {
            heading.text = "New Top 5!";
            AudioSource.PlayClipAtPoint(top5Sound, Camera.main.transform.position, top5SoundVolume);
        }
        HighScore.color = newHighScoreColor;
        scoreText.color = newHighScoreColor;
    }

    void DisplayHighScores()
    {
        highScore1.text = PlayerPrefs.GetInt("HighScore1", 0).ToString();
        highScore2.text = PlayerPrefs.GetInt("HighScore2", 0).ToString();
        highScore3.text = PlayerPrefs.GetInt("HighScore3", 0).ToString();
        highScore4.text = PlayerPrefs.GetInt("HighScore4", 0).ToString();
        highScore5.text = PlayerPrefs.GetInt("HighScore5", 0).ToString();
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        DisplayHighScores();
    }
}

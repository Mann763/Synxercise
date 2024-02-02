using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    private int score = 0;
    private int highScore = 0;
    private string highScoreKey = "HighScore";

    public delegate void ScoreChangedDelegate(int newScore);
    public static event ScoreChangedDelegate OnScoreChanged;

    public delegate void HighScoreChangedDelegate(int newHighScore);
    public static event HighScoreChangedDelegate OnHighScoreChanged;

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ScoreManager");
                    instance = singletonObject.AddComponent<ScoreManager>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        // Load the high score from PlayerPrefs on start
        LoadHighScore();
    }

    public void AddPoints(int points)
    {
        score += points;

        // Update high score if the current score surpasses it
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
            OnHighScoreChanged?.Invoke(highScore);
        }

        // Notify subscribers that the score has changed
        OnScoreChanged?.Invoke(score);
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
    }

    public void ResetHighScore()
    {
        highScore = 0;
        SaveHighScore();
        OnHighScoreChanged?.Invoke(highScore);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            highScore = PlayerPrefs.GetInt(highScoreKey);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

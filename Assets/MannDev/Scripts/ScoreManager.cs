using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Singleton instance
    private static ScoreManager instance;

    // Player's score
    private int score = 0;

    // Event to notify score changes
    public delegate void ScoreChangedDelegate(int newScore);
    public static event ScoreChangedDelegate OnScoreChanged;

    // Method to get the singleton instance
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                // If no instance exists, try to find one in the scene
                instance = FindObjectOfType<ScoreManager>();

                // If no instance is found in the scene, create a new one
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ScoreManager");
                    instance = singletonObject.AddComponent<ScoreManager>();
                }
            }

            return instance;
        }
    }

    // Method to add points to the score
    public void AddPoints(int points)
    {
        score += points;

        // Notify subscribers that the score has changed
        OnScoreChanged?.Invoke(score);
    }

    // Method to get the current score
    public int GetScore()
    {
        return score;
    }

    // Optional: Reset the score
    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
    }

    // Ensure that the instance is not destroyed when changing scenes
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

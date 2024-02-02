using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiveManager : MonoBehaviour
{
    public TextMeshProUGUI livesText;

    // Singleton instance
    private static LiveManager instance;

    // Number of lives
    public int lives = 10; // You can set the initial number of lives as needed

    // Event to notify when lives change
    public delegate void LivesChangedDelegate(int newLives);
    public static event LivesChangedDelegate OnLivesChanged;

    // Method to get the singleton instance
    public static LiveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LiveManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("LiveManager");
                    instance = singletonObject.AddComponent<LiveManager>();
                }
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnLivesChanged += UpdateLivesText;
        UpdateLivesText(lives);
    }

    // Method to update the lives text
    void UpdateLivesText(int newLives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + newLives.ToString();
        }
    }

    // Method to decrease lives
    public void DecreaseLives()
    {
        lives--;

        OnLivesChanged?.Invoke(lives);

        if (lives <= 0)
        {
            GameObject.FindGameObjectWithTag("Modes").GetComponent<Spawner>().GameOver();
            Debug.Log("Game Over!");
        }
    }

    // Method to reset lives to a specified value
    public void ResetLives(int newLives)
    {
        lives = newLives;

        OnLivesChanged?.Invoke(lives);
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

    // Unsubscribe from the event when the LiveManager is destroyed
    private void OnDestroy()
    {
        OnLivesChanged -= UpdateLivesText;
    }
}

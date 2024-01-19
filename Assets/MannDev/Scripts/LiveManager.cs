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
    public int lives = 3; // You can set the initial number of lives as needed

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
                // If no instance exists, try to find one in the scene
                instance = FindObjectOfType<LiveManager>();

                // If no instance is found in the scene, create a new one
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
        // Subscribe to the OnLivesChanged event
        OnLivesChanged += UpdateLivesText;

        // Initialize the lives text
        UpdateLivesText(lives);
    }

    // Method to update the lives text
    void UpdateLivesText(int newLives)
    {
        // Update the TextMeshPro text component
        if (livesText != null)
        {
            livesText.text = "Lives: " + newLives.ToString();
        }
    }

    // Method to decrease lives
    public void DecreaseLives()
    {
        lives--;

        // Notify subscribers that the lives have changed
        OnLivesChanged?.Invoke(lives);

        // Check for game over or handle other logic based on remaining lives
        if (lives <= 0)
        {
            // Game over logic, reset the game or show a game over screen
            Debug.Log("Game Over!");
        }
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

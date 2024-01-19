using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    // Singleton instance
    private static UIManager instance;

    // Method to get the singleton instance
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // If no instance exists, try to find one in the scene
                instance = FindObjectOfType<UIManager>();

                // If no instance is found in the scene, create a new one
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("UIManager");
                    instance = singletonObject.AddComponent<UIManager>();
                }
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the OnScoreChanged event in ScoreManager
        ScoreManager.OnScoreChanged += UpdateScoreText;
    }

    // Method to update the score text
    void UpdateScoreText(int newScore)
    {
        // Update the TextMeshPro text component
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString();
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

    // Unsubscribe from the event when the UIManager is destroyed
    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreText;
    }
}

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEditor.Rendering;
using TMPro;

[System.Serializable]
public class GridPreset
{
    public string name;
    public string[] rows;
}

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject BoxPrefab;
    [SerializeField] private GameObject ScorePrefab;

    [SerializeField] private List<GridPreset> gridPresets;
    [SerializeField] private float spacing = 2f;
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [SerializeField] private float duration = 2f;

    private Spawner spawner;

    private void OnEnable()
    {
        // Clean the grid when the game object is enabled
        InstantiateRandomGrid();
        MoveToTarget();
    }

    private void OnDisable()
    {
        // Clean the grid when the game object is disabled
        CleanGrid();
    }

    private void Awake()
    {
        spawner = GetComponentInParent<Spawner>();
        startTransform = GameObject.FindGameObjectWithTag("StartPoz").transform;
        endTransform = GameObject.FindGameObjectWithTag("EndPoz").transform;

        if (startTransform != null)
        {
            transform.position = startTransform.position;
        }
        else
        {
            Debug.LogError("Start position not found!");
        }
    }

    private void Update()
    {
        if (LiveManager.Instance.lives <= 0)
        {
            OnGameOver();
        }
    }

    private void InstantiateRandomGrid()
    {
        GridPreset selectedPreset = gridPresets[Random.Range(0, gridPresets.Count)];
        int rows = selectedPreset.rows.Length;
        int columns = selectedPreset.rows[0].Length;

        StringBuilder stringBuilder = new StringBuilder();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                stringBuilder.Append(selectedPreset.rows[row][col] == '1' ? "1" : "0");

                float x = col * spacing;
                float y = row * spacing;

                if (selectedPreset.rows[row][col] == '1')
                {
                    Vector3 spawnPosition = new Vector3(x + transform.position.x, y + transform.position.y, transform.position.z);

                    GameObject box = Instantiate(BoxPrefab, spawnPosition, Quaternion.identity, transform);
                    box.transform.localScale = Vector3.zero;
                }
                else if (selectedPreset.rows[row][col] == '2')
                {
                    Vector3 spawnPosition = new Vector3(x + transform.position.x, y + transform.position.y, transform.position.z);                    
                    GameObject scorePrefab = Instantiate(ScorePrefab, spawnPosition, Quaternion.LookRotation(endTransform.up), transform);
                    // Optionally set the scale or other properties for the score prefab
                }
            }

            stringBuilder.AppendLine(); // Add a newline after each row
        }

        string finalResult = stringBuilder.ToString();
    }

    private void CleanGrid()
    {
        // Destroy all child objects (grid elements) when cleaning the grid
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void MoveToTarget()
    {
        if (endTransform != null)
        {
            transform.DOMove(endTransform.position, duration)
                .SetEase(Ease.Linear)
                .OnComplete(OnMoveComplete);
        }
        else
        {
            Debug.LogError("End position not found!");
        }
    }

    private void OnMoveComplete()
    {
        Debug.Log("Object has reached the end position!");
        DisableObjects();
    }

    private void DisableObjects()
    {
        gameObject.transform.position = startTransform.transform.position;
        gameObject.SetActive(false);
    }

    public void OnGameOver()
    {
        transform.DOPause();

        if (spawner != null)
        {
            spawner.TogglePause();
        }
    }
}

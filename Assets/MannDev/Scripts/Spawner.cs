using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;

public class Spawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING, PAUSED };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject enemyPrefab;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    [HideInInspector]
    public int nextWave = 0;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING;

    // Object Pooling
    private GameObject[] enemyPool;
    private int poolIndex = 0;
    [SerializeField] private bool startgame = false;

    private Coroutine spawnCoroutine;

    public float songBPM = 120f;

    public float StartDelay = 2.0f;    

    void Start()
    {
        ScoreManager.Instance.ResetScore();
        LiveManager.Instance.ResetLives(10);
        waveCountdown = StartDelay;
        enemyPool = new GameObject[waves[0].count]; // Assuming the first wave count is the maximum
        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = InstantiateEnemy(waves[0].enemyPrefab);
            enemyPool[i].SetActive(false);
        }
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0 && startgame)
        {
            if (state != SpawnState.SPAWNING && state != SpawnState.PAUSED)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            if (waveCountdown > 0 && state != SpawnState.PAUSED)
            {
                waveCountdown -= Time.deltaTime;
            }
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        // Stop the coroutine when the wave is completed
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        if (nextWave + 1 > waves.Length - 1)
        {
            GameOver();
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (IsPoolEmpty())
            {
                Debug.Log("Searching Enemy: Completed");
                waveCountdown = timeBetweenWaves;
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave" + wave.name);
        state = SpawnState.SPAWNING;

        float timeBetweenEnemies = 60f / (songBPM * wave.rate);

        for (int i = 0; i < wave.count; i++)
        {
            while (state == SpawnState.PAUSED)
            {
                yield return null;
            }

            GameObject enemy = GetPooledEnemy(wave.enemyPrefab);
            enemy.SetActive(true);

            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        state = SpawnState.WAITING;
        spawnCoroutine = null;
    }

    GameObject GetPooledEnemy(GameObject prefab)
    {
        while (enemyPool[poolIndex].activeInHierarchy)
        {
            poolIndex = (poolIndex + 1) % enemyPool.Length;
        }

        return enemyPool[poolIndex];
    }

    bool IsPoolEmpty()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (enemy.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    GameObject InstantiateEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        enemy.SetActive(false);
        return enemy;
    }

    public void TogglePause()
    {
        state = SpawnState.PAUSED;
    }

    public void StartGame()
    {
        startgame = true;
    }

    public void RestartGame()
    {
        // Reset variables
        nextWave = 0;
        waveCountdown = timeBetweenWaves;
        state = SpawnState.COUNTING;
        startgame = true;

        //reset score
        ScoreManager.Instance.ResetScore();
        // Reset enemy pool
        ResetEnemyPool();

        // Stop any ongoing coroutine
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // Start the game
        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    void ResetEnemyPool()
    {
        foreach (GameObject enemy in enemyPool)
        {
            enemy.SetActive(false);
        }
    }

    public void GameOver()
    {
        GameModes.Instance.gameObject.SetActive(true);
        GameModes.Instance.ShowButtons();
        DOTween.KillAll();

        FindObjectOfType<AudioManager>().StopActiveSound();
        Destroy(this.gameObject);
    }
}

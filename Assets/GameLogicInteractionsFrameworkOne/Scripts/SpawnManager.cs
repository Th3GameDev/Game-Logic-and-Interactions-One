using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { if (_instance == null) { Debug.LogError("The SpawnManager Is NULL!"); } return _instance; } }

    [SerializeField] private GameObject[] _enemyPrefabs;

    [Space]
    [SerializeField] private GameObject _strongEnemyPrefab;

    private List<GameObject> _enemyPool = new List<GameObject>();

    [Range(0, 20)][SerializeField][Tooltip("The Amount of Enemies to Spawn")] private int _enemySpawnAmount;
    private float _enemySpawnTime = 10f;

    private Transform _enemySpawnPos;

    private int _enemiesAlive = 0;

    private float _timeRemaining;
    private bool _isTimerCountdown = false;

    private int _currentWave;
    private int _randomNum;
    private int _lastRandomNum;

    private int _timerTicks;

    private bool _isSpawningComplete = false;
    private bool _isGameComplete = false;

    private int _enemiesSurvived = 0;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _timerTicks = 10;
        _isTimerCountdown = true;
        _timeRemaining = 10f;
        UIManager.Instance.EnableTimeRemaining();
        _currentWave = 1;
        _enemySpawnPos = transform.GetChild(1);
        _enemyPool = PopulateEnemyPool(_enemySpawnAmount, _enemySpawnPos.position);
    }

    private void Update()
    {
        //check for enemies alive to progress wave
        if (_enemiesSurvived >= 1 && !_isGameComplete)
        {
            _isGameComplete = true;
            StopCoroutine(SpawnEnemies());
            _isSpawningComplete = false;
            GameManager.Instance.GameOver(false);
        }


        if (_enemiesAlive == 0 && _isSpawningComplete)
        {
            //If current wave is ten. Its GameOver
            if (_currentWave == 10 && _isGameComplete)
            {
                _isSpawningComplete = false;

                GameManager.Instance.GameOver(true);
            }
            else
            {
                _isSpawningComplete = false;
                OnWaveComplete();
            }
        }

        //Wave timer Count down before spawning
        if (_isTimerCountdown)
        {
            if (_timeRemaining > 0f)
            {
                UIManager.Instance.UpdateTimeRemaining();
                UIManager.Instance.PlayTimerTickSound(_timerTicks);
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _timeRemaining = 0f;
                _isTimerCountdown = false;
                UIManager.Instance.DisableTimeRemaining();
                StartCoroutine(UIManager.Instance.BlinkGameObject(3, 0.3f, true));
                UIManager.Instance.PlayWarningSound();
                UIManager.Instance.UpdateWaveNumber();
                StartEnemySpawning();
            }
        }
    }

    private void StartEnemySpawning()
    {
        _enemiesAlive = 0;

        if (!_isSpawningComplete)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    List<GameObject> PopulateEnemyPool(int SpawnAmount, Vector3 enemySpawnPos)
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            GameObject enemy = Instantiate(GetRandomEnemyPrefab(_enemyPrefabs), enemySpawnPos, Quaternion.identity);

            enemy.transform.parent = transform.GetChild(0);

            enemy.SetActive(false);

            _enemyPool.Add(enemy);
        }

        return _enemyPool;
    }

    private GameObject RequestEnemy(Transform enemySpawnPos)
    {
        foreach (GameObject enemy in _enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.position = enemySpawnPos.position;

                enemy.SetActive(true);

                return enemy;
            }
        }

        GameObject newEnemy = null;

        //If wave is Five or Greater random chance to spawn strong enemy
        if (_currentWave >= 5 && Random.Range(0, 2) == 0)
        {
            newEnemy = Instantiate(_strongEnemyPrefab, enemySpawnPos.position, Quaternion.identity);
        }
        else
        {
            newEnemy = Instantiate(GetRandomEnemyPrefab(_enemyPrefabs), enemySpawnPos.position, Quaternion.identity);
        }

        newEnemy.transform.parent = transform.GetChild(0);
        _enemyPool.Add(newEnemy);
        return newEnemy;
    }

    private GameObject GetRandomEnemyPrefab(GameObject[] prefabs)
    {
        _randomNum = Random.Range(0, prefabs.Length);

        for (int i = 0; i < prefabs.Length; i++)
        {
            if (_randomNum == _lastRandomNum)
            {
                while (_randomNum == _lastRandomNum)
                {
                    _randomNum = Random.Range(0, prefabs.Length);
                }
            }

            _lastRandomNum = _randomNum;
        }

        return prefabs[_randomNum];

    }

    public void OnEnemyKilled()
    {
        _enemiesAlive--;

        UIManager.Instance.UpdateEnemyCount(_enemiesAlive);
    }

    public void OnEnemySurvived()
    {
        _enemiesSurvived++;
        _enemiesAlive--;
        UIManager.Instance.UpdateEnemyCount(_enemiesAlive);
    }

    public int GetEnemyCount()
    {
        return _enemySpawnAmount;
    }

    public float GetTimeRemaining()
    {
        return _timeRemaining;
    }

    void OnWaveComplete()
    {
        if (_currentWave < 10)
            _currentWave++;

        if (_currentWave < 5)
        {
            _timerTicks = 6;
            _isSpawningComplete = false;
            _isTimerCountdown = true;
            _timeRemaining = 6f;
            UIManager.Instance.EnableTimeRemaining();
        }
        else if (_currentWave >= 5 && _currentWave < 10)
        {
            _timerTicks = 6;
            _isSpawningComplete = false;
            _isTimerCountdown = true;
            _timeRemaining = 6f;
            UIManager.Instance.EnableTimeRemaining();

            _enemySpawnAmount++;

            _enemySpawnTime = 5f;
        }
        else if (_currentWave == 10)
        {
            _timerTicks = 6;
            _isSpawningComplete = false;
            _isTimerCountdown = true;
            _timeRemaining = 6f;
            UIManager.Instance.EnableTimeRemaining();

            _enemySpawnAmount++;

            _isGameComplete = true;
        }
    }

    public void ResetWave()
    {
        _isSpawningComplete = false;

        foreach (GameObject enemy in _enemyPool)
        {
            if (enemy.activeInHierarchy)
            {
                enemy.SetActive(false);
                enemy.GetComponent<BasicAI>().ResetAI();
                OnEnemyKilled();
            }
        }

        _enemySpawnAmount = 5;
        _timerTicks = 10;
        _isTimerCountdown = true;
        _timeRemaining = 10f;
        UIManager.Instance.EnableTimeRemaining();
        _currentWave = 1;
        _enemySpawnPos = transform.GetChild(1);
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }

    private IEnumerator SpawnEnemies()
    {
        int temp = 0;

        for (int i = 0; i < _enemySpawnAmount; i++)
        {
            GameObject enemy = RequestEnemy(_enemySpawnPos);

            if (enemy != null)
            {
                _enemiesAlive++;
                temp++;
                UIManager.Instance.UpdateEnemyCount(_enemiesAlive);
            }

            if (temp != _enemySpawnAmount)
            {
                yield return new WaitForSeconds(_enemySpawnTime);
            }
            else
            {
                yield return null;
            }
        }

        _isSpawningComplete = true;
    }
}
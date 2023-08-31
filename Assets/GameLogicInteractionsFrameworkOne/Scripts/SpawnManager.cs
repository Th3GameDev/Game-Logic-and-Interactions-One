using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { if (_instance == null) { Debug.LogError("The SpawnManager Is NULL!"); } return _instance; } }

    [SerializeField] private GameObject[] _enemyPrefabs;

    [SerializeField] private List<GameObject> _enemyPool = new List<GameObject>();

    [Range(0, 20)][SerializeField][Tooltip("The Amount of Enemies to Spawn")] private int _enemySpawnAmount;

    [SerializeField] private Transform _enemySpawnPos;

    [SerializeField] private int _enemiesSpawned = 0;

    private int _randomNum;
    private int _lastRandomNum;

    [SerializeField] private bool _isSpawningComplete = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _enemySpawnPos = transform.GetChild(1);
        _enemyPool = PopulateEnemyPool(_enemySpawnAmount, _enemySpawnPos.position);
        StartEnemySpawning();
    }

    private void Update()
    {
        if (_enemiesSpawned == 0 && _isSpawningComplete == true)
        {
            _isSpawningComplete = false;
            StartCoroutine(WaitToRespawnEnemies());
        }
    }

    private void StartEnemySpawning()
    {
        _enemiesSpawned = 0;
        _isSpawningComplete = false;
        StartCoroutine(SpawnEnemies());
    }

    List<GameObject> PopulateEnemyPool(int SpawnAmount, Vector3 enemySpawnPos)
    {
        GameObject randomEnemy = GetRandomEnemyPrefab(_enemyPrefabs);

        for (int i = 0; i < SpawnAmount; i++)
        {
            GameObject enemy = Instantiate(randomEnemy, enemySpawnPos, Quaternion.identity);

            enemy.transform.parent = transform.GetChild(0);

            enemy.SetActive(false);

            _enemyPool.Add(enemy);
        }

        return _enemyPool;
    }

    public GameObject RequestEnemy(Transform enemySpawnPos)
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
        GameObject randomEnemy = GetRandomEnemyPrefab(_enemyPrefabs);
        GameObject newEnemy = Instantiate(randomEnemy, enemySpawnPos.position, Quaternion.identity);
        newEnemy.transform.parent = transform.GetChild(0);
        _enemyPool.Add(newEnemy);

        return null;
    }

    private GameObject GetRandomEnemyPrefab(GameObject[] prefabs)
    {
        _randomNum = Random.Range(0, _enemyPrefabs.Length);

        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            if (_randomNum == _lastRandomNum)
            {
                while (_randomNum == _lastRandomNum)
                {
                    _randomNum = Random.Range(0, _enemyPrefabs.Length);
                }
            }

            _lastRandomNum = _randomNum;
        }

        return _enemyPrefabs[_randomNum];
    }

    public void OnEnemyKilled()
    {
        _enemiesSpawned--;
    }

    private IEnumerator WaitToRespawnEnemies()
    {      
        Debug.Log("Enemies Respawning Soon");

        yield return new WaitForSeconds(3f);

        StartEnemySpawning();
    }

    private IEnumerator SpawnEnemies()
    {
        //yield return new WaitForSeconds(1.5f);
      
        for (int i = 0; i < _enemySpawnAmount; i++)
        {
            GameObject enemy = RequestEnemy(_enemySpawnPos);           

            if (enemy != null)
            {
                _enemiesSpawned++;
            }

            yield return new WaitForSeconds(15f);
        }

        _isSpawningComplete = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { if (_instance == null) { Debug.LogError("The SpawnManager Is NULL!"); } return _instance; } }

    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private List<GameObject> _enemyPool = new List<GameObject>();

    [Range(0, 20)][SerializeField][Tooltip("The Amount of Enemies to Spawn")] private int _enemySpawnAmount;

    [SerializeField] private Transform _enemySpawnPos;

    private int _enemiesSpawned = 0;

    private bool _spawningComplete;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _enemySpawnPos = transform.GetChild(1);
        _enemyPool = PopulateEnemyPool(_enemySpawnAmount, _enemySpawnPos.position);
        StartCoroutine(SpawnEnemies());
    }

    List<GameObject> PopulateEnemyPool(int enemySpawnAmount, Vector3 enemySpawnPos)
    {

        for (int i = 0; i < enemySpawnAmount; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, enemySpawnPos, Quaternion.identity);

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

        GameObject newEnemy = Instantiate(_enemyPrefab, enemySpawnPos.position, Quaternion.identity);
        newEnemy.transform.parent = transform.GetChild(0);
        _enemyPool.Add(newEnemy);

        return null;
    }

    private IEnumerator SpawnEnemies()
    {
        while (_enemiesSpawned < _enemySpawnAmount)
        {
            GameObject enemy = RequestEnemy(_enemySpawnPos);

            if (enemy != null)
            {
                _enemiesSpawned++;
            }

            yield return new WaitForSeconds(25f);
        }

        _spawningComplete = true;
    }
}

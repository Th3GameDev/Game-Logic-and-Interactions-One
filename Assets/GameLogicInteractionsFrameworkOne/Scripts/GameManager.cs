using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { if (_instance == null) { Debug.LogError("GameManager is NULL!"); } return _instance; } }

    [SerializeField] private int Score = 0;


    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Score = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SpawnManager.Instance.gameObject.GetComponent<SpawnManager>().enabled = true;
    }

    public void UpdateScore(int points)
    {
        Score += points;
        UIManager.Instance.UpdateScoreUI(Score);
    }
}

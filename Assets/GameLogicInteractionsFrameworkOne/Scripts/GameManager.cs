using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { if (_instance == null) { Debug.LogError("GameManager is NULL!"); } return _instance; } }

    private int Score = 0;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private bool _isGameOver = false;

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
        //Start Enemy Spawning
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }

        //Restart Game
        if (Input.GetKeyDown(KeyCode.L) && _isGameOver)
        {
            RestartGame();
        }

        //Quit Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        UIManager.Instance.DisableStartText();
        SpawnManager.Instance.gameObject.GetComponent<SpawnManager>().enabled = true;
    }

    public void GameOver(bool Win)
    {
        if (Win)
        {
            _isGameOver = true;
            UIManager.Instance.DisplayWinLossUI(Win);
            Debug.Log("GameOver! You Won");
        }
        else
        {
            _isGameOver = true;
            UIManager.Instance.DisplayWinLossUI(Win);
            Debug.Log("GameOver! You Lost");
        }
    }

    public void RestartGame()
    {
        _audioSource.Stop();
        _audioSource.Play();
        Score = 0;
        SpawnManager.Instance.ResetWave();
        UIManager.Instance.ResetUI();
    }

    public void UpdateScore(int points)
    {
        Score += points;
        UIManager.Instance.UpdateScoreUI(Score);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    private Player _player;

    private static UIManager _instance;
    public static UIManager Instance { get { if (_instance == null) { Debug.LogError("UI Manager Is NULL"); } return _instance; } }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _enemyCountText;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _waveText;

    [SerializeField] private TextMeshProUGUI _warningText;
    [SerializeField] private TextMeshProUGUI _timeRemainingText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;

    private bool _isReset = true;
    [SerializeField] private bool _isTimerTicking = false;
    [SerializeField] private bool _isWarning = false;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
            Debug.LogError("Player Script is NULL!");

        ResetUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayWarningSound();
        }
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoText.text = $"{ammoCount}";
    }

    public void UpdateScoreUI(int score)
    {
        _scoreText.text = $"{score}";
    }

    public void UpdateEnemyCount(int enemiesRemaining)
    {
        _enemyCountText.text = $"{enemiesRemaining}";
    }

    public void UpdateWaveNumber()
    {
        _waveText.text = $"wave {SpawnManager.Instance.GetCurrentWave()}";
    }

    public void UpdateTimeRemaining()
    {
        float time = SpawnManager.Instance.GetTimeRemaining();
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        _timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        _isReset = false;
    }

    public void EnableTimeRemaining()
    {
        _timeRemainingText.enabled = true;
        _timeText.enabled = true;
    }

    public void DisableTimeRemaining()
    {
        _timeRemainingText.enabled = false;
        _timeText.enabled = false;
    }

    void ResetUI()
    {
        _scoreText.text = "0";
        _enemyCountText.text = "0";
        _ammoText.text = $"{_player.GetAmmoCount()}";
        UpdateWaveNumber();
        UpdateTimeRemaining();
    }

    public IEnumerator BlinkGameObject(int numBlinks, float seconds, bool diactivateOnExit)
    {
        _waveText.enabled = false;

        for (int i = 0; i < numBlinks * 2; i++)
        {
            //toggle Text
            _warningText.enabled = !_warningText.enabled;
            //wait for a bit
            yield return new WaitForSeconds(seconds);
        }

        if (diactivateOnExit)
        {
            //make sure Text is enabled when we exit
            _warningText.enabled = false;
            _waveText.enabled = true;
        }
        else
        {
            _warningText.enabled = true;
        }
    }

    public void PlayWarningSound()
    {
        if (_isWarning == false)
        {
            _audioSource.volume = 1.0f;
            _isWarning = true;
            StartCoroutine(PlayWarning());
        }
    }

    public void PlayTimerTickSound(int NumOfTimerTicks)
    {
        if (_isTimerTicking == false)
        {
            _audioSource.volume = 0.2f;
            _audioSource.clip = _audioClips[0];
            StartCoroutine(PlayTimerSound(NumOfTimerTicks));
            _isTimerTicking = true;
        }
    }

    private IEnumerator PlayWarning()
    {
        _audioSource.clip = _audioClips[1];

        for (int i = 0; i < 3; i++)
        {
            _audioSource.PlayOneShot(_audioClips[1]);
            yield return new WaitForSeconds(0.6f);
        }

        _isWarning = false;
    }

    private IEnumerator PlayTimerSound(int NumOfTicks)
    {
        for (int i = 0; i < NumOfTicks; i++)
        {
            if (i == 10)
            {
                _audioSource.Stop();
            }
            else if (i < 9)
            {
                _audioSource.PlayOneShot(_audioClips[0]);
            }
            yield return new WaitForSeconds(1f);
        }

        _isTimerTicking = false;
    }
}

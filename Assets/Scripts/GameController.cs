using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] Asteroids;
    public GameObject PlayerPrefab;
    public GameObject GameMenu;
    public GameObject GameOverText;
    public Button ShieldButton;
    public Button KillAllButoon;
    public GameObject GameUi;
    public Text ScoreText;
    public Vector3 SpawnValues;

    private int _score = 0;
    private bool _gameRunning = false;
    private GameObject _player;
    private float _spawnWait = 1;

    private int _shieldCoolDown = 20;
    private int _shieldDuration = 5;
    private int _killAllButton = 50;

    public void Awake()
    {
        if (!PlayerPrefs.HasKey("PlayerStats"))
        {
            var playerStats = new PlayerStats()
            {
                FireRate = 0.6f,
                NumberOfGuns = 1,
                Shield = false,
                KillAll = false
            };
            PlayerPrefs.SetString("PlayerStats", JsonUtility.ToJson(playerStats));
        }

        if (!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetInt("Score", 0);
            _score = 0;
        }
        else
        {
            _score = PlayerPrefs.GetInt("Score");
            ScoreText.text = "Score: " + _score;
        }

        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        _score = PlayerPrefs.GetInt("Score");
        ScoreText.text = "Score: " + _score;

        SetShieldButton();
        SetUltiyButton();
        GameUi.SetActive(true);
        _gameRunning = true;
        _player = Instantiate(PlayerPrefab) as GameObject;
        _player.name = "Player";
        GameOverText.SetActive(false);
        GameMenu.SetActive(false);
        _spawnWait = 1f;
        StartCoroutine(SpawnWaves());
        StartCoroutine(IntensityControl());
    }

    public void StopGame()
    {
        _gameRunning = false;
        GameUi.SetActive(false);
        Destroy(_player);
        GameMenu.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ClearBattleField());
        ShieldButton.onClick.RemoveListener(ActivateShield);

        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.Save();
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOver());
    }

    public void AddScore(int newScoreValue)
    {
        _score += newScoreValue * (int)((1.1f - _spawnWait) * 10);
        ScoreText.text = "Score: " + _score;
    }

    public void ActivateKillAll()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            var contactByDestroy = enemy.GetComponent<DestroyByContact>();
            if (contactByDestroy)
            {
                contactByDestroy.DestroyByUltiy();
            }
        }

        KillAllButoon.interactable = false;

        StartCoroutine(ReEnableButtons(_killAllButton, KillAllButoon));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator ShowGameOver()
    {
        StopGame();
        GameOverText.SetActive(true);
        yield return new WaitForSeconds(3f);
        GameOverText.SetActive(false);
    }

    private IEnumerator ClearBattleField()
    {
        yield return new WaitForEndOfFrame();
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }
    }

    private IEnumerator IntensityControl()
    {
        yield return new WaitForSeconds(5);
        if (_spawnWait >= 0.1 && _gameRunning)
        {
            _spawnWait -= 0.1f;
        }
    }

    private IEnumerator SpawnWaves()
    {
        while (_gameRunning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-SpawnValues.x, SpawnValues.x), SpawnValues.y, SpawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(Asteroids[Random.Range(0, Asteroids.Length)], spawnPosition, spawnRotation);
            yield return new WaitForSeconds(_spawnWait);
        }
    }

    private void SetShieldButton()
    {
        var playerStats = JsonUtility.FromJson<PlayerStats>(PlayerPrefs.GetString("PlayerStats"));

        if (playerStats.Shield)
        {
            ShieldButton.interactable = true;
        }
        else
        {
            ShieldButton.interactable = false;
        }

        ShieldButton.onClick.AddListener(ActivateShield);
    }

    private void ActivateShield()
    {
        var controller = _player.GetComponent<PlayerController>();
        controller.SetShieldStatus(true);
        ShieldButton.interactable = false;
        StartCoroutine(DisableShield());
    }

    private IEnumerator DisableShield()
    {
        yield return new WaitForSeconds(_shieldDuration);
        var controller = _player.GetComponent<PlayerController>();
        controller.SetShieldStatus(false);
        StartCoroutine(ReEnableButtons(_shieldCoolDown, ShieldButton));
    }

    private IEnumerator ReEnableButtons(float coolDown, Button button)
    {
        yield return new WaitForSeconds(coolDown);
        button.interactable = true;
    }

    private void SetUltiyButton()
    {
        var playerStats = JsonUtility.FromJson<PlayerStats>(PlayerPrefs.GetString("PlayerStats"));

        if (playerStats.KillAll)
        {
            KillAllButoon.interactable = true;
        }
        else
        {
            KillAllButoon.interactable = false;
        }

        KillAllButoon.onClick.AddListener(ActivateKillAll);
    }
}

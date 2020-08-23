using UnityEngine;
using UnityEngine.UI;

public class MarketController : MonoBehaviour
{
    public Button FirerateButton;
    public Button GunsButton;
    public Button ShieldButton;
    public Button KillAllButton;

    public Slider FirerateSlider;
    public Slider GunsSlider;
    public Slider ShieldSlider;
    public Slider KillAllSlider;

    public Text Coins;

    private PlayerStats _playerStats;
    private int _coins;

    private int _fireratePrice = 500;
    private int _gunsPrice = 500;
    private int _shieldPrice = 2000;
    private int _killAllPrice = 5000;

    private void OnEnable()
    {
        _playerStats = JsonUtility.FromJson<PlayerStats>(PlayerPrefs.GetString("PlayerStats"));
        _coins = PlayerPrefs.GetInt("Score");

        SetValues();
        SetPrices();
        Checks();

        FirerateButton.onClick.AddListener(() => { Buy("firerate"); });
        GunsButton.onClick.AddListener(() => { Buy("guns"); });
        ShieldButton.onClick.AddListener(() => { Buy("shield"); });
        KillAllButton.onClick.AddListener(() => { Buy("killAll"); });
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
        FirerateButton.onClick.RemoveAllListeners();
        GunsButton.onClick.RemoveAllListeners();
        ShieldButton.onClick.RemoveAllListeners();
        KillAllButton.onClick.RemoveAllListeners();
    }

    private void Buy(string type)
    {
        switch (type)
        {
            case "firerate":
                _playerStats.FireRate -= 0.05f;
                _coins -= _fireratePrice;
                break;

            case "guns":
                _playerStats.NumberOfGuns++;
                _coins -= _gunsPrice;
                break;

            case "shield":
                _playerStats.Shield = true;
                _coins -= _shieldPrice;
                break;

            case "killAll":
                _playerStats.KillAll = true;
                _coins -= _killAllPrice;
                break;
        }

        PlayerPrefs.SetString("PlayerStats", JsonUtility.ToJson(_playerStats));
        PlayerPrefs.SetInt("Score", _coins);

        SetValues();
        SetPrices();
        Checks();
    }

    private void SetValues()
    {
        FirerateSlider.value = 0.6f - _playerStats.FireRate;
        GunsSlider.value = _playerStats.NumberOfGuns;
        ShieldSlider.value = _playerStats.Shield ? 1 : 0;
        KillAllSlider.value = _playerStats.KillAll ? 1 : 0;
        Coins.text = "MONEY: " + _coins + " C";
    }

    private void SetPrices()
    {
        _fireratePrice = (int)(70f - (_playerStats.FireRate * 100f)) * 50;
        _gunsPrice = _playerStats.NumberOfGuns * 500;

        FirerateButton.GetComponentInChildren<Text>().text = _fireratePrice + " C";
        GunsButton.GetComponentInChildren<Text>().text = _gunsPrice + " C";
    }

    private void Checks()
    {
        if (_playerStats.FireRate <= 0.1 || _coins < _fireratePrice)
        {
            FirerateButton.interactable = false;
        }
        else
        {
            FirerateButton.interactable = true;
        }

        if (_playerStats.NumberOfGuns == 4 || _coins < _gunsPrice)
        {
            GunsButton.interactable = false;
        }
        else
        {
            GunsButton.interactable = true;
        }

        if (_playerStats.Shield || _coins < _shieldPrice)
        {
            ShieldButton.interactable = false;
        }
        else
        {
            ShieldButton.interactable = true;
        }

        if (_playerStats.KillAll || _coins < _killAllPrice)
        {
            KillAllButton.interactable = false;
        }
        else
        {
            KillAllButton.interactable = true;
        }
    }
}

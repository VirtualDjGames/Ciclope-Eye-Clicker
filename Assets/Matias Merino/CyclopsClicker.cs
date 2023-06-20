using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CyclopsClicker : MonoBehaviour
{
    public TextMeshProUGUI tearCountText;
    public TextMeshProUGUI tearsPerClickUpgradePriceText;
    public TextMeshProUGUI[] autoClickerUpgradePriceTexts;
    public TextMeshProUGUI autoClickerPriceText;
    public Button buyAutoClickerButton;
    public Button[] upgradeAutoClickerButtons;
    public GameObject[] minionsimage;

    public float baseTearsPerClick = 1f;
    public float tearsPerClickUpgradePrice = 10f;

    public int maxAutoClickers = 2;
    public float autoClickerPrice = 5000f;
    public float[] autoClickerUpgradePrice;
    public float[] autoClickerUpgradeAmount;

    private float tearCount;
    private float tearsPerClickLevel = 1f;
    private int autoClickerLevel = 0;

    private const string TearsKey = "TearCount";
    private const string TearsPerClickLevelKey = "TearsPerClickLevel";
    private const string AutoClickerLevelKey = "AutoClickerLevel";
    private const string TearsPerClickUpgradePriceKey = "TearsPerClickUpgradePrice";
    private const string AutoClickerPriceKey = "AutoClickerPrice";
    private const string AutoClickerUpgradePriceKey = "AutoClickerUpgradePrice";
    private const string AutoClickerUpgradeAmountKey = "AutoClickerUpgradeAmount";
    private const string LastSaveTimeKey = "LastSaveTime";

    private float autoClickerTimer = 1f;
    private const float AutoClickerInterval = 1f;

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
        else
        {
            LoadData();
            CalculateOfflineEarnings();
        }
    }

    private void Start()
    {
        LoadData();
        UpdateTearCountText();
        UpdateUpgradePriceText();
        UpdateAutoClickerButtons();

        // Iniciar la recolección de lágrimas de los subditos incluso cuando el juego está cerrado
        InvokeRepeating("AutoClickerTick", 0f, AutoClickerInterval);
    }

    private void UpdateTearCountText()
    {
        tearCountText.text =  Mathf.Round(tearCount).ToString();
    }

    private void UpdateUpgradePriceText()
    {
        tearsPerClickUpgradePriceText.text = Mathf.Round(tearsPerClickUpgradePrice).ToString();
        autoClickerPriceText.text = Mathf.Round(autoClickerPrice).ToString();

        for (int i = 0; i < maxAutoClickers; i++)
        {
            autoClickerUpgradePriceTexts[i].text = (i + 1) + Mathf.Round(autoClickerUpgradePrice[i]).ToString();
        }
    }

    public void AddTear()
    {
        tearCount += Mathf.Round(tearsPerClickLevel * baseTearsPerClick);
        UpdateTearCountText();
        SaveData();
    }

    public void BuyTearsPerClickUpgrade()
    {
        if (tearCount >= tearsPerClickUpgradePrice)
        {
            tearCount -= tearsPerClickUpgradePrice;
            tearsPerClickUpgradePrice *= 1.5f;
            tearsPerClickLevel *= 1.3f;
            UpdateTearCountText();
            UpdateUpgradePriceText();
            SaveData();
        }
    }

    public void BuyAutoClicker()
    {
        if (tearCount >= autoClickerPrice && autoClickerLevel < maxAutoClickers)
        {
            tearCount -= autoClickerPrice;
            autoClickerLevel++;
            autoClickerPrice *= 5f;
            UpdateTearCountText();
            UpdateUpgradePriceText();
            UpdateAutoClickerButtons();
            SaveData();
        }
    }

    public void BuyAutoClickerUpgrade(int index)
    {
        if (tearCount >= autoClickerUpgradePrice[index])
        {
            tearCount -= autoClickerUpgradePrice[index];
            autoClickerUpgradeAmount[index] += 1f;
            autoClickerUpgradePrice[index] *= 2f;
            UpdateTearCountText();
            UpdateUpgradePriceText();
            SaveData();
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(TearsKey, tearCount);
        PlayerPrefs.SetFloat(TearsPerClickLevelKey, tearsPerClickLevel);
        PlayerPrefs.SetInt(AutoClickerLevelKey, autoClickerLevel);
        PlayerPrefs.SetFloat(TearsPerClickUpgradePriceKey, tearsPerClickUpgradePrice);
        PlayerPrefs.SetFloat(AutoClickerPriceKey, autoClickerPrice);

        for (int i = 0; i < maxAutoClickers; i++)
        {
            string upgradePriceKey = AutoClickerUpgradePriceKey + i;
            PlayerPrefs.SetFloat(upgradePriceKey, autoClickerUpgradePrice[i]);

            string upgradeAmountKey = AutoClickerUpgradeAmountKey + i;
            PlayerPrefs.SetFloat(upgradeAmountKey, autoClickerUpgradeAmount[i]);
        }

        // Guardar la hora actual en el dispositivo
        PlayerPrefs.SetString(LastSaveTimeKey, DateTime.Now.ToString());

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(TearsKey))
        {
            tearCount = PlayerPrefs.GetFloat(TearsKey);
        }

        if (PlayerPrefs.HasKey(TearsPerClickLevelKey))
        {
            tearsPerClickLevel = PlayerPrefs.GetFloat(TearsPerClickLevelKey);
        }

        if (PlayerPrefs.HasKey(AutoClickerLevelKey))
        {
            autoClickerLevel = PlayerPrefs.GetInt(AutoClickerLevelKey);
        }

        if (PlayerPrefs.HasKey(TearsPerClickUpgradePriceKey))
        {
            tearsPerClickUpgradePrice = PlayerPrefs.GetFloat(TearsPerClickUpgradePriceKey);
        }

        if (PlayerPrefs.HasKey(AutoClickerPriceKey))
        {
            autoClickerPrice = PlayerPrefs.GetFloat(AutoClickerPriceKey);
        }

        for (int i = 0; i < maxAutoClickers; i++)
        {
            string upgradePriceKey = AutoClickerUpgradePriceKey + i;
            if (PlayerPrefs.HasKey(upgradePriceKey))
            {
                autoClickerUpgradePrice[i] = PlayerPrefs.GetFloat(upgradePriceKey);
            }

            string upgradeAmountKey = AutoClickerUpgradeAmountKey + i;
            if (PlayerPrefs.HasKey(upgradeAmountKey))
            {
                autoClickerUpgradeAmount[i] = PlayerPrefs.GetFloat(upgradeAmountKey);
            }
        }
    }

    private void UpdateAutoClickerButtons()
    {
        buyAutoClickerButton.gameObject.SetActive(autoClickerLevel < maxAutoClickers);

        for (int i = 0; i < maxAutoClickers; i++)
        {
            upgradeAutoClickerButtons[i].gameObject.SetActive(i < autoClickerLevel);
            minionsimage[i].gameObject.SetActive(i < autoClickerLevel);
        }
    }

    private void AutoClickerTick()
    {
        if (autoClickerLevel > 0)
        {
            float timeSinceLastSave = (float)(DateTime.Now - GetLastSaveTime()).TotalSeconds;
            float totalTearsPerSecond = 0f;

            for (int i = 0; i < autoClickerLevel; i++)
            {
                totalTearsPerSecond += autoClickerUpgradeAmount[i] * 1.1f;
            }

            float tearsToAdd = totalTearsPerSecond * timeSinceLastSave;
            tearCount += tearsToAdd;
            UpdateTearCountText();
            SaveData();
        }
    }

    private DateTime GetLastSaveTime()
    {
        if (PlayerPrefs.HasKey(LastSaveTimeKey))
        {
            string lastSaveTimeString = PlayerPrefs.GetString(LastSaveTimeKey);
            DateTime lastSaveTime = DateTime.Parse(lastSaveTimeString);
            return lastSaveTime;
        }

        return DateTime.Now;
    }

    private void CalculateOfflineEarnings()
    {
        if (autoClickerLevel > 0)
        {
            float timeSinceLastSave = (float)(DateTime.Now - GetLastSaveTime()).TotalSeconds;
            float totalTearsPerSecond = 0f;

            for (int i = 0; i < autoClickerLevel; i++)
            {
                totalTearsPerSecond += autoClickerUpgradeAmount[i] * 1.1f;
            }

            float tearsToAdd = totalTearsPerSecond * timeSinceLastSave;
            tearCount += tearsToAdd;
            UpdateTearCountText();
            SaveData();
        }
    }
}

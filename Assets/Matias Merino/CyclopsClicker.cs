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

    public int baseTearsPerClick = 1;
    public int tearsPerClickUpgradePrice = 10;
    public int tearsPerClickUpgradeAmount = 1;

    public int maxAutoClickers = 2;
    public int autoClickerPrice = 2000;
    public int[] autoClickerUpgradePrice;
    public int[] autoClickerUpgradeAmount;

    private int tearCount;
    private int tearsPerClickLevel = 1;
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
        tearCountText.text = "Lágrimas: " + tearCount.ToString();
    }

    private void UpdateUpgradePriceText()
    {
        tearsPerClickUpgradePriceText.text = "Precio mejora Lágrimas/Clic: " + tearsPerClickUpgradePrice.ToString();
        autoClickerPriceText.text = "Precio subdito: " + autoClickerPrice.ToString();

        for (int i = 0; i < maxAutoClickers; i++)
        {
            autoClickerUpgradePriceTexts[i].text = "Precio mejora subdito " + (i + 1) + ": " + autoClickerUpgradePrice[i].ToString();
        }
    }

    public void AddTear()
    {
        tearCount += tearsPerClickLevel * baseTearsPerClick;
        UpdateTearCountText();
        SaveData();
    }

    public void BuyTearsPerClickUpgrade()
    {
        if (tearCount >= tearsPerClickUpgradePrice)
        {
            tearCount -= tearsPerClickUpgradePrice;
            tearsPerClickUpgradePrice *= 2;
            tearsPerClickUpgradeAmount *= 2;
            tearsPerClickLevel++;
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
            autoClickerPrice *= 2;
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
            autoClickerUpgradeAmount[index] *= 2;
            autoClickerUpgradePrice[index] *= 2;
            UpdateTearCountText();
            UpdateUpgradePriceText();
            SaveData();
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(TearsKey, tearCount);
        PlayerPrefs.SetInt(TearsPerClickLevelKey, tearsPerClickLevel);
        PlayerPrefs.SetInt(AutoClickerLevelKey, autoClickerLevel);
        PlayerPrefs.SetInt(TearsPerClickUpgradePriceKey, tearsPerClickUpgradePrice);
        PlayerPrefs.SetInt(AutoClickerPriceKey, autoClickerPrice);

        for (int i = 0; i < maxAutoClickers; i++)
        {
            string upgradePriceKey = AutoClickerUpgradePriceKey + i;
            PlayerPrefs.SetInt(upgradePriceKey, autoClickerUpgradePrice[i]);

            string upgradeAmountKey = AutoClickerUpgradeAmountKey + i;
            PlayerPrefs.SetInt(upgradeAmountKey, autoClickerUpgradeAmount[i]);
        }

        // Guardar la hora actual en el dispositivo
        PlayerPrefs.SetString(LastSaveTimeKey, DateTime.Now.ToString());

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(TearsKey))
        {
            tearCount = PlayerPrefs.GetInt(TearsKey);
        }

        if (PlayerPrefs.HasKey(TearsPerClickLevelKey))
        {
            tearsPerClickLevel = PlayerPrefs.GetInt(TearsPerClickLevelKey);
        }

        if (PlayerPrefs.HasKey(AutoClickerLevelKey))
        {
            autoClickerLevel = PlayerPrefs.GetInt(AutoClickerLevelKey);
        }

        if (PlayerPrefs.HasKey(TearsPerClickUpgradePriceKey))
        {
            tearsPerClickUpgradePrice = PlayerPrefs.GetInt(TearsPerClickUpgradePriceKey);
        }

        if (PlayerPrefs.HasKey(AutoClickerPriceKey))
        {
            autoClickerPrice = PlayerPrefs.GetInt(AutoClickerPriceKey);
        }

        for (int i = 0; i < maxAutoClickers; i++)
        {
            string upgradePriceKey = AutoClickerUpgradePriceKey + i;
            if (PlayerPrefs.HasKey(upgradePriceKey))
            {
                autoClickerUpgradePrice[i] = PlayerPrefs.GetInt(upgradePriceKey);
            }

            string upgradeAmountKey = AutoClickerUpgradeAmountKey + i;
            if (PlayerPrefs.HasKey(upgradeAmountKey))
            {
                autoClickerUpgradeAmount[i] = PlayerPrefs.GetInt(upgradeAmountKey);
            }
        }
    }

    private void UpdateAutoClickerButtons()
    {
        buyAutoClickerButton.gameObject.SetActive(autoClickerLevel < maxAutoClickers);

        for (int i = 0; i < maxAutoClickers; i++)
        {
            upgradeAutoClickerButtons[i].gameObject.SetActive(i < autoClickerLevel);
        }
    }

    private void AutoClickerTick()
    {
        if (autoClickerLevel > 0)
        {
            float timeSinceLastSave = (float)(DateTime.Now - GetLastSaveTime()).TotalSeconds;
            int totalTearsPerSecond = 0;

            for (int i = 0; i < autoClickerLevel; i++)
            {
                totalTearsPerSecond += autoClickerUpgradeAmount[i] * 2;
            }

            int tearsToAdd = (int)(totalTearsPerSecond * timeSinceLastSave);
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
            int totalTearsPerSecond = 0;

            for (int i = 0; i < autoClickerLevel; i++)
            {
                totalTearsPerSecond += autoClickerUpgradeAmount[i] * 2;
            }

            int tearsToAdd = (int)(totalTearsPerSecond * timeSinceLastSave);
            tearCount += tearsToAdd;
            UpdateTearCountText();
            SaveData();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contador : MonoBehaviour
{
    // Agregar lagrima
    int Clicks;
    public GameObject[] Ciclopes;
    private int activeImageIndex = 0;
    private string ClicksKey = "Clicks";
    private string ActiveImageIndexKey = "ActiveImageIndex";

    private void Start()
    {
        LoadData();
        UpdateActiveImage();
    }

    public void Clickear_Ojo()
    {
        Clicks++;
        Cambio();
        SaveData();
    }

    void Cambio()
    {
        if (Clicks >= 100)
        {
            activeImageIndex = 2;
        }
        if (Clicks >= 200)
        {
            activeImageIndex = 3;
        }
        if (Clicks >= 300)
        {
            activeImageIndex = 4;
        }
        if (Clicks >= 400)
        {
            activeImageIndex = 5;
        }
        if (Clicks >= 500)
        {
            activeImageIndex = 6;
        }
        if (Clicks >= 600)
        {
            activeImageIndex = 7;
        }
        if (Clicks >= 700)
        {
            activeImageIndex = 8;
        }
        if (Clicks >= 800)
        {
            activeImageIndex = 9;
        }
        if (Clicks >= 900)
        {
            activeImageIndex = 10;
        }
        if (Clicks >= 1000)
        {
            Clicks = 0;
            activeImageIndex = 0;
        }

        UpdateActiveImage();
    }

    private void UpdateActiveImage()
    {
        for (int i = 0; i < Ciclopes.Length; i++)
        {
            Ciclopes[i].SetActive(i == activeImageIndex);
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(ClicksKey, Clicks);
        PlayerPrefs.SetInt(ActiveImageIndexKey, activeImageIndex);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(ClicksKey))
        {
            Clicks = PlayerPrefs.GetInt(ClicksKey);
        }

        if (PlayerPrefs.HasKey(ActiveImageIndexKey))
        {
            activeImageIndex = PlayerPrefs.GetInt(ActiveImageIndexKey);
        }
    }
}
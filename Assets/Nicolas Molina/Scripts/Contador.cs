using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contador : MonoBehaviour
{
    //addtear agregar lagrima
    int Clicks;
    public GameObject[] Ciclopes;

    public void Clickear_Ojo()
    {
        //9
        Clicks++;
        Cambio();
    }
    
    void Cambio()
    {
        if (Clicks >= 100)
        {
            Ciclopes[2].SetActive(true);
        }
        else if (Clicks >= 200)
        {
            Ciclopes[2].SetActive(false);
            Ciclopes[3].SetActive(true);
        }
        else if (Clicks >= 300)
        {
            Ciclopes[3].SetActive(false);
            Ciclopes[4].SetActive(true);
        }
        else if (Clicks >= 400)
        {
            Ciclopes[4].SetActive(false);
            Ciclopes[5].SetActive(true);
        }
        else if (Clicks >= 500)
        {
            Ciclopes[5].SetActive(false);
            Ciclopes[6].SetActive(true);
        }
        else if (Clicks >= 600)
        {
            Ciclopes[6].SetActive(false);
            Ciclopes[7].SetActive(true);
        }
        else if (Clicks >= 700)
        {
            Ciclopes[7].SetActive(false);
            Ciclopes[8].SetActive(true);
        }
        else if (Clicks >= 800)
        {
            Ciclopes[8].SetActive(false);
            Ciclopes[9].SetActive(true);
        }
        else if (Clicks >= 900)
        {
            Ciclopes[9].SetActive(false);
            Ciclopes[10].SetActive(true);
        }
        else if (Clicks >= 1000)
        {
            Clicks = 0;
        }
    }


}

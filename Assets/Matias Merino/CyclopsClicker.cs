using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CyclopsClicker : MonoBehaviour
{
    public TextMeshProUGUI tearCountText; // Referencia al Texto que mostrar� la cantidad de l�grimas obtenidas
    public int tearsPerClick = 1; // Cantidad de l�grimas por clic

    private int tearCount; // Contador de l�grimas

    // M�todo para agregar l�grimas cuando el jugador hace clic en el ojo
    public void AddTear()
    {
        tearCount += tearsPerClick;
        tearCountText.text = "L�grimas: " + tearCount.ToString();
    }
}

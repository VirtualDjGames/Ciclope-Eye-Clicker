using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CyclopsClicker : MonoBehaviour
{
    public TextMeshProUGUI tearCountText; // Referencia al Texto que mostrará la cantidad de lágrimas obtenidas
    public int tearsPerClick = 1; // Cantidad de lágrimas por clic

    private int tearCount; // Contador de lágrimas

    // Método para agregar lágrimas cuando el jugador hace clic en el ojo
    public void AddTear()
    {
        tearCount += tearsPerClick;
        tearCountText.text = "Lágrimas: " + tearCount.ToString();
    }
}

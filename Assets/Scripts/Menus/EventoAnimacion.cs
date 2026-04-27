using UnityEngine;

public class EventoAnimacion : MonoBehaviour
{
    public GameObject botones;

    public void ActivarBotonesDelMenu()
    {
        botones.SetActive(true);
    }
}

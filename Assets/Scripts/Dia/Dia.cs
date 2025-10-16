using UnityEngine;

public class Dia : MonoBehaviour
{
     private IFases[] faseActual;

    private void Awake()
    {
        faseActual = GetComponentsInChildren<IFases>();
    }
}

using UnityEngine;

public class A_Seleccion : MonoBehaviour, IAcciones
{
    void IAcciones.ejecutarAccion()
    {
        Debug.Log("Seleccion");
    }
}

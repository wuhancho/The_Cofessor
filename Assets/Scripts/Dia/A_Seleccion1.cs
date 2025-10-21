using UnityEngine;

public class A_Seleccion1 : MonoBehaviour, IAcciones
{
    [SerializeField] string[] seleccion = { "Seleccion"," " };

    void IAcciones.EjecutarAccion(string accion)
    {

    }
}

using UnityEngine;

public class A_Seleccion : MonoBehaviour, IAcciones
{
    [SerializeField] string[] seleccion = { "Seleccion"," " };

    private void ButtonSeleccion(string action)
    {
        switch (action)
        {

        }
    }
    void IAcciones.EjecutarAccion(string accion)
    {
        
    }
}

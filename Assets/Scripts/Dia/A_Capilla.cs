using UnityEngine;
using UnityEngine.Events;

public class A_Capilla : MonoBehaviour, IAcciones
{
    public UnityEvent onCapillaAction;
    public UnityEvent onCapillaCancel;

    void IAcciones.EjecutarAccion()
    {
        Debug.Log("Ejecutando acci�n de la Capilla.");
        // Aqu� va la l�gica espec�fica para la acci�n de la Capilla.
    }
    public void TriggerCapillaAction()
    {
        Debug.Log("Acci�n de la Capilla activada.");
        onCapillaAction.Invoke();
    }
    public void CancelCapillaAction()
    {
        Debug.Log("Acci�n de la Capilla cancelada.");
        onCapillaCancel.Invoke();
    }

}

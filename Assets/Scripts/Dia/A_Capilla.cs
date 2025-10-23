using UnityEngine;
using UnityEngine.Events;

public class A_Capilla : MonoBehaviour, IAcciones
{
    public UnityEvent onCapillaAction;
    public UnityEvent onCapillaCancel;

    void IAcciones.EjecutarAccion()
    {
        Debug.Log("Ejecutando acción de la Capilla.");
        // Aquí va la lógica específica para la acción de la Capilla.
    }
    public void TriggerCapillaAction()
    {
        Debug.Log("Acción de la Capilla activada.");
        onCapillaAction.Invoke();
    }
    public void CancelCapillaAction()
    {
        Debug.Log("Acción de la Capilla cancelada.");
        onCapillaCancel.Invoke();
    }

}

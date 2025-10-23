using UnityEngine;
using UnityEngine.Events;

public class A_Capilla : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energy;
    [SerializeField] private int faith;

    public UnityEvent onCapillaAction;
    public UnityEvent onCapillaCancel;
    public UnityEvent onCapillaOperative;

    public int EnergyCost => energy;

    public void EjecutarAccion(PlayerStatus playerStatus)
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
    public void OperateCapilla()
    {
        Debug.Log("Operando la Capilla.");
        onCapillaOperative.Invoke();
    }

}

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
    public void OperateCapilla()
    {
        Debug.Log("Operando la Capilla.");
        onCapillaOperative.Invoke();
    }

}

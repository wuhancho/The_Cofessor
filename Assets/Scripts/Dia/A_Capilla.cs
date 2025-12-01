using UnityEngine;
using UnityEngine.Events;

public class A_Capilla : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energy;
    [SerializeField] private int faith;
    [SerializeField] private string nameAction;
    private int day;

    public UnityEvent<string> onCapillaAction;
    public UnityEvent onCapillaCancel;
    public UnityEvent<int,int> onCapillaOperative;

    public int EnergyCost => energy;

    public int FaithCost =>faith;

    public int ReputationChurchCost => 0;

    public int ReputationPeopleCost => 0;

    public void SetDay(int day)
    {
        this.day = day;
    }

    public void EjecutarAccion(PlayerController playerController)
    {
        Debug.Log("Ejecutando acción de la Capilla.");
        // Aquí va la lógica específica para la acción de la Capilla.
    }

    public void TriggerAction()
    {
        Debug.Log("Acción de la Capilla activada.");
        onCapillaAction.Invoke(nameAction);
    }
    public void CancelAction()
    {
        Debug.Log("Acción de la Capilla cancelada.");
        onCapillaCancel.Invoke();
    }
    public void OperateCapilla()
    {
        Debug.Log("Operando la Capilla.");
        onCapillaOperative.Invoke(energy,faith);
    }

    public void Initialize(PlayerController playerController)
    {
        throw new System.NotImplementedException();
    }
}

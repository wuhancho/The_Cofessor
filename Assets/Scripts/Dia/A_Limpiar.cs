using UnityEngine;

public class A_Limpiar : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energyCost;
    [SerializeField] private string actionName;
    public int EnergyCost => energyCost;

    public int FaithCost => 0;

    public int ReputationChurchCost => 0;

    public int ReputationPeopleCost => 0;

    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void EjecutarAccion(PlayerStatus playerStatus)
    {
        Debug.Log($"Ejecutando acción de {actionName}.");
        // Aquí va la lógica específica para la acción de Limpiar.
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

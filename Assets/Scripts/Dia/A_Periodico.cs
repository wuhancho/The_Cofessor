using UnityEngine;

public class A_Periodico : MonoBehaviour, IAccionesEnergia
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
        // Aquí va la lógica específica para la acción del Periódico.
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

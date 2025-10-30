using UnityEngine;

public class A_Limpiar : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energyCost;
    [SerializeField] private float moneyCost;
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
        bool iscleaned = playerStatus.cleaned;
        // Aquí va la lógica específica para la acción de Limpiar.
        if (iscleaned)
        {
            Debug.Log("El jugador ya ha limpiado. No se puede limpiar de nuevo.");
            return;
        }
        else
        {
            playerStatus.DecreaseEnergy(EnergyCost);
            playerStatus.IncreaseRepPueblo(ReputationPeopleCost);
            playerStatus.Spendmoney(moneyCost);
            playerStatus.SetCleaned(true);
            Debug.Log($"El jugador ha limpiado y ha gastado {EnergyCost} de energía.");
        }
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

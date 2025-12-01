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

    public void EjecutarAccion(PlayerController playerController)
    {
        Debug.Log($"Ejecutando acción de {actionName}.");
        bool iscleaned = playerController.PlayerStatus.cleaned;
        // Aquí va la lógica específica para la acción de Limpiar.
        if (iscleaned)
        {
            Debug.Log("El jugador ya ha limpiado. No se puede limpiar de nuevo.");
            return;
        }
        else
        {
            playerController.PlayerStatus.DecreaseEnergy(EnergyCost);
            playerController.PlayerStatus.IncreaseRepPueblo(ReputationPeopleCost);
            playerController.PlayerStatus.Spendmoney(moneyCost);
            playerController.PlayerStatus.SetCleaned(true);
            Debug.Log($"El jugador ha limpiado y ha gastado {EnergyCost} de energía.");
        }
    }

    public void Initialize(PlayerController playerController)
    {
        throw new System.NotImplementedException();
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

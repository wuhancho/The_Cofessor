using UnityEngine;

public class A_Limpiar : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energyCost;
    [SerializeField] private float moneyCost;
    [SerializeField] private string actionName = "limpiar";
    private int day;
    private PlayerController _playerController;
    public int EnergyCost => energyCost;

    public int FaithCost => 0;

    public int ReputationChurchCost => 0;

    public int ReputationPeopleCost => 0;

    public void SetDay(int day)
    {
        this.day = day;
    }
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

            Dia.Instance.RemoveEnergy(EnergyCost);
            playerController.PlayerStatus.IncreaseRepPueblo(ReputationPeopleCost);
            playerController.PlayerStatus.Spendmoney(moneyCost);
            playerController.PlayerStatus.SetCleaned(true);
            Debug.Log($"El jugador ha limpiado y ha gastado {EnergyCost} de energía.");
        }
    }

    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        //SetDay(playerController.PlayerStatus.Day);
        //Debug.Log($"playerController.Day: {playerController.PlayerStatus.Day}");
    }

    public void TriggerAction()
    {
        EjecutarAccion(_playerController);
    }
    public void DebugAccion()
    {
        Debug.Log($"{_playerController.PlayerStatus.Day} - Acción de {actionName} - Día: {day}, Costo de Energía: {energyCost}, Costo de Dinero: {moneyCost}");
    }
}

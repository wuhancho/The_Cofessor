using UnityEngine;

public class A_Periodico : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energyCost;
    [SerializeField] private string actionName;
    private int day;
    private PlayerController _playerController;
    private GameObject[] newsPapers;
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
        // Aquí va la lógica específica para la acción del Periódico.
    }

    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        //SetDay(playerController.PlayerStatus.Day);
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
    public void DebugAccion() 
    {
        Debug.Log($"{_playerController.PlayerStatus.Day} - Acción de {actionName} - Día: {day}, Costo de Energía: {energyCost}");
    }
}

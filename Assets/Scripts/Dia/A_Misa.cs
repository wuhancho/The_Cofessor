using UnityEngine;
using UnityEngine.Events;

public class A_Misa : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energyCost;
    [SerializeField] private int faithCost;
    [SerializeField] private string actionName;
    private int day;
    private PlayerController _playerController;

    [Header("Configuración de la acción")]
    [Space]
    [Tooltip("Evento que se dispara para habilitar el boton.\n objetos necesarios")]   
    public UnityEvent onMisaAction;
    [Tooltip("Evento que se dispara al cancelar la Misa.")]
    public UnityEvent onMisaCancel;
    [Tooltip("Evento que se dispara al activar la Capilla.\n objetos necesarios(DEBEN ESTAR EN EL ESCENARIO)\n Dia")]
    public UnityEvent<int> onMisaOperativeEnergyCost;
    [Tooltip("Evento que se dispara al activar la Capilla.\n objetos necesarios(DEBEN ESTAR EN EL ESCENARIO)\n Dia")]
    public UnityEvent<int> onMisaOperativeFaithCost;

    public int EnergyCost => energyCost;

    public int FaithCost => faithCost;

    public int ReputationChurchCost => 0;

    public int ReputationPeopleCost => 0;

    public void SetDay(int day)
    {
        this.day = day;
    }

    public void CancelAction()
    {
       onMisaCancel.Invoke();
    }

    public void EjecutarAccion(PlayerController playerController)
    {
        Debug.Log($"Ejecutando acción de {actionName}.");
        // Aquí va la lógica específica para la acción de la Misa.
    }

    public void TriggerAction()
    {
        onMisaAction.Invoke();
    }
    public void OperateMisa()
    {
        Debug.Log("Operando la Misa.");
        onMisaOperativeEnergyCost.Invoke(energyCost);
        onMisaOperativeFaithCost.Invoke(faithCost);
        _playerController.PlayerStatus.SetMisaDone(true);
    }
    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        //SetDay(playerController.PlayerStatus.Day);
    }

    public void DebugAccion() 
    {
        Debug.Log($"{_playerController.PlayerStatus.Day} - Acción de {actionName} - Día: {day}, Costo de Energía: {energyCost}");
    }
}

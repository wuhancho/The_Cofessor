using UnityEngine;
using UnityEngine.Events;

public class A_Rezar : MonoBehaviour, IAccionesEnergia
{
    [SerializeField] private int energy;
    [SerializeField] private int faith;
    [SerializeField] private string nameAction;
    private int day;
     private PlayerController _playerController;

    [Header("Configuración de la acción")]
    [Space]
    [Tooltip("Evento que se dispara para habilitar el boton.\n objetos necesarios(DEBEN ESTAR EN EL ESCENARIO)\n " +
        "Altar - mesa interactuable\n" +
        "REZO\n " +
        "ENERGIA DIARIA")]
    public UnityEvent onCapillaAction;
    [Tooltip("Evento que se dispara al cancelar la Capilla.\n objetos necesarios(DEBEN ESTAR EN EL ESCENARIO)\n " +
        "Altar - mesa interactuable\n" +
        "REZO\n " +
        "ENERGIA DIARIA")]
    public UnityEvent onCapillaCancel;
    [Tooltip("Evento que se dispara al activar la Capilla.\n objetos necesarios(DEBEN ESTAR EN EL ESCENARIO)\n Dia")]
    public UnityEvent<int> onCapillaOperativeEnergyCost;
    [Tooltip("Evento que se dispara al activar la Capilla.\n objetos necesarios(DEBEN ESTAR EN EL ESCENARIO)\n Dia")]
    public UnityEvent<int> onCapillaOperativeFaithCost;

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
        onCapillaAction.Invoke();
    }
    public void CancelAction()
    {
        Debug.Log("Acción de la Capilla cancelada.");
        onCapillaCancel.Invoke();
    }
    public void OperateCapilla()
    {
        Debug.Log("Operando la Capilla.");
        onCapillaOperativeEnergyCost.Invoke(energy);
        onCapillaOperativeFaithCost.Invoke(faith);

    }

    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        //SetDay(playerController.PlayerStatus.Day);
    }
    public void DebugAccion() 
    {
        Debug.Log($"{_playerController.PlayerStatus.Day} - Acción de Rezar - Día: {day}, Costo de Energía: {energy}, Costo de Fe: {faith}");
    }

}

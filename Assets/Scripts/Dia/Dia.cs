using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum TypeEventDay
{
    tutorial,
    day1,
    day2,
    day3,
    day4,
    day5,
    day6,
}

[RequireComponent(typeof(EventDayManager))]
public class Dia : MonoBehaviour
{
    private IFases[] fasesActuales;
    [SerializeField, IReadOnly] private EventDayManager eventDay;
    [SerializeField] private DayEvent eventDayActual;
    private PlayerController playerController;
    private PenitentController penitentController;
    [SerializeField] int numberDay;
    [SerializeField] private GameObject[] energyPrefab;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField, IReadOnly] private SPenitent guiltyPenitent;



    public void Awake()
    {
        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
        }
        if (penitentController == null)
        {
            penitentController = FindAnyObjectByType<PenitentController>();
        }
        eventDay = GetComponent<EventDayManager>();
        Initialize(playerController, penitentController);
        StartEventDay();

    }

    public void Initialize(PlayerController pController, PenitentController ptController)
    {
        playerController = pController;
        penitentController = ptController;

        // Obtener fases en children y inicializarlas
        fasesActuales = GetComponentsInChildren<MonoBehaviour>(true)
            .OfType<IFases>()
            .ToArray();
        //if (playerController.PlayerStatus.Day == 0)
        //{
        //    playerController.PlayerStatus.ResetAllStatus();
        //    RemoveEnergy(1);
        //}
        numberDay = playerController.PlayerStatus.Day;

        foreach (var fase in fasesActuales)
        {
            fase.Initialize(playerController, penitentController);
        }
        penitentController.UpdateDayPenitent(numberDay);
        Debug.Log($"Dia.Initialize: inicializadas {fasesActuales.Length} fases para el día {numberDay}");
    }

    public int GetNumberDay() => numberDay;
    public void AddEnergy(int energycount)
    {
        // Buscar el primer prefab de energía activo y retirarlo una sola vez
        foreach (var energy in energyPrefab)
        {
            if (energy.activeSelf) continue;

            if (playerController.PlayerStatus.Energy > playerController.PlayerStatus.MinEnergy &&
                playerController.PlayerStatus.Energy <= playerController.PlayerStatus.MaxEnergy)
            {
                playerController.PlayerStatus.DecreaseEnergy(energycount);
                energy.SetActive(true);
                Debug.Log("Energía añadida: " + energy.name);
                break; // salir para que sólo se retire una energía
            }
        }
    }
    public void RemoveEnergy(int energycount)
    {
        // Buscar el primer prefab de energía activo y retirarlo una sola vez
        foreach (var energy in energyPrefab)
        {
            if (!energy.activeSelf) continue;

            if (playerController.PlayerStatus.Energy > playerController.PlayerStatus.MinEnergy &&
                playerController.PlayerStatus.Energy <= playerController.PlayerStatus.MaxEnergy)
            {
                playerController.PlayerStatus.DecreaseEnergy(energycount);
                energy.SetActive(false);
                Debug.Log("Energía removida: " + energy.name);
                if (playerController.PlayerStatus.Energy == playerController.PlayerStatus.MinEnergy)
                {
                    Debug.Log("Energía al mínimo, activar evento de fin de día.");
                    if (playerController.PlayerStatus.Day == 0)
                    {
                        FadeController.Instance.FadeAndLoadScene("DÍA 1 - TARDE", fadeDuration);
                        //SceneManager.LoadScene("DÍA 1 - TARDE");
                    }
                    else if (playerController.PlayerStatus.Day >= 1)
                    {
                        FadeController.Instance.FadeAndLoadScene("DÍA 2 - TARDE", fadeDuration);
                        //SceneManager.LoadScene("DÍA 2 - TARDE");
                    }
                    // Aquí puedes activar un evento o llamar a un método para manejar el fin del día
                }
                break; // salir para que sólo se retire una energía
            }
        }
    }
    public void AddMoney(int moneycount)
    {
        // Lógica para añadir dinero al jugador
        playerController.PlayerStatus.Getmoney(moneycount);
        moneyText.text = playerController.PlayerStatus.Money.ToString();
        Debug.Log($"Dinero añadido: {moneycount}, Dinero Total: {moneyText.text}");
    }
    public void RemoveMoney(int moneycount)
    {
        // Lógica para quitar dinero al jugador
        if (playerController.PlayerStatus.Money > playerController.PlayerStatus.MinMoney)
        {
            playerController.PlayerStatus.Spendmoney(moneycount);
            moneyText.text = playerController.PlayerStatus.Money.ToString();
        }
        Debug.Log($"Dinero removido: {moneycount}, Dinero Total: {moneyText.text}");
    }
    public void AddFaith(int faithcount)
    {
        playerController.PlayerStatus.IncreaseFaith(faithcount);
        Debug.Log("Fe añadida: " + faithcount);
    }
    public void RemoveFaith(int faithcount)
    {
        playerController.PlayerStatus.DecreaseFaith(faithcount);
        Debug.Log("Fe removida: " + faithcount);
    }
    public void AddReputationPeople(int reputationcount)
    {
        playerController.PlayerStatus.IncreaseRepPueblo(reputationcount);
        Debug.Log("Reputación con el pueblo añadida: " + reputationcount);
    }
    public void RemoveReputationPeople(int reputationcount)
    {
        playerController.PlayerStatus.DecreaseRepPueblo(reputationcount);
        Debug.Log("Reputación con el pueblo removida: " + reputationcount);
    }

    private void StartEventDay()
    {
        eventDay = GetComponent<EventDayManager>();
        eventDay.Initialized(this, playerController, penitentController);
        eventDayActual = eventDay.GetTypeEvent(numberDay);
        if (eventDayActual != null)
        {
            Debug.Log($"Evento del día {numberDay} encontrado: {eventDayActual.GetTypeEventDay()}");
            // Aquí puedes activar el evento específico del día, por ejemplo:
            switch (eventDayActual.GetTypeEventDay())
            {
                case TypeEventDay.tutorial:
                    // Activar evento de tutorial
                    eventDayActual.ActivateEvent(); // Activar evento de tutorial
                    break;
                case TypeEventDay.day1:
                    eventDayActual.ActivateEvent(); // Activar evento del día 1
                    guiltyPenitent = eventDayActual.GuiltyPenitent; // Obtener el penitente culpable del evento del día 1
                    break;
                case TypeEventDay.day2:
                    // Activar evento del día 2
                    break;
                case TypeEventDay.day3:
                    // Activar evento del día 3
                    break;
                case TypeEventDay.day4:
                    // Activar evento del día 4
                    break;
                case TypeEventDay.day5:
                    // Activar evento del día 5
                    break;
                case TypeEventDay.day6:
                    // Activar evento del día 6
                    break;
                default:
                    Debug.LogWarning($"Evento no reconocido para el día {numberDay}");
                    break;
            }
        }
        else
        {
            Debug.Log($"No se encontró un evento para el día {numberDay}");
        }
    }

}


using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dia : MonoBehaviour
{
    [SerializeField] int numberDay;
    private IFases[] fasesActuales;
    private PlayerController playerController;
    private PenitentController penitentController;
    [SerializeField] private GameObject[] energyPrefab;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private float fadeDuration = 1f;


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
        Initialize(playerController, penitentController);
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
                if(playerController.PlayerStatus.Energy == playerController.PlayerStatus.MinEnergy)
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

}

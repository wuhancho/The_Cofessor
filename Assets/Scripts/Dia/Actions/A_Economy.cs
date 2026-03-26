using System;
using UnityEngine;

public class A_Economy : MonoBehaviour, IAcciones
{
    [SerializeField, IReadOnly] private float playerMoney; // Dinero que tiene el jugador total al iniciar la fase de economía
    [SerializeField] private float comidaCost; // Costo de cada unidad de comida
    [SerializeField] private float comidaAmount; // cantidad de comida que se obtiene
    [SerializeField] private float donacionAmount; // Porcentaje de dinero que se obtiene por cada unidad donada
    [SerializeField, IReadOnly] private float donations; // Dinero total que se obtiene por las donaciones
    [SerializeField, IReadOnly] private float salary; // Dinero total que se obtiene por el salario
    [SerializeField] private float salaryAmount; // multiplicador para calcular el salario total
    [SerializeField] private float churchCost; // Costo de la iglesia, cantidad fija que se resta al dinero del jugador
    [SerializeField] private float salaryCarlitos; // Cantidad fija que se le paga a Carlitos, se resta al dinero del jugador
    [SerializeField, IReadOnly] private float donationsForArzobispo; // Porcentaje de las donaciones que se le da al arzobispo, se resta al dinero del jugador
    [SerializeField] private float donationsForArzobispoAmount; // multiplicador para calcular las donaciones que se le da al arzobispo
    [SerializeField] private CanvasEconomy canvasEconomy;
    private float Sobornos; // Cantidad de dinero que se obtiene por los sobornos, se suma al dinero del jugador
    private PlayerController playerController;
    Dia dia;
    F_noche faseNoche;
    private int currentDay;

    public Dia Dia { get => dia; }
    public float PlayerMoney { get => playerMoney; }
    public float ComidaCost { get => comidaCost; }
    public float ComidaAmount { get => comidaAmount; }
    public float DonacionAmount { get => donacionAmount; }
    public float Donations { get => donations; }
    public float Salary { get => salary; }
    public float SalaryAmount { get => salaryAmount; }
    public float ChurchCost { get => churchCost; }
    public float SalaryCarlitos { get => salaryCarlitos; }
    public float DonationsForArzobispo { get => donationsForArzobispo; }

    public void Initialize(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    private void Awake()
    {
        canvasEconomy.gameObject.SetActive(false);
    }
    public void Initialize(PlayerController player, F_noche FaseNoche)
    {
        playerController = player;
        dia = FaseNoche.Dia;
        faseNoche = FaseNoche;
        playerMoney = playerController.PlayerStatus.Money;
        donations = Dia.GetDonations(donacionAmount);
        Sobornos = Dia.Sobornos;
        salary = Dia.GetSalary(salaryAmount);
        donationsForArzobispo = salary * donationsForArzobispoAmount;
    }

    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void DebugAccion()
    {
        throw new System.NotImplementedException();
    }

    public void EjecutarAccion(PlayerController playerController)
    {
        canvasEconomy.gameObject.SetActive(true);
        canvasEconomy.Initialize(playerController, this);
    }


    public void SetDay(int day)
    {
        currentDay = day;
    }

    public void TriggerAction()
    {
        EjecutarAccion(playerController);
    }


    internal void EndEconomy()
    {
        playerController.PlayerStatus.ResetMoney();
        playerController.PlayerStatus.Getmoney(canvasEconomy.Total);
        playerController.PlayerStatus.GetFood(canvasEconomy.ComidaGet);
        faseNoche.EndFase.Invoke();
    }
}

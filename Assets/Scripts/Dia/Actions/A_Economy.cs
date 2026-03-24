using System;
using UnityEngine;

public class A_Economy : MonoBehaviour, IAcciones
{
    [SerializeField] private float playerMoney;
    [SerializeField] private float comidaCost;
    [SerializeField] private float comidaAmount;
    [SerializeField] private float donacionAmount;
    [SerializeField] private float donations = 0.5f; // Porcentaje de dinero que se obtiene por cada unidad donada
    [SerializeField] private float salary;
    [SerializeField] private float salaryAmount;
    [SerializeField] private float churchCost;
    [SerializeField] private float salaryCarlitos;
    [SerializeField] private float donationsForArzobispo;
    [SerializeField] private CanvasEconomy canvasEconomy;
    private float Sobornos;
    private PlayerController playerController;
    Dia dia;
    F_noche faseNoche;
    private int currentDay;

    public Dia Dia { get => dia; }
    public float PlayerMoney { get => playerMoney; set => playerMoney = value; }
    public float ComidaCost { get => comidaCost; set => comidaCost = value; }
    public float ComidaAmount { get => comidaAmount; set => comidaAmount = value; }
    public float DonacionAmount { get => donacionAmount; set => donacionAmount = value; }
    public float Donations { get => donations;  }
    public float Salary { get => salary; }
    public float SalaryAmount { get => salaryAmount; set => salaryAmount = value; }
    public float ChurchCost { get => churchCost; set => churchCost = value; }
    public float SalaryCarlitos { get => salaryCarlitos; set => salaryCarlitos = value; }
    public float DonationsForArzobispo { get => donationsForArzobispo; set => donationsForArzobispo = value; }

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
        comidaAmount = playerController.PlayerStatus.Food; // Asignar la comida del jugador a la cantidad de comida

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
        faseNoche.EndAction.Invoke();
    }
}

using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasEconomy : MonoBehaviour
{
    private float playerMoney;
    private float comidaCost;
    private float comidaAmount;
    private float donacionAmount;
    private float donationsGet = 0.5f; // Porcentaje de dinero que se obtiene por cada unidad donada
    private float salary;
    private float churchCost;
    private float salaryCarlitos;
    private float donationsForArzobispo;
    private float sobornos;
    [SerializeField] private TextMeshProUGUI textSalary;
    [SerializeField] private TextMeshProUGUI textDonations;
    [SerializeField] private TextMeshProUGUI textChurchCost;
    [SerializeField] private TextMeshProUGUI textSalaryCarlitos;
    [SerializeField] private TextMeshProUGUI textSobornos;
    [SerializeField] private TextMeshProUGUI textDonationsForArzobispo;
    [SerializeField] private TextMeshProUGUI textComida;
    [SerializeField] private TextMeshProUGUI textTotal;

    private PlayerController PlayerController;
    Dia dia;
    A_Economy economi;
    private int currentDay;
    public void Initialize(PlayerController player,A_Economy economy)
    {
        this.PlayerController = player;
        dia = economy.Dia;
        economi = economy;
        SetDay(dia.GetNumberDay());
        playerMoney = player.PlayerStatus.Money;
        comidaCost = economi.ComidaCost;
        comidaAmount = economi.ComidaAmount;
        donacionAmount = economi.DonacionAmount;
        donationsGet = economi.DonationsGet;
        salary = economi.Salary;
        churchCost = economi.ChurchCost;
        salaryCarlitos = economi.SalaryCarlitos;
        donationsForArzobispo = economi.DonationsForArzobispo;
        donacionAmount = dia.GetDonations();
        sobornos = dia.Sobornos;

        UpdateEconomy();
        
    }
    public void SetDay(int day)
    {
        currentDay = day;
    }
    private void UpdateEconomy()
    {

    }
}
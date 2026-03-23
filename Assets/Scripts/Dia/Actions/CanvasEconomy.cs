using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasEconomy : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI textSalary;
    [SerializeField] private TextMeshProUGUI textDonations;
    [SerializeField] private TextMeshProUGUI textChurchCost;
    [SerializeField] private TextMeshProUGUI textSalaryCarlitos;
    [SerializeField] private TextMeshProUGUI textSobornos;
    [SerializeField] private TextMeshProUGUI textDonationsForArzobispo;
    [SerializeField] private TextMeshProUGUI textComida;
    [SerializeField] private TextMeshProUGUI textTotal;
    [SerializeField] private Button nextDay;
    [Header("Settings")]

    [SerializeField] private string textNextDay = "Siguiente día";
    // Variables para almacenar los valores económicos
    private float playerMoney;
    private float comidaCost;
    private float comidaAmount;
    private float donacionAmount;
    private float donationsGet; // Porcentaje de dinero que se obtiene por cada unidad donada
    private float salary;
    private float churchCost;
    private float salaryCarlitos;
    private float donationsForArzobispo;
    private float sobornos;


    private TextMeshProUGUI textButtonNext;
    private PlayerController PlayerController;
    Dia dia;
    A_Economy economi;
    private int currentDay;

    public void Initialize(PlayerController player, A_Economy economy)
    {
        this.PlayerController = player;
        textButtonNext.text = textNextDay;
        dia = economy.Dia;
        economi = economy;
        SetDay(dia.GetNumberDay());
        playerMoney = player.PlayerStatus.Money;
        comidaCost = economi.ComidaCost;
        comidaAmount = economi.ComidaAmount;
        donationsGet = economi.Donations;
        donacionAmount = economi.DonacionAmount;
        salary = economi.Salary;
        churchCost = economi.ChurchCost;
        salaryCarlitos = economi.SalaryCarlitos;
        donationsForArzobispo = economi.DonationsForArzobispo;
        sobornos = dia.Sobornos;

        UpdateEconomy();

        nextDay.onClick.AddListener(() =>
        {
            economi.EndEconomy();
        });
    }
    public void SetDay(int day)
    {
        currentDay = day;
    }
    private void UpdateEconomy()
    {
        textSalary.text = $"Salario: {salary}";
        textDonations.text = $"Donaciones: {donacionAmount} (x{donationsGet})";
        textChurchCost.text = $"Costo Iglesia: {churchCost}";
        textSalaryCarlitos.text = $"Salario Carlitos: {salaryCarlitos}";
        textSobornos.text = $"Sobornos: {sobornos}";
        textDonationsForArzobispo.text = $"Donaciones para Arzobispo: {donationsForArzobispo}";
        textComida.text = $"Comida: {comidaAmount} (x{comidaCost})";
        float total = salary + donationsGet - churchCost - salaryCarlitos - sobornos - donationsForArzobispo - (comidaAmount * comidaCost);
        textTotal.text = $"Total: {total}";
    }
}
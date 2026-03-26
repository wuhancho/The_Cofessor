using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasEconomy : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI textPlayerMoney;
    [SerializeField] private TextMeshProUGUI textSalary;
    [SerializeField] private TextMeshProUGUI textDonations;
    [SerializeField] private TextMeshProUGUI textChurchCost;
    [SerializeField] private TextMeshProUGUI textSalaryCarlitos;
    [SerializeField] private TextMeshProUGUI textSobornos;
    [SerializeField] private TextMeshProUGUI textDonationsForArzobispo;
    [SerializeField] private TextMeshProUGUI textComida;
    [SerializeField] private TextMeshProUGUI textTotal;
    [SerializeField] private TextMeshProUGUI textButtonNext;
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
    private float ganancias;
    private float gastos;
    private float comidaGet;
    private float total;




    private PlayerController playerController;
    Dia dia;
    A_Economy economi;
    private int currentDay;

    public float Total { get => total; }
    public float ComidaGet { get => comidaGet; }

    public void Initialize(PlayerController player, A_Economy economy)
    {
        playerController = player;
        textButtonNext.text = textNextDay;
        dia = economy.Dia;
        economi = economy;
        SetDay(dia.GetNumberDay());
        playerMoney = player.PlayerStatus.Money;
        comidaCost = economi.ComidaCost;
        donationsGet = economi.Donations;
        donacionAmount = economi.DonacionAmount;
        salary = economi.Salary;
        churchCost = economi.ChurchCost;
        salaryCarlitos = economi.SalaryCarlitos;
        donationsForArzobispo = economi.DonationsForArzobispo;
        sobornos = dia.Sobornos;
        comidaAmount = economi.ComidaAmount;
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
        textPlayerMoney.text = $"Dinero = {playerMoney}$";
        textSalary.text = $"Salario = {salary}$";
        textDonations.text = $"Donaciones = {donacionAmount}$";
        textChurchCost.text = $"Gastos de Iglesia = {churchCost}$";
        textSalaryCarlitos.text = $"Salario Carlitos = {salaryCarlitos}$";
        textSobornos.text = $"Sobornos = {sobornos}$";
        textDonationsForArzobispo.text = $"Donaciones para Arzobispo = {donationsForArzobispo}$";
        textComida.text = $"Comida = {comidaAmount * comidaCost}$";
        comidaGet = comidaAmount;
        ganancias = salary + donationsGet + sobornos;
        gastos = churchCost + salaryCarlitos + donationsForArzobispo + comidaAmount * comidaCost;
        total = (playerMoney + ganancias) - gastos;
        textTotal.text = $"Total: {total}$";
    }
}
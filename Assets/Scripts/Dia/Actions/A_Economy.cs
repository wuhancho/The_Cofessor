using TMPro;
using UnityEngine;

public class A_Economy : MonoBehaviour, IAcciones
{
    [SerializeField] private float PlayerMoney;
    [SerializeField] private float ComidaCost;
    [SerializeField] private float ComidaAmount;
    [SerializeField] private float DonacionAmount;
    [SerializeField] private float DonationsGet = 0.5f; // Porcentaje de dinero que se obtiene por cada unidad donada
    [SerializeField] private float Salary;
    [SerializeField] private float ChurchCost;
    [SerializeField] private float salaryCarlitos;
    [SerializeField] private float DonationsForArzobispo;
    private float Sobornos;
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
    private int currentDay;

    public void EjecutarAccion(PlayerController playerController)
    {
        throw new System.NotImplementedException();
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }

    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(PlayerController playerController)
    {
        this.PlayerController = playerController;
        dia = FindFirstObjectByType<Dia>();
        DonacionAmount = dia.GetDonations();
        Sobornos = dia.Sobornos;
        UpdateEconomy();
    }

    public void DebugAccion()
    {
        throw new System.NotImplementedException();
    }

    public void SetDay(int day)
    {
        currentDay = day;
    }
    public void UpdateEconomy()
    {

    }
}
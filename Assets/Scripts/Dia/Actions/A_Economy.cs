using UnityEngine;

public class A_Economy : MonoBehaviour, IAcciones
{
    [SerializeField] private float playerMoney;
    [SerializeField] private float comidaCost;
    [SerializeField] private float comidaAmount;
    [SerializeField] private float donacionAmount;
    [SerializeField] private float donationsGet = 0.5f; // Porcentaje de dinero que se obtiene por cada unidad donada
    [SerializeField] private float salary;
    [SerializeField] private float churchCost;
    [SerializeField] private float salaryCarlitos;
    [SerializeField] private float donationsForArzobispo;
    [SerializeField] private CanvasEconomy canvasEconomy;
    private float Sobornos;
    private PlayerController PlayerController;
    Dia dia;
    private int currentDay;

    public Dia Dia { get => dia; }
    public float PlayerMoney { get => playerMoney; set => playerMoney = value; }
    public float ComidaCost { get => comidaCost; set => comidaCost = value; }
    public float ComidaAmount { get => comidaAmount; set => comidaAmount = value; }
    public float DonacionAmount { get => donacionAmount; set => donacionAmount = value; }
    public float DonationsGet { get => donationsGet; set => donationsGet = value; }
    public float Salary { get => salary; set => salary = value; }
    public float ChurchCost { get => churchCost; set => churchCost = value; }
    public float SalaryCarlitos { get => salaryCarlitos; set => salaryCarlitos = value; }
    public float DonationsForArzobispo { get => donationsForArzobispo; set => donationsForArzobispo = value; }

    public void Initialize(PlayerController playerController)
    {
        PlayerController = playerController;
    }
    public void Initialize(PlayerController player,F_noche FaseNoche)
    {
        this.PlayerController = player;
        dia = FaseNoche.Dia;
        donationsGet = Dia.GetDonations();
        Sobornos = Dia.Sobornos;
        canvasEconomy.Initialize(player,this);
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
        throw new System.NotImplementedException();
    }


    public void SetDay(int day)
    {
        throw new System.NotImplementedException();
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

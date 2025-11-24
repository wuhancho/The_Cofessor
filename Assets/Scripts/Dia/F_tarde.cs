using System.Linq;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class F_tarde : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    private IAcciones[] acciones;
    [SerializeField] private Dia dia;
    [SerializeField] private PlayerController playerController;


    private void Awake()
    {
        dia = GetComponentInParent<Dia>();
        RefrescarAcciones();
        Initialize(dia.GetComponent<PlayerController>());
        foreach (var accion in acciones)
        {
            //accion.InitializePlayer(playerController);
            A_confecciones accionType = (A_confecciones)accion;
            accionType.SetDay(dia.GetNumberDay());
            //accionType.InitializePlayer(playerController);
        }

    }
    private void Update()
    {
        foreach (var accion in acciones)
        {
            A_confecciones accionType = (A_confecciones)accion;
            accionType.onConfession.AddListener(ChangeStatesPlayer);
        }
    }

    public void RefrescarAcciones()
    {
        acciones = GetComponentsInChildren<MonoBehaviour>(includeInactiveChildren)
            .OfType<IAcciones>()
            .ToArray();
        foreach (var accion in acciones)
        {
            Debug.Log("Accion encontrada en F_tarde: " + accion.GetType().Name);
        }
    }

    public IAcciones[] GetAcciones() => acciones;

    public void Initialize(PlayerController pController)
    {
        playerController = pController;
    }
    public void ChangeStatesPlayer(DialogNode dialogNode)
    {
        if (dialogNode.GetFaithCost() != 0)
        {
            if (dialogNode.GetFaithCost()<0)
            {
                playerController.PlayerStatus.DecreaseFaith(dialogNode.GetFaithCost());
                return;
            }
            else if (dialogNode.GetFaithCost() >= 0)
            {
                playerController.PlayerStatus.IncreaseFaith(dialogNode.GetFaithCost());
            }
        }
        if( dialogNode.GetRepIglesiaCost() != 0)
        {
            if (dialogNode.GetRepIglesiaCost()<0)
            {
                playerController.PlayerStatus.DecreaseRepIglesia(dialogNode.GetRepIglesiaCost());
                return;
            }
            else if (dialogNode.GetRepIglesiaCost() >= 0)
            {
                playerController.PlayerStatus.IncreaseRepIglesia(dialogNode.GetRepIglesiaCost());
            }
        }
        if (dialogNode.GetRepPuebloCost() != 0)
        {
            if (dialogNode.GetRepPuebloCost()<0)
            {
                playerController.PlayerStatus.DecreaseRepPueblo(dialogNode.GetRepPuebloCost());
                return;
            }
            else if (dialogNode.GetRepPuebloCost() >= 0)
            {
                playerController.PlayerStatus.IncreaseRepPueblo(dialogNode.GetRepPuebloCost());
            }
        }
        if (dialogNode.GetSobornoCost() != 0)
        {
            if (playerController.PlayerStatus.Money < dialogNode.GetSobornoCost())
            {
                Debug.Log("No tienes suficiente dinero para sobornar.");
                return;
            }
            else if (playerController.PlayerStatus.Money >= dialogNode.GetSobornoCost())
            {
                playerController.PlayerStatus.Spendmoney(dialogNode.GetSobornoCost());
            }
            //playerController.ChangeMoney(dialogNode.GetSobornoCost());
        }
    }
}

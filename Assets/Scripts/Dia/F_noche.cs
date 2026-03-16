using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class F_noche : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    private IAcciones[] acciones;
    private Dia dia;
    private PlayerController playerController;
    private PenitentController penitentController;
    public Dia Dia {get => dia; }


    public UnityEvent EndAction;

    private void Awake()
    {
        RefrescarAcciones();
        EndAction.AddListener(() =>
        {
            Debug.Log("F_noche: EndAction invoked");
        });
    }

    [ContextMenu("Refrescar acciones")]
    public void RefrescarAcciones()
    {
        acciones = GetComponentsInChildren<MonoBehaviour>(includeInactiveChildren)
            .OfType<IAcciones>()
            .ToArray();
    }

    public IAcciones[] GetAcciones() => acciones;
    private void InitializeActions()
    {
        dia = GetComponentInParent<Dia>();
        RefrescarAcciones();
        foreach (var accion in acciones)
        {
            if (accion is A_Decision accionType)
            {
                Debug.Log("Inicializando A_Decision en F_tarde");
                accionType.Initialize(playerController, penitentController);
                //accion.SetDay(dia.GetNumberDay());
                Debug.Log("Suscribiendo AEconomy a onEndAction de A_Decision en F_noche");
                accionType.onEndAction += () => AEconomy();
            }
            accion.SetDay(dia.GetNumberDay());
            
        }
    }
    public void Initialize(PlayerController pController, PenitentController ptController)
    {
        playerController = pController;
        penitentController = ptController;
        InitializeActions();
    }
    private void AEconomy()
    {
        Debug.Log("Ejecutando AEconomy desde F_noche");
        foreach (var accion in acciones)
        {
            if (accion is A_Economy accionEconomy)
            {
                Debug.Log("Ejecutando A_Economy en F_noche");
                accionEconomy.Initialize(playerController,this);
            }
        }
    }
}

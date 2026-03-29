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
    public Dia Dia { get => dia; }


    public UnityEvent EndFase;

    private void Awake()
    {
        RefrescarAcciones();
        EndFase.AddListener(() =>
        {
            Debug.Log("F_noche: EndFase invoked");
            Dia.EndDay();
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
        Debug.Log("Inicializando acciones en F_noche para el día " + dia.GetNumberDay());
        RefrescarAcciones();
        foreach (var accion in acciones)
        {
            if (dia.GetNumberDay() == 0 && accion is A_Decision)
            {
                Debug.Log("Desactivando A_Decision en F_noche del día 0");
                ((A_Decision)accion).gameObject.SetActive(false);
            }
            if (accion is A_Decision accion_Decision && accion_Decision.gameObject.activeSelf == true)
            {
                Debug.Log("Inicializando A_Decision en F_tarde");
                accion_Decision.Initialize(playerController, penitentController);
                accion_Decision.SetDay(dia.GetNumberDay());
                Debug.Log("Suscribiendo AEconomy a onEndAction de A_Decision en F_noche");
                accion_Decision.onEndAction += () => AEconomy();
            }
            if (accion is A_Economy accionEconomy)
            {
                Debug.Log("Inicializando A_Economy en F_noche");
                accionEconomy.SetDay(dia.GetNumberDay());
                accionEconomy.Initialize(playerController, this);

            }
            //accion.SetDay(dia.GetNumberDay());
            //accion.SetDay(dia.GetNumberDay());

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
                accionEconomy.Initialize(playerController, this);
                accionEconomy.TriggerAction();
            }
        }
    }
    public void ActivateFase()
    {
        Debug.Log("Activando fase F_noche");
        if (dia.GetNumberDay() == 0)
        {
            Debug.Log("Día 0: Desactivando A_Decision en F_noche");
            foreach (var accion in acciones)
            {
                if (accion is A_Decision accionDecision)
                {
                    accionDecision.gameObject.SetActive(false);
                    continue;
                }
                if (accion is A_Economy accionEconomy)
                {
                    Debug.Log("Día 0: Inicializando A_Economy en F_noche");
                    //accionEconomy.Initialize(playerController, this);
                    FadeController.Instance.FadeIn(2f, () =>
                    {
                        accionEconomy.TriggerAction();
                        FadeController.Instance.FadeOut(2f);
                    });
                }
            }
        }
        else if (dia.GetNumberDay() > 0)
        {
            Debug.Log("Día " + dia.GetNumberDay() + ": Activando acciones en F_noche");
            foreach (var accion in acciones)
            {
                if (accion is A_Decision accionDecision)
                {
                    Debug.Log("Activando A_Decision en F_noche");
                    accionDecision.gameObject.SetActive(true);
                    //accionDecision.Initialize(playerController, penitentController);
                    FadeController.Instance.FadeIn((2f),() =>
                    {
                        accionDecision.TriggerAction();
                        FadeController.Instance.FadeOut(2f);
                    });
                }
            }
        }
    }
}

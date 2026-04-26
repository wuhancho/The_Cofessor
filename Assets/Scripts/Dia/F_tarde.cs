using NUnit.Framework;
using System.Linq;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;
using UnityEngine.Events;

public class F_tarde : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    private IAcciones[] acciones;
    private Dia dia;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PenitentController penitentController;


    [Header("Configuración de la acción misa hecha/ no hecha")]
    [Space]
    [Tooltip("Evento que escucha que la misa esta hecha.\n objetos necesarios")]
    public UnityEvent onMisaDone;
    [Tooltip("Evento que escucha que la misa no esta hecha.\n objetos necesarios")]
    public UnityEvent onMisaNotDone;

    //private void OnBeforeSerialize()
    //{
    //    foreach (var accion in acciones)
    //    {
    //        accion.InitializePlayer(playerController);
    //        //A_confecciones accionType = (A_confecciones)accion;
    //        //accionType.SetDay(dia.GetNumberDay());
    //        ////accionType.InitializePlayer(playerController);
    //    }
    //}

    private void Awake()
    {
        //InitializeActions();

    }

    private void InitializeActions()
    {
        dia = GetComponentInParent<Dia>();
        RefrescarAcciones();
        foreach (var accion in acciones)
        {
            if (accion is A_confessions accionType)
            {
                Debug.Log("Inicializando A_confecciones en F_tarde");
                accionType.Initialize(playerController, penitentController);
                //accion.SetDay(dia.GetNumberDay());
            }
            if (accion is NotesBook notesBook)
            {
                Debug.Log("Inicializando NotesBook en F_tarde");
                notesBook.Initialize(playerController, penitentController);
                //accion.SetDay(dia.GetNumberDay());
            }
            accion.Initialize(playerController);
            Debug.Log($"F_tarde - Acción inicializada: {accion.GetType().Name} para el día {dia.GetNumberDay()}");
            accion.SetDay(dia.GetNumberDay());
        }
    }

    public void RefrescarAcciones()
    {
        acciones = GetComponentsInChildren<MonoBehaviour>(includeInactiveChildren)
            .OfType<IAcciones>()
            .ToArray();
        //foreach (var accion in acciones)
        //{
        //    Debug.Log("Accion encontrada en F_tarde: " + accion.GetType().Name);
        //}
    }

    public IAcciones[] GetAcciones() => acciones;

    public void Initialize(PlayerController pController, PenitentController ptController)
    {
        //Debug.Log($"Inicializando F_tarde con las variables {pController.name}  {ptController.name}");
        this.playerController = pController;
        this.penitentController = ptController;
        InitializeActions();
    }

    public void CheckMisaDone()
    {
        MisaDone();
    }

    public bool MisaDone()
    {
        if (playerController.PlayerStatus.MisaDone == true)
        {
            onMisaDone.Invoke();
            return true;
        }
        else
        {
            onMisaNotDone.Invoke();
            return false;
        }
    }

}

using System.Linq;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class F_tarde : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    private IAcciones[] acciones;
    [SerializeField] private Dia dia;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PenitentController penitentController;

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
        dia = GetComponentInParent<Dia>();
        penitentController = GetComponentInParent<PenitentController>();
        RefrescarAcciones();
        //Initialize(dia.GetComponent<PlayerController>());
        foreach (var accion in acciones)
        {
            //accion.InitializePlayer(playerController);
            A_confecciones accionType = (A_confecciones)accion;
            accionType.SetDay(dia.GetNumberDay());
            
            //accionType.InitializePlayer(playerController);
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
        playerController = pController;
        penitentController = ptController;
        foreach (var accion in acciones)
        {
            accion.Initialize(playerController);
            if (accion is A_confecciones accionType)
            {
                accionType.Initialize(playerController,penitentController);
            }
        }
    }
    
}

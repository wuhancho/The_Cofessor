using System.Linq;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class F_tarde : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    private IAcciones[] acciones;
    private Dia dia;
    private PlayerController playerController;
    private PenitentController penitentController;

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
        RefrescarAcciones();
        foreach (var accion in acciones)
        {
            accion.Initialize(playerController);

            if (accion is A_confessions accionType)
            {
                Debug.Log("Inicializando A_confecciones en F_tarde");
                accionType.Initialize(playerController, penitentController);
                //accion.SetDay(dia.GetNumberDay());
            }
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
    }

}

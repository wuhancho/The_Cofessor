using System.Linq;
using UnityEngine;

public class F_mañana : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    private IAcciones[] acciones;
    private Dia dia;
    private PlayerController playerController;
    private PenitentController penitentController;
    

    [ContextMenu("Refrescar acciones")]
    public void RefrescarAcciones()
    {
        var nuevas = GetComponentsInChildren<MonoBehaviour>(includeInactiveChildren)
            .OfType<IAcciones>()
            .ToArray();

        // Si cambian las referencias, reemplaza y (re)inicializa
        if (acciones == null || acciones.Length != nuevas.Length)
        {
            acciones = nuevas;
        }
        else
        {
            // opcional: comparar elementos por InstanceID si se requiere exactitud
            acciones = nuevas;
        }
    }

    public IAcciones[] GetAcciones() => acciones;

    public void Initialize(PlayerController pController, PenitentController ptController)
    {
        playerController = pController;
        penitentController = ptController;
        dia = GetComponentInParent<Dia>();

        // Asegúrate de refrescar antes de inicializar
        RefrescarAcciones();

        // Inicializar cada acción y registrar si falta playerController
        foreach (var accion in acciones)
        {
            if (accion == null)
            {
                Debug.LogWarning("F_mañana.Initialize: accion NULL encontrada.");
                continue;
            }

            accion.Initialize(playerController);
            accion.SetDay(dia != null ? dia.GetNumberDay() : 0);
            if (dia.GetNumberDay() >= 1)
            {
                playerController.ChangeSetCleaned(false);
            }
            accion.DebugAccion();

            Debug.Log($"F_mañana - Inicializada {((MonoBehaviour)accion).name} con playerController={(playerController==null?"NULL":playerController.name)}");
        }
    }
    
}

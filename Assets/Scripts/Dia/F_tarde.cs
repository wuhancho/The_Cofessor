using System.Linq;
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
            A_confecciones accionType = (A_confecciones)accion;
            accionType.SetDay(dia.GetNumberDay());
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
}

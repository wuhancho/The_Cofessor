using System.Linq;
using UnityEngine;

public class F_mañana : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    [SerializeField] private IAcciones[] acciones;
    private void Awake()
    {
        RefrescarAcciones();
    }

    [ContextMenu("Refrescar acciones")]
    public void RefrescarAcciones()
    {
        acciones = GetComponentsInChildren<MonoBehaviour>(includeInactiveChildren)
            .OfType<IAcciones>()
            .ToArray();
    }

    public IAcciones[] GetAcciones() => acciones;

    private void Start()
    {
        Debug.Log("Fase de la mañana iniciada con " + acciones.Length + " acciones disponibles.");
    }

}

using System.Linq;
using UnityEngine;

public class F_tarde : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    private IAcciones[] acciones;

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
}

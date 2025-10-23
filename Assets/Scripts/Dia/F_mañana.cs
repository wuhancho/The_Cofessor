using System.Linq;
using UnityEngine;

public class F_mañana : MonoBehaviour, IFases
{
    [SerializeField] private bool includeInactiveChildren = true;
    [SerializeField] private IAccionesEnergia[] acciones;

    private void Awake()
    {
        RefrescarAcciones();
    }

    [ContextMenu("Refrescar acciones")]
    public void RefrescarAcciones()
    {
        acciones = GetComponentsInChildren<IAccionesEnergia>(includeInactiveChildren);
    }

    public IAcciones[] GetAcciones() => acciones;

    private void Start()
    {
        Debug.Log("Fase de la mañana iniciada con " + acciones.Length + " acciones disponibles.");
        
    }

    public void Initialize(PlayerStatus playerStatus)
    {
        throw new System.NotImplementedException();
    }
}

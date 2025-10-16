using UnityEngine;

public class F_noche : MonoBehaviour, IFases
{
    [SerializeField] private IAcciones[] accion;
    public IAcciones GetAccion()
    {
        throw new System.NotImplementedException();
    }
}

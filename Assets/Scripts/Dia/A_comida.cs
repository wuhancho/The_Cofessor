using UnityEngine;

public class A_comida : MonoBehaviour, IAcciones
{
    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void EjecutarAccion(PlayerStatus playerStatus)
    {
        Debug.Log("Ejecutando acción de Comida.");
        // Aquí va la lógica específica para la acción de Comida.
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

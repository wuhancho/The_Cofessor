using UnityEngine;

public class A_comida : MonoBehaviour, IAcciones
{
    [SerializeField] private float cuantityFood;
    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void EjecutarAccion(PlayerStatus playerStatus)
    {
        Debug.Log("Ejecutando acción de Comida.");
        // Aquí va la lógica específica para la acción de Comida.
        playerStatus.Food 
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

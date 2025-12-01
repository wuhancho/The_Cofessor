using UnityEngine;

public class A_comida : MonoBehaviour, IAcciones
{
    [SerializeField] private float cuantityFood;
    private int day;

    public void SetDay(int day)
    {
        throw new System.NotImplementedException();
    }

    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void EjecutarAccion(PlayerController playerController)
    {
        Debug.Log("Ejecutando acción de Comida.");
        // Aquí va la lógica específica para la acción de Comida.
        Debug.Log($"Aumentando comida en {playerController.PlayerStatus.Food}.");
    }

    public void Initialize(PlayerController playerController)
    {
        throw new System.NotImplementedException();
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
}

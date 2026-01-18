using TMPro;
using UnityEngine;

public class A_comida : MonoBehaviour, IAcciones
{
    [SerializeField] private float cuantityFood;
    [SerializeField] private GameObject PrefabFood;
    [SerializeField] private TextMeshProUGUI FoodText;
    [SerializeField] private int day;
    private PlayerController _playerController;



    public void SetDay(int day)
    {
        this.day = day;
    }

    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void EjecutarAccion(PlayerController playerController)
    {
        Debug.Log("Ejecutando acción de Comida.");
        // Aquí va la lógica específica para la acción de Comida.
        Debug.Log($"En el almacén quedan {playerController.PlayerStatus.Food} paquetes de pan.");
        FoodText.text = $"En el almacén quedan {playerController.PlayerStatus.Food} paquetes de pan.";
    }

    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        //SetDay(_playerController.PlayerStatus.Day);
        cuantityFood = _playerController.PlayerStatus.Food;
        
    }
    public void DebugAccion()
    {
        Debug.Log($"{_playerController.PlayerStatus.Day} - Acción de Comida - Día: {day}, cantidad de comida: {cuantityFood}");
    }

    public void TriggerAction()
    {
        PrefabFood.SetActive(true);
        DebugAccion();
        EjecutarAccion(_playerController);
    }
}

using UnityEngine;

public class D_Player : MonoBehaviour, INormalDialogs, IExpecialDialogs
{
    [SerializeField] string[] normalDialogs;
    [SerializeField] string[] expecialDialogs;

    void Start()
    {
        // Llama al método de instancia, no a través de la interfaz
        
    }

    // Implementación del método de la interfaz IExpecialDialogs

}

using UnityEngine;

public class D_Player : MonoBehaviour, INormalDialogs, IExpecialDialogs
{
    [SerializeField] string[] normalDialogs;
    [SerializeField] string[] expecialDialogs;

    void Start()
    {
        // Llama al m�todo de instancia, no a trav�s de la interfaz
        
    }

    // Implementaci�n del m�todo de la interfaz IExpecialDialogs

}

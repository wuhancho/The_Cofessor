using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Esto permite que otros scripts lo encuentren usando LevelManager.Instance
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Para que no se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Esta es la función que el MenuPrincipalSFX está buscando
    public void LoadScene(string sceneName, string transitionName)
    {
        // Por ahora cargará la escena directamente. 
        // Si luego haces un sistema de animaciones (CrossFade), aquí es donde lo activarías.
        Debug.Log("Cargando: " + sceneName + " con transición: " + transitionName);
        SceneManager.LoadScene(sceneName);
    }
}
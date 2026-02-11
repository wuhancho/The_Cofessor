using UnityEngine;
using UnityEngine.SceneManagement;

public class BUCLE : MonoBehaviour
{


    public void Play()
    {
        SceneManager.LoadScene("DÍA 1 - MAÑANA");
    }

   
    public void Quit()
    {
        Application.Quit();
    }
}

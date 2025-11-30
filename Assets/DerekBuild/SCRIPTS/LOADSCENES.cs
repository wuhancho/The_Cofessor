using UnityEngine;
using UnityEngine.SceneManagement;

public class LOADSCENES : MonoBehaviour
{


    public void Play()
    {
        SceneManager.LoadScene("DÍA 1 - TARDE");
    }

   
    public void Quit()
    {
        Application.Quit();
    }
}

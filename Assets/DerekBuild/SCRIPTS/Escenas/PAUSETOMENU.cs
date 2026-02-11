using UnityEngine;
using UnityEngine.SceneManagement;

public class PAUSETOMENU : MonoBehaviour
{


    public void Play()
    {
        SceneManager.LoadScene("MAIN MENU");
    }

   
    public void Quit()
    {
        Application.Quit();
    }
}

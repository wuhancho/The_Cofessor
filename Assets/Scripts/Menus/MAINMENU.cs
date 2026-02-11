using UnityEngine;
using UnityEngine.SceneManagement;

public class MAINMENU : MonoBehaviour
{
    public float idleTimeLimit = 120f; // 2 minutos
    private float idleTimer;

    void Update()
    {
        // Si detecta alguna entrada, reinicia el contador
        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;
        }

        // Si pasa el tiempo sin actividad, vuelve a la intro
        if (idleTimer >= idleTimeLimit)
        {
            SceneManager.LoadScene("INTROMENU"); // nombre de la escena con tu video
        }
    }


    public void Play()
    {
        SceneManager.LoadScene("DÍA 1 - MAÑANA");
    }

   
    public void Quit()
    {
        Application.Quit();
    }
}

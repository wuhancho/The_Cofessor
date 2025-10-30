using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el componente VideoPlayer en el Inspector
    public string nextSceneName = "MAINMENU"; // Nombre de la escena del menú principal

    void Start()
    {
        // Cuando el video termina, ejecuta OnVideoEnd
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

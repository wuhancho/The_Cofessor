using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSFX : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        LoadVolume();
        // Asegúrate de que ManagerDeMúsica esté en la escena o dará error
        if (ManagerDeMúsica.Instance != null)
            ManagerDeMúsica.Instance.PlayMusic("DÍA 1 - MAÑANA");
    }

    public void Play()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.LoadScene("DÍA 1 - MAÑANA", "CrossFade");

        if (ManagerDeMúsica.Instance != null)
            ManagerDeMúsica.Instance.PlayMusic("DÍA 1 - MAÑANA");
    }

    public void UpdateMasterVolume(float volume)
    {
        // "MasterVolume" nombre del parámetro expuesto en el AudioMixer
        audioMixer.SetFloat("VolumenMaestro", volume);
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("SonidoAmbiental", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("EfectosdeSonido", volume);
    }

    // --- Guardado y Carga ---

    public void SaveVolume()
    {
        // Guardamos Maestro
        audioMixer.GetFloat("VolumenMaestro", out float masterVol);
        PlayerPrefs.SetFloat("VolumenMaestro", masterVol);

        // Guardamos Música
        audioMixer.GetFloat("SonidoAmbiental", out float musicVol);
        PlayerPrefs.SetFloat("SonidoAmbiental", musicVol);

        // Guardamos SFX
        audioMixer.GetFloat("EfectosdeSonido", out float sfxVol);
        PlayerPrefs.SetFloat("EfectosdeSonido", sfxVol);

        PlayerPrefs.Save(); // Forzamos el guardado en el disco
    }

    public void LoadVolume()
    {
        // 
        masterSlider.value = PlayerPrefs.GetFloat("VolumenMaestro", 5f);
        musicSlider.value = PlayerPrefs.GetFloat("SonidoAmbiental", 5f);
        sfxSlider.value = PlayerPrefs.GetFloat("EfectosdeSonido", 5f);

        // Aplicamos los valores al Mixer inmediatamente
        UpdateMasterVolume(masterSlider.value);
        UpdateMusicVolume(musicSlider.value);
        UpdateSoundVolume(sfxSlider.value);
    }
}
   
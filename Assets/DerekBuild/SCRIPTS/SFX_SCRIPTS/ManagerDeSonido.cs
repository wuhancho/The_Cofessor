using UnityEngine;

public class ManagerDeSonidos : MonoBehaviour
{
    public static ManagerDeSonidos Instance;

    [SerializeField]
    private LibreriaDeSonidos sfxLibrary; // Declara una referencia a la clase SoundLibrary
    [SerializeField]
    private AudioSource sfx2DSource; // referencia a un componente AudioSource que se usara
                       // para reproducir todos los sonidos 2D (sonidos que no se ven afectados
                       // por la posicion o la distancia, como los clics de menu o la musica)

    private void Awake()
    {
        if (Instance != null) // Verifica si ya existe una instancia del Manager en la escena
        {
            Destroy(gameObject); // Si existe, destruye el nuevo GameObject.
                                 // Esto asegura que solo exista una copia del Manager
        }
        else // Si no existe una instancia por que es la primera vez que se carga:
        {
            Instance = this; // asigna la instancia actual a la variable estatica Instance,
                             // haciendola accesible globalmente
            DontDestroyOnLoad(gameObject); // Por otro lado, le decimos a Unity que no destruya
                   // este GameObject cuando se carguen nuevas escenas. Esto garantiza que la musica
                   // y los ajustes de sonido persistan a lo largo de todo el juego.
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos) // Metodo Base, porque crea temporalmente
                  // un AudioSource en la posicion pos, reproduce el clip,
                         // y luego se destruye automaticamente

    {
        if (clip != null) 
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName) // Llama internamente al metodo base,
        // pero primero usa sfxLibrary.GetClipFromName(soundName) para obtener el clip
        // aleatorio de la libreria, usando solo el nombre del grupo. Esto desacopla la
        // logica de sonido del codigo del juego.
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
        // Reproduce el clip de sonido una sola vez. Usar PlayOneShot es
        // mejor que simplemente Play(), ya que permite que se reproduzcan varios
        // sonidos 2D al mismo tiempo sin interrumpirse
    }
}

using UnityEngine;

[System.Serializable] // le dice a Unity que esta estructura debe ser editable y visible en el Inspector
public struct SoundEffect //define como se vera un solo grupo de sonido
{
    public string groupID;
    public AudioClip[] clips;
} // contiene un array de AudioClip, que permite asociar multiples archivos de audio
  // al mismo groupID. Esta es la clave para anadir variación y evitar que los sonidos
  // repetitivos suenen monotonos

public class LibreriaDeSonidos : MonoBehaviour
{
    public SoundEffect[] soundEffects; // variable central.
    // Aqui es donde se almacenaran todos los grupos de sonidos definidos en el Inspector

    public AudioClip GetClipFromName(string name) // método se encarga de buscar y devolver un clip de audio
    {
        foreach (var soundEffect in soundEffects) // se utiliza un bucle foreach
                                   // para recorrer cada efecto de sonido almacenado en la libreria
        {
            if (soundEffect.groupID == name) // Dentro del bucle,
                       // compara el groupID del efecto de sonido actual con el nombre (name)
            {
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
                // Si encuentra una coincidencia, en lugar de devolver el primer clip,
                // utiliza Random.Range para seleccionar un clip al azar del array clips
                // asociado a ese grupo, devolviendo ese clip para su reproduccion
            }
        }
        return null;
        // Si el bucle termina sin encontrar el groupID solicitado,
        // el metodo simplemente devuelve null, actuando como una senal de que el
        // sonido no existe en la libreria
    }
}
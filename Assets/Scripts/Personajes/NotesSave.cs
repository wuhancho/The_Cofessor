using UnityEngine;


public class NotesSave : MonoBehaviour
{
    [SerializeField] private GameObject namePenitent;
    [SerializeField] private GameObject[] notesLies;
    [SerializeField] private GameObject[] notesTruths;
    [SerializeField] private GameObject[] notesUnique;

    public GameObject NamePenitent { get => namePenitent; }
    public GameObject[] NotesLies { get => notesLies; }
    public GameObject[] NotesTruths { get => notesTruths; }
    public GameObject[] NotesUnique { get => notesUnique; }
    public void GetTruthNote(int index, out GameObject note)
    {
        foreach (GameObject noteTruth in notesTruths)
        {
            string[] parts = noteTruth.name.Split('.');
            if (parts.Length > 1 && int.TryParse(parts[2], out int noteIndex) && noteIndex == index)
            {
                note = noteTruth;
                return;
            }
        }
        note = null;
    }
    public void GetLieNote(int index, out GameObject note)
    {
        foreach (GameObject noteLie in notesLies)
        {
            string[] parts = noteLie.name.Split('.');
            if (parts.Length > 1 && int.TryParse(parts[2], out int noteIndex) && noteIndex == index)
            {
                note = noteLie;
                return;
            }
        }
        note = null;
    }
    public void GetUniqueNote(int index, out GameObject note)
    {
        foreach (GameObject noteUnique in notesUnique)
        {
            string[] parts = noteUnique.name.Split('.');
            if (parts.Length > 1 && int.TryParse(parts[1], out int noteIndex) && noteIndex == index)
            {
                note = noteUnique;
                return;
            }
        }
        note = null;
    }
}

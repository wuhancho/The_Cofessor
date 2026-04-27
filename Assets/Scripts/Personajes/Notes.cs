using UnityEngine;


public class Notes : MonoBehaviour
{
    [SerializeField] private GameObject namePenitent;

    [SerializeField] private GameObject[] notesLies;
    [SerializeField] private GameObject[] notesTruths;
    [SerializeField] private GameObject[] notesUnique;

    public GameObject NamePenitent { get => namePenitent; }
    public GameObject[] NotesLies { get => notesLies; }
    public GameObject[] NotesTruths { get => notesTruths; }
    public GameObject[] NotesUnique { get => notesUnique; }
    private void GetTruthNote(int index, out GameObject note)
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
    private void GetLieNote(int index, out GameObject note)
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
    private void GetUniqueNote(int index, out GameObject note)
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
    private void GetNote(NoteType noteType, int index, out GameObject note)
    {
        switch (noteType)
        {
            case NoteType.Lie:
                GetLieNote(index, out note);
                break;
            case NoteType.Truth:
                GetTruthNote(index, out note);
                break;
            case NoteType.Unique:
                GetUniqueNote(index, out note);
                break;
            default:
                note = null;
                break;
        }
    }

    public void WriteNote(NoteType noteType, int index)
    {
        GetNote(noteType, index, out GameObject note);
        if (note != null)
        {
            note.SetActive(true);
        }
    }
}

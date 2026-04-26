using System.Collections;
using TMPro;
using UnityEngine;

public enum NoteType
{
    None,
    Lie,
    Truth,
    Unique
}
[RequireComponent(typeof(TextMeshProUGUI))]
public class NoteData : MonoBehaviour
{
    [Header("Note Data")]
    [SerializeField, IReadOnly] private int indexNote;
    [Tooltip("Text to display on the note")]
    [SerializeField, IReadOnly] private string noteText;
    [Tooltip("Type of the note, can be Lie, Truth or Unique")]
    [SerializeField] private NoteType noteType;
    [SerializeField, IReadOnly] private string penitentID;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI textMeshNote;

    private void Awake()
    {
        if (textMeshNote == null)
            textMeshNote = GetComponent<TextMeshProUGUI>();
        if (textMeshNote != null)
            if (textMeshNote.text != null && noteText == "")
                noteText = textMeshNote.text;
            else
                textMeshNote.text = noteText;
        if (indexNote == 0 && (name.Split('.').Length > 1))
            {
                string[] parts = name.Split('.');
                indexNote = int.Parse(parts[2]);
            }
        if (penitentID == null || penitentID == "")
        {
            if (name.Split(".").Length > 1)
            {
                penitentID = name.Split(".")[0];
            }
        }
        if (noteType == NoteType.None)
        {
            if (name.Split(".")[1].Split("_")[0] == "M")
            {
                noteType = NoteType.Lie;
            }
            if (name.Split(".")[1].Split("_")[0] == "V")
            {
                noteType = NoteType.Truth;
            }
            if (name.Split(".")[1].Split("_")[0] == "U")
            {
                noteType = NoteType.Unique;
            }
        }
    }
}

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

    [SerializeField, IReadOnly] private int index;
    [Header("Note Data")]
    [Tooltip("Text to display on the note")]
    [SerializeField, IReadOnly] private string noteText;
    [Tooltip("Type of the note, can be Lie, Truth or Unique")]
    [SerializeField] private NoteType noteType;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI textMeshNote;

    private void Awake()
    {
        if (textMeshNote == null)
            textMeshNote = GetComponent<TextMeshProUGUI>();
        else if (textMeshNote.text != noteText)
            noteText = textMeshNote.text;
        if (index == 0 && (name.Split('.').Length > 1))
        {
            string[] parts = name.Split('.');
            index = int.Parse(parts[2]);
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

using System;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;
using UnityEngine.Events;

public class NotesBook : MonoBehaviour, IAcciones
{
    public enum UpdateType
    {
        Penitent,
        TypeDialogue,
        Note,
    }
    [SerializeField] private GameObject notesBookUI;
    [SerializeField] private NotesSave notesSave;
    private BookCanvas bookCanvas;
    private PlayerController playerC;
    private PenitentController penitentsC;
    [SerializeField, IReadOnly] private SPenitent currentPenitent;
    //private bool isCurrentPenitentUpdated;
    [SerializeField, IReadOnly] private string currentTypeDialog;
    //private bool isCurrentTypeDialogUpdated;
    [SerializeField, IReadOnly] private int currentIndexNote;
    //private bool isCurrentIndexNoteUpdated;
    [SerializeField, IReadOnly] private Notes currentPenitentNotes;

    public UnityEvent OnselectAction;
    public UnityEvent OncancelAction;
    public UnityEvent<UpdateType> OnWriteBook;
    public UnityEvent OnReadBook;

    public static NotesBook Instance { get; private set; }


    public void CancelAction()
    {
        OncancelAction.Invoke();
    }

    public void DebugAccion()
    {

    }

    public void EjecutarAccion(PlayerController playerController)
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(PlayerController playerController)
    {
        playerC = playerController;
    }
    public void Initialize(PlayerController playerController, PenitentController penitentController)
    {
        playerC = playerController;
        penitentsC = penitentController;
        if (notesBookUI != null)
        {
            bookCanvas = notesBookUI.GetComponent<BookCanvas>();
            if (bookCanvas == null)
            {
                Debug.LogWarning("BookCanvas component not found on notesBookUI GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("notesBookUI GameObject reference is not set.");
        }
        playerC.PlayerConversant.OnCurrentNodeChanged += GetNotePenitent;
        penitentsC.CurrentPenitentChanged += ChangeCurrentPenitent;
        OnWriteBook.AddListener(WriteBook);

    }

    public void SetDay(int day)
    {
        bookCanvas.EditDateText(day);
    }

    public void TriggerAction()
    {
        OnselectAction.Invoke();
    }
    public void SaveNotes(NoteData note)
    {
        notesSave.SaveNotes(note);
    }
    public void SetTypeDialogue(char type = 'U')
    {
        Debug.Log("Setting current type dialogue to: " + type);
        currentTypeDialog = type.ToString();
        OnWriteBook.Invoke(UpdateType.TypeDialogue);
    }
    private void GetNotePenitent(DialogNode node)
    {
        string indexNoteStr = node.GetIndexNote();

        // Verifica que no sea nulo ni esté vacío
        if (!string.IsNullOrEmpty(indexNoteStr))
        {
            // TryParse intenta convertirlo a número. Si puede, devuelve true y guarda el valor en parsedIndex
            if (int.TryParse(indexNoteStr, out int parsedIndex))
            {
                currentIndexNote = parsedIndex;

            }
            else
            {
                Debug.LogWarning($"[NotesBook] Formato inválido. El IndexNote '{indexNoteStr}' no es un número.");
            }
        }
        else
        {
            Debug.LogWarning("Current dialog node does not have an index note. Cannot update current index note.");
        }

        OnWriteBook.Invoke(UpdateType.Note);
    }
    private void ChangeCurrentPenitent(SPenitent newPenitent = null)
    {
        //currentPenitent = newPenitent;
        if (currentPenitent != null)
        {
            Debug.Log("Current penitent changed to: " + currentPenitent.CharacterName);
            if (currentPenitent != newPenitent)
            {
                currentPenitent = newPenitent;

                OnWriteBook.Invoke(UpdateType.Penitent);
            }
            else
            {
                Debug.Log("Current penitent is the same as the new penitent. No update needed.");

            }
        }
        else if (currentPenitent == null && newPenitent != null)
        {
            Debug.Log("Current penitent changed from null to: " + newPenitent.CharacterName);
            currentPenitent = newPenitent;

            OnWriteBook.Invoke(UpdateType.Penitent);
        }
        else
        {
            Debug.Log("Current penitent changed to null.");
            currentPenitent = null;

        }
    }


    private void WriteBook(UpdateType type)
    {
        switch (type)
        {
            case UpdateType.Penitent:
                if (currentPenitent != null)
                {
                    Debug.Log("Writing penitent note for: " + currentPenitent.CharacterName);

                    // 1. Verificamos que se le haya asignado el Prefab en el Scriptable Object
                    if (currentPenitent.NotesPenitent == null)
                    {
                        Debug.LogError($"[NotesBook] El penitente {currentPenitent.CharacterName} no tiene asignado 'NotesPenitent' en el su Scriptable Object.");
                        return; // Salimos sin romper el juego
                    }

                    // 2. Verificamos que el bookCanvas se haya inicializado
                    if (bookCanvas == null)
                    {
                        Debug.LogError("[NotesBook] bookCanvas es nulo. Asegurate de que notesBookUI tiene añadido el componente BookCanvas.");
                        return;
                    }

                    // 3. Ejecutamos la lógica que tenías de manera segura
                    GameObject noteObj = bookCanvas.WriteName(currentPenitent.NotesPenitent.gameObject);
                    Debug.Log("bookCanvas.WriteName returned GameObject: " + (noteObj != null ? noteObj.name : "null"));
                    if (noteObj != null)
                    {
                        currentPenitentNotes = noteObj.GetComponent<Notes>();
                        if (currentPenitentNotes == null)
                        {
                            Debug.LogWarning("[NotesBook] El objeto instanciado por bookCanvas.WriteName no tiene componente Notes.");
                        }
                    }
                    else
                    {
                        Debug.LogError("[NotesBook] bookCanvas.WriteName devolvió un GameObject nulo.");
                    }
                }
                else
                {
                    Debug.LogWarning("Current penitent is null. Cannot write penitent note.");
                }
                break;
            case UpdateType.TypeDialogue:
                if (!string.IsNullOrEmpty(currentTypeDialog))
                {
                    Debug.Log("Writing type dialogue note: " + currentTypeDialog);
                    //currentPenitentNotes.WriteNote()

                }
                else
                {
                    Debug.LogWarning("Current type dialogue is null or empty. Cannot write type dialogue note.");
                }
                break;
            case UpdateType.Note:
                Debug.Log("Writing note with index: " + currentIndexNote);
                if (currentTypeDialog == "U")
                {
                    Debug.Log("Writing unique note with index: " + currentIndexNote);
                    if (currentPenitentNotes != null && currentIndexNote > 0)
                    {
                        currentPenitentNotes.WriteNote(NoteType.Unique, currentIndexNote, out GameObject note);
                        NoteData data = note.GetComponent<NoteData>();
                        SaveNotes(data);
                    }
                    else
                    {
                        Debug.LogWarning("Current penitent notes is null. Cannot write unique note.");
                    }
                }
                else if (currentTypeDialog == "T")
                {
                    Debug.Log("Writing truth note with index: " + currentIndexNote);
                    if (currentPenitentNotes != null && currentIndexNote > 0)
                    {
                        currentPenitentNotes.WriteNote(NoteType.Truth, currentIndexNote, out GameObject note);
                        NoteData data = note.GetComponent<NoteData>();
                        SaveNotes(data);
                    }
                    else
                    {
                        Debug.LogWarning("Current penitent notes is null. Cannot write truth note.");
                    }
                }
                else if (currentTypeDialog == "F")
                {
                    Debug.Log("Writing lie note with index: " + currentIndexNote);
                    if (currentPenitentNotes != null && currentIndexNote > 0)
                    {
                        currentPenitentNotes.WriteNote(NoteType.Lie, currentIndexNote, out GameObject note);
                        Debug.Log("Note GameObject created: " + (note != null ? note.name : "null") + " State: " + (note != null ? note.activeSelf.ToString() : "null"));
                        NoteData data = note.GetComponent<NoteData>();
                        SaveNotes(data);
                    }
                    else
                    {
                        Debug.LogWarning("Current penitent notes is null. Cannot write lie note.");
                    }
                }
                break;
        }
    }
}

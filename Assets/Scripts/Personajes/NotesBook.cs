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
    private SPenitent currentPenitent;
    private bool isCurrentPenitentUpdated;
    private string currentTypeDialog;
    private bool isCurrentTypeDialogUpdated;
    private int currentIndexNote;
    private bool isCurrentIndexNoteUpdated;
    private Notes currentPenitentNotes;

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
                isCurrentIndexNoteUpdated = true;

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
        currentPenitent = newPenitent;
        WriteBook(UpdateType.Penitent);
    }


    private void WriteBook(UpdateType type)
    {
        switch (type)
        {
            case UpdateType.Penitent:
                if (currentPenitent != null)
                {
                    Debug.Log("Writing penitent note for: " + currentPenitent.CharacterName);
                    bookCanvas.WriteName(currentPenitent.NotesPenitent.gameObject);
                    currentPenitentNotes = currentPenitent.NotesPenitent;
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
                    currentPenitentNotes.WriteNote(NoteType.Unique, currentIndexNote);
                }
                else if (currentTypeDialog == "T")
                {
                    Debug.Log("Writing truth note with index: " + currentIndexNote);
                    currentPenitentNotes.WriteNote(NoteType.Truth, currentIndexNote);
                }
                else if (currentTypeDialog == "F")
                {
                    Debug.Log("Writing lie note with index: " + currentIndexNote);
                    currentPenitentNotes.WriteNote(NoteType.Lie, currentIndexNote);
                }
                break;
        }
    }
}

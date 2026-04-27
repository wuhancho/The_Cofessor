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
        currentIndexNote = int.Parse(node.GetIndexNote());
        OnWriteBook.Invoke(UpdateType.Penitent);
    }
    private void ChangeCurrentPenitent(SPenitent newPenitent = null)
    {
        currentPenitent = newPenitent;
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
                break;
        }
    }
}

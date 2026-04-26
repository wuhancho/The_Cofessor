using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class NotesBook : MonoBehaviour, IAcciones
{
    [SerializeField] private GameObject notesBookUI;
    [SerializeField] private NotesSave notesSave;
    private PlayerController playerC;
    private PenitentController penitentsC;
    private BookCanvas bookCanvas;


    public void CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public void DebugAccion()
    {
        throw new System.NotImplementedException();
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
    }

    public void SetDay(int day)
    {
        bookCanvas.EditDateText(day);
    }

    public void TriggerAction()
    {
        throw new System.NotImplementedException();
    }
    public void SaveNotes(NoteData note)
    {
        notesSave.SaveNotes(note);
    }
    public void GetTypeDialog(Dialog dialog)
    {
        
    }
    private void GetNotePenitent(DialogNode node)
    {
        if (node.GetIndexNote() != "")
        {
            Debug.Log("Current Dialog Node Index Note: " + node.GetIndexNote());
        }

    }
}

using System;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class A_Decision : MonoBehaviour, IAcciones
{
    [SerializeField] private int dayToActivate;
    private SPenitent[] todayPenitents;
    private int todayPenintentIndex;
    [SerializeField] private VoteCanvas voteCanvas;
    [SerializeField] private CriptaDialogue criptaDialogue;
    private PlayerController playerController;
    private PenitentController penitentController;

    private void Start()
    {
        voteCanvas.OnCulpritSelected += HandlePenitentSelected; // Suscribirse al evento de selección de culpable
    }


    public void Initialize(PlayerController playerController)
    {
        
    }
    public void Initialize(PlayerController playerController, PenitentController penitentController)
    {
        //Debug.Log("A_confecciones - Initialize invoked.");
        this.playerController = playerController;
        this.penitentController = penitentController;
    }

    public void SetDay(int day)
    {
        dayToActivate = day;
        todayPenitents = penitentController.GetSPenitents(day);
        todayPenintentIndex = 0;
    }
    public void EjecutarAccion(PlayerController playerController)
    {
        
    }
    public void TriggerAction()
    {
        voteCanvas.gameObject.SetActive(true);
        voteCanvas.Initialize(penitentController, playerController,dayToActivate);

    }
    public void CancelAction()
    {
       
    }

    public void DebugAccion()
    {
        
    }
    private void HandlePenitentSelected(SPenitent penitent)
    {
        Dialog selectedDialog = GetDialogue(penitent);

    }

    private Dialog GetDialogue(SPenitent penitent)
    {
        if (penitent != null)
        {
            foreach (Dialog dialog in penitent.Dialogs)
            {
                if (dialog == null) continue;
                if (dayToActivate != penitent.DayDialogue) continue;
                if (penitent.TypeDialogue == "C")
                {
                    playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
                    Debug.Log($"Diálogo único encontrado: {dialog.name} para el penitente {penitent.CharacterName} en el día {dayToActivate}");
                    return dialog;
                }
            }
        }
        return null;
    }

}

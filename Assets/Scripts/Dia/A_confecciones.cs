using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class A_confecciones : MonoBehaviour, IAcciones
{
    [SerializeField] private PenitentController penitentController;
    [SerializeField] private int day;
    [SerializeField] private PlayerController playerController;
    

    private void Start()
    {
        EjecutarAccion(playerController);
    }
    public void CancelAction()
    {

    }
    private void GetChoices(DialogNode dialogueNode)
    {
        //Debug.Log($"A_confecciones - GetChoices invoked. de node is: {dialogueNode}");
        playerController.ChangeStatesPlayer(dialogueNode);
    }
    public void EjecutarAccion(PlayerController playerController)
    {
        if (playerController.PlayerStatus.RepPueblo >= 8)
        {
            Dialog dialog = TrueDialogueUpdate();
            if (dialog == null)
            {
                Debug.LogWarning($"[A_confecciones] No se encontró diálogo verdadero para el día {day}.");
                return;
            }
            Debug.Log($"a playerController le doy el diálogo {dialog.name}");
            playerController.PlayerConversant.GetTestDialogue(dialog);
        }
        else
        {
            Dialog dialog = FalseDialogueUpdate();
            if (dialog == null)
            {
                Debug.LogWarning($"[A_confecciones] No se encontró diálogo falso para el día {day}.");
                return;
            }
            
            playerController.PlayerConversant.GetTestDialogue(dialog);
        }

        //playerController.PlayerConversant.StartDialogue(dialog);
    }

    public void TriggerAction()
    {
       
    }


    public void SetDay(int day)
    {
        this.day = day;
    }

    

    private Dialog TrueDialogueUpdate()
    {
        foreach (SPenitent sPenitent in penitentController.GetAllPenitents())
        {
            if (sPenitent == null) continue;
            //if (sPenitent.Day == day) continue;

            foreach (Dialog dialo in sPenitent.Dialogs)
            {
                if (dialo == null) continue;
                if (dialo.IsTrueDialogue)
                {
                    playerController.PlayerConversant.CurrentSpeakerNPC = sPenitent.CharacterName;
                    return dialo;
                }
            }
            break;
        }
        return null;
    }
    private Dialog FalseDialogueUpdate()
    {
        foreach (SPenitent sPenitent in penitentController.GetAllPenitents())
        {
            //Debug.Log("Revisando penitente...");
            if (sPenitent == null) continue;
            //Debug.Log($"El penitente encontrado es {sPenitent.CharacterName}");
            foreach (Dialog dialo in sPenitent.Dialogs)
            {
                //Debug.Log("Revisando diálogo falso...");
                if (dialo == null) continue;
                //Debug.Log($"El dialogo encotrado es {dialo.name}");
                if (!dialo.IsTrueDialogue)
                {
                    playerController.PlayerConversant.CurrentSpeakerNPC = sPenitent.CharacterName;
                    return dialo;
                }
            }
            break;
        }
        return null;
    }

    public void InitializePlayer(PlayerController playerController)
    {
        this.playerController = playerController;
    }
}

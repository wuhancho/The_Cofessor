using The_cofessor.Personajes.Dialogs;
using TMPro;
using UnityEngine;

public class A_confecciones : MonoBehaviour, IAcciones
{
    [SerializeField] private SPenitent[] sPenitents;
    [SerializeField] private int day;
    [SerializeField] private PlayerController playerController;
    private void Awake()
    {
        InitializePlayer(GetComponentInParent<PlayerController>());
        EjecutarAccion(playerController);
    }
    public void CancelAction()
    {

    }

    public void EjecutarAccion(PlayerController playerController)
    {
        if (playerController == null)
        {
            Debug.LogError("[A_confecciones] PlayerController es null.");
            return;
        }
        if (playerController.PlayerStatus == null)
        {
            Debug.LogError("[A_confecciones] PlayerStatus no asignado en PlayerController.");
            return;
        }
        if (playerController.PlayerConversant == null)
        {
            Debug.LogError("[A_confecciones] PlayerConversant no asignado en PlayerController.");
            return;
        }

        // Seleccionar diálogo según reputación
        Dialog dialog = playerController.PlayerStatus.RepPueblo >= 8
            ? TrueDialogueUpdate()
            : FalseDialogueUpdate();

        if (dialog == null)
        {
            Debug.LogWarning($"[A_confecciones] No se encontró diálogo (day={day}, repPueblo={playerController.PlayerStatus.RepPueblo}).");
            return;
        }

        playerController.PlayerConversant.StartDialogue(dialog);
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
        foreach (SPenitent sPenitent in sPenitents)
        {
            if (sPenitent == null) continue;
            if (sPenitent.Day != day) continue;

            foreach (Dialog dialo in sPenitent.Dialogs)
            {
                if (dialo == null) continue;
                if (dialo.IsTrueDialogue)
                    return dialo;
            }
            break;
        }
        return null;
    }
    private Dialog FalseDialogueUpdate()
    {
        foreach (SPenitent sPenitent in sPenitents)
        {
            if (sPenitent == null) continue;
            if (sPenitent.Day != day) continue;

            foreach (Dialog dialo in sPenitent.Dialogs)
            {
                if (dialo == null) continue;
                if (!dialo.IsTrueDialogue)
                    return dialo;
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

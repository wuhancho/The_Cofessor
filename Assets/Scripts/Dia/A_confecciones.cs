using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using Unity.VisualScripting;
using UnityEngine;

public class A_confecciones : MonoBehaviour, IAcciones
{
    [SerializeField] private int day;
    private PenitentController penitentController;
    private PlayerController playerController;


    private SPenitent[] todayPenitents;
    private int todayPenintentIndex = 0;
    [SerializeField] private Texture2D[] penitentImages;
    [SerializeField] GameObject entrancePenitent;
    private Action vara;

    private void Start()
    {
        EjecutarAccion(playerController);
        entrancePenitent.GetComponent<EntrancePenitent>().DisplayDuration = playerController.PlayerConversant.TestDelay;
        
    }
    public void Initialize(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void Initialize(PlayerController playerController, PenitentController penitentController)
    {
        //Debug.Log("A_confecciones - Initialize invoked.");
        this.playerController = playerController;
        this.penitentController = penitentController;
    }
    public void CancelAction()
    {

    }

    public void GetChoices(DialogNode dialogueNode)
    {
        //Debug.Log($"A_confecciones - GetChoices invoked. de node is: {dialogueNode}");
        playerController.ChangeStatesPlayer(dialogueNode);
    }
    private Dialog TrueDialogueUpdate()
    {
        //foreach (SPenitent sPenitent in penitentController.GetAllPenitents())
        //{
        //    if (sPenitent == null) continue;
        //    if (sPenitent.Day != day) continue;
        //    if (sPenitent != currentPenitent) continue;
        //    foreach (Dialog dialo in sPenitent.Dialogs)
        //    {
        //        if (dialo == null) continue;
        //        if (dialo.IsTrueDialogue)
        //        {
        //            playerController.PlayerConversant.CurrentSpeakerNPC = sPenitent.CharacterName;
        //            return dialo;
        //        }
        //    }
        //    break;
        //}
        return GetDialogue(true);
    }

    private Dialog FalseDialogueUpdate()
    {
        //foreach (SPenitent sPenitent in penitentController.GetAllPenitents())
        //{
        //    //Debug.Log("Revisando penitente...");
        //    if (sPenitent == null) continue;
        //    if (sPenitent.Day != day) continue;
        //    //Debug.Log($"El penitente encontrado es {sPenitent.CharacterName}");
        //    foreach (Dialog dialo in sPenitent.Dialogs)
        //    {
        //        //Debug.Log("Revisando diálogo falso...");
        //        if (dialo == null) continue;
        //        //Debug.Log($"El dialogo encotrado es {dialo.name}");
        //        if (!dialo.IsTrueDialogue)
        //        {
        //            playerController.PlayerConversant.CurrentSpeakerNPC = sPenitent.CharacterName;
        //            return dialo;
        //        }
        //    }
        //    break;
        //}
        //return null;
        return GetDialogue(false);
    }

    private Dialog GetDialogue(bool isTrueDialogue)
    {
        SPenitent penitent = todayPenitents[todayPenintentIndex];
        if (penitent != null)
        {
            foreach (Dialog dialo in penitent.Dialogs)
            {
                if (dialo == null) continue;
                if (dialo.IsTrueDialogue == isTrueDialogue)
                {
                    //UpdatePenitentImage(penitent);
                    ShowEntrancePenitent(true);
                    //playerController.PlayerConversant.isTheLastNode.AddListener(HideEntrancePenitent);
                    playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
                    return dialo;
                }
            }
        }

        return null;
    }
    
    private void UpdatePenitentImage(SPenitent penitent)
    {
        penitentImages = penitent.GetTextures2D();
        if (penitentImages != null && penitentImages.Length > 0)
        {
            playerController.PlayerConversant.SetIconNPC(penitentImages[0]);
        }
    }

    public void EjecutarAccion(PlayerController playerController)
    {
        //entrancePenitent.GetComponent<EntrancePenitent>().StopAnimation();
        if (playerController.PlayerStatus.RepPueblo >= 8)
        {
            //entrancePenitent.GetComponent<EntrancePenitent>().PlayEntranceAnimation();
            Dialog dialog = TrueDialogueUpdate();
            if (dialog == null)
            {
                Debug.LogWarning($"[A_confecciones] No se encontró diálogo verdadero para el día {day}.");
                return;
            }
            Debug.Log($"a playerController le doy el diálogo {dialog.name}");
            UpdatePenitentImage(todayPenitents[todayPenintentIndex]);
            playerController.PlayerConversant.GetTestDialogue(dialog);
        }
        else
        {
            //entrancePenitent.GetComponent<EntrancePenitent>().PlayEntranceAnimation();
            Dialog dialog = FalseDialogueUpdate();
            if (dialog == null)
            {
                Debug.LogWarning($"[A_confecciones] No se encontró diálogo falso para el día {day}.");
                return;
            }
            UpdatePenitentImage(todayPenitents[todayPenintentIndex]);
            playerController.PlayerConversant.GetTestDialogue(dialog);
        }

        //playerController.PlayerConversant.StartDialogue(dialog);
    }

    public void TriggerAction()
    {
        //Debug.Log("TriggerAction en A_confecciones");
        EjecutarAccion(playerController);
    }

    public IEnumerator ShowEntrancePenitent(bool isChange)
    {

        if (entrancePenitent != null)
        {
            if(isChange == true)
            {
                yield return new WaitForSeconds(entrancePenitent.GetComponent<EntrancePenitent>().DisplayDuration);
                entrancePenitent.GetComponent<EntrancePenitent>().PlayEntranceAnimation(isChange);
            }
            
        }
    }
    public IEnumerator HideEntrancePenitent(bool isChange)
    {
        if (entrancePenitent != null)
        {
            if (isChange == false)
            {
                yield return new WaitForSeconds(entrancePenitent.GetComponent<EntrancePenitent>().DisplayDuration);
                entrancePenitent.GetComponent<EntrancePenitent>().PlayExitAnimation(isChange);
            }
            //entrancePenitent.GetComponent<EntrancePenitent>().StopAnimation();
        }
    }

    public void SetDay(int day)
    {
        this.day = day;
        Debug.Log($"A_confecciones - SetDay invoked. Day set to: {day}");
        todayPenitents = penitentController.GetSPenitents(day);
        todayPenintentIndex = 0;
    }

    public void IsConfession(bool isLast)
    {
        //Debug.Log($"IsConfession invoked with isLast = {isLast}");
        if (isLast == true)
        {
            //playerController.PlayerConversant.isTheLastNode.AddListener(HideEntrancePenitent);
            //entrancePenitent.GetComponent<EntrancePenitent>().PlayExitAnimation(false);
            todayPenintentIndex++;
            if (todayPenintentIndex < todayPenitents.Length)
            {
                //entrancePenitent.GetComponent<EntrancePenitent>().PlayEntranceAnimation();
                TriggerAction();
            }
            else
            {
                //pasar a lo siguiente;
            }
        }
    }




}

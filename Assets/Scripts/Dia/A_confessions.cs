using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using Unity.VisualScripting;
using UnityEngine;

public class A_confessions : MonoBehaviour, IAcciones
{
    [SerializeField] private int day;
    [SerializeField] private PenitentController penitentController;
    [SerializeField] private PlayerController playerController;


    private SPenitent[] todayPenitents;
    private int todayPenintentIndex = 0;
    [SerializeField] private Texture2D[] penitentImages;
    [SerializeField] EntrancePenitent entrancePenitent;
    //private Action vara;

    //private void Start()
    //{
    //    EjecutarAccion(playerController);
    //    entrancePenitent.DisplayDuration = playerController.PlayerConversant.TestDelay;

    //}

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
            foreach (Dialog dialog in penitent.Dialogs)
            {
                if (dialog == null) continue;
                if (day != penitent.DayDialogue) continue;
                if (penitent.TypeDialogue == "U")
                {
                    playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
                    Debug.Log($"Diálogo único encontrado: {dialog.name} para el penitente {penitent.CharacterName} en el día {day}");
                    return dialog;
                }
                if (penitent.TypeDialogue == "M" || penitent.TypeDialogue == "V") continue;
                if (dialog.IsTrueDialogue == isTrueDialogue)
                {
                    //UpdatePenitentImage(penitent);
                    //ShowEntrancePenitent(true);
                    playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
                    return dialog;
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

    public void StartConfession()
    {
        //Debug.Log($"start confession with playerController{playerController.name} and penitentController{penitentController.name}");
        EjecutarAccion(playerController);
    }

    public void EjecutarAccion(PlayerController playerController)
    {
        Dialog dialog;
        //entrancePenitent.GetComponent<EntrancePenitent>().StopAnimation();
        if (playerController.PlayerStatus.RepPueblo >= 8)
        {
            //entrancePenitent.GetComponent<EntrancePenitent>().PlayEntranceAnimation();
            dialog = TrueDialogueUpdate();
            if (dialog == null)
            {
                Debug.LogWarning($"[A_confecciones] No se encontró diálogo verdadero para el día {day}.");
                return;
            }
            Debug.Log($"a playerController le doy el diálogo {dialog.name}");
        }
        else
        {
            //entrancePenitent.GetComponent<EntrancePenitent>().PlayEntranceAnimation();
            dialog = FalseDialogueUpdate();
            if (dialog == null)
            {
                Debug.LogWarning($"[A_confecciones] No se encontró diálogo falso para el día {day}.");
                return;
            }
        }
        playerController.PlayerConversant.GetTestDialogue(dialog);

        //playerController.PlayerConversant.StartDialogue(dialog);
    }

    public void TriggerAction()
    {
        //Debug.Log("TriggerAction en A_confecciones");
        EjecutarAccion(playerController);
    }

    //public void ShowEntrancePenitent(bool isChange)
    //{
    //    if (entrancePenitent != null)
    //    {
    //        if (isChange == true)
    //        {
    //            //yield return new WaitForSeconds(entrancePenitent.DisplayDuration);
    //            entrancePenitent.PlayEntranceAnimation(isChange);
    //        }

    //    }
    //}

    //public void PlayExitAnim(bool isChange)
    //{
    //    if (isChange == true)
    //        StartCoroutine(ShowExitPenitent(!isChange));
    //}
    //private IEnumerator ShowExitPenitent(bool isChange)
    //{
    //    if (entrancePenitent != null)
    //    {
    //        if (isChange == false)
    //        {
    //            yield return new WaitForSeconds(entrancePenitent.DisplayDuration);
    //            entrancePenitent.PlayExitAnimation(isChange);
    //        }
    //        //entrancePenitent.GetComponent<EntrancePenitent>().StopAnimation();
    //    }
    //}

    public void SetDay(int day)
    {
        this.day = day;
        //Debug.Log($"A_confecciones - SetDay invoked. Day set to: {day}");
        todayPenitents = penitentController.GetSPenitents(day);
        Debug.Log($"A_confecciones - Found {todayPenitents.Length} penitents for day {day}.");
        todayPenintentIndex = 0;
    }

    public bool ToNextPenitent()
    {
        //Debug.Log($"IsConfession invoked with isLast = {isLast}");

        //playerController.PlayerConversant.isTheLastNode.AddListener(HideEntrancePenitent);
        //entrancePenitent.GetComponent<EntrancePenitent>().PlayExitAnimation(false);
        todayPenintentIndex++;
        return todayPenintentIndex < todayPenitents.Length;
        //if ()
        //{
        ////entrancePenitent.GetComponent<EntrancePenitent>().PlayEntranceAnimation();
        ////TriggerAction();
        //return true;
        //}
        //else
        //{
        //    //pasar a lo siguiente;
        //}
    }

    public void UpdatePenitent()
    {
        UpdatePenitentImage(todayPenitents[todayPenintentIndex]);
    }

    public Sprite GetCurrentPeninentSprite()
    {
        SPenitent penitent = todayPenitents[todayPenintentIndex];
        if (penitent != null)
        {
            Texture2D[] textures = penitent.GetTextures2D();
            if (textures != null && textures.Length > 0)
            {
                Texture2D texture = textures[0];
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
        return null;
    }
    public void DebugAccion() 
    {
        Debug.Log($"{playerController.PlayerStatus.Day} - Acción de confesion - Día: {day}");
    }
}

using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class A_confessions : MonoBehaviour, IAcciones
{
    [SerializeField] private int day;
    [SerializeField] private PenitentController penitentController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Texture2D[] penitentImages;
    [SerializeField] EntrancePenitent entrancePenitent;
    [SerializeField] private NotesBook notesBook;


    private SPenitent[] todayPenitents;
    private int todayPenintentIndex = 0;

    private bool halfPenitentTriggered = false;
    private int halfCount = 0;

    public UnityEvent onHalfPenitent;
    public UnityEvent SecondPartConfessions;
    public UnityEvent<Dialog> onDialogue;
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
                //Debug.Log($"Revisando diálogo {dialog.name} para el penitente {penitent.CharacterName} en el día {day}");
                if (dialog == null) continue;
                //Debug.Log($"Diálogo {dialog.name} encontrado para el penitente {penitent.CharacterName} en el día {day}");
                if (day != penitent.DayDialogue(dialog)) continue;
                //Debug.Log($"Diálogo {dialog.name} corresponde al día {day} para el penitente {penitent.CharacterName}");
                penitentController.SetCurrentPenitentOnConffession(penitent);
                if (penitent.DayDialogue(dialog) == day)
                {
                    if (penitent.DayDialogue(dialog) == 0)
                    {
                        if (penitent.TypeDialogue == "U")
                        {
                            playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
                            //Debug.Log($"Diálogo único encontrado: {dialog.name} para el penitente {penitent.CharacterName} en el día {day}");
                            notesBook.SetTypeDialogue();
                            //Debug.Log($"a notas le doy el tipo Unique");
                            return dialog;
                        }
                    }
                    else
                    {
                        //Debug.Log($"Diálogo encontrado: {dialog.name} para el penitente {penitent.CharacterName} en el día {day}");
                        if (dialog.IsTrueDialogue == isTrueDialogue)
                        {
                            //Debug.Log($"Diálogo {(isTrueDialogue ? "verdadero" : "falso")} encontrado: {dialog.name} para el penitente {penitent.CharacterName} en el día {day}");
                            if (isTrueDialogue)
                            {
                                //Debug.Log($"Diálogo verdadero seleccionado: {dialog.name} para el penitente {penitent.CharacterName} en el día {day}");
                                playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
                                notesBook.SetTypeDialogue('T');
                                //Debug.Log($"a notas le doy el tipo true");
                                return dialog;
                            }
                            else if (!isTrueDialogue)
                            {
                                //Debug.Log($"Diálogo falso seleccionado: {dialog.name} para el penitente {penitent.CharacterName} en el día {day}");
                                playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
                                notesBook.SetTypeDialogue('F');
                                //Debug.Log($"a notas le doy el tipo false");
                                return dialog;
                            }
                        }
                    }
                }
            }

            return null;
        }
        else
        {
            //Debug.LogWarning($"[A_confecciones] No se encontró un penitente válido para el día {day} en el índice {todayPenintentIndex}.");
            return null;
        }
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
            onDialogue?.Invoke(dialog);
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
            onDialogue?.Invoke(dialog);
            if (dialog == null)
            {
                Debug.LogWarning($"[A_confecciones] No se encontró diálogo falso para el día {day}.");
                return;
            }
        }
        playerController.PlayerConversant.StartDialogue(dialog);

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
        foreach (SPenitent penitent in todayPenitents)
        {
            if (penitent != null)
            {
                Debug.Log($"A_confecciones - Penitent found for day {day}: {penitent.CharacterName}");
            }
            else
            {
                Debug.LogWarning($"A_confecciones - Null penitent found for day {day}.");
            }
        }
        Debug.Log($"A_confecciones - Retrieved {todayPenitents.Length} penitents for day {day}");
        todayPenintentIndex = 0;
        halfPenitentTriggered = false;
        halfCount = Mathf.FloorToInt(todayPenitents.Length / 2);
        Debug.Log($"A_confecciones - Found {todayPenitents.Length} penitents for day {day}.");
        todayPenintentIndex = 0;
    }

    public bool ToNextPenitent()
    {
        //Debug.Log($"IsConfession invoked with isLast = {isLast}");

        //playerController.PlayerConversant.isTheLastNode.AddListener(HideEntrancePenitent);
        //entrancePenitent.GetComponent<EntrancePenitent>().PlayExitAnimation(false);
        todayPenintentIndex++;
        //mitad de los penitentes alcanzada
        if (!halfPenitentTriggered && todayPenintentIndex >= halfCount)
        {
            halfPenitentTriggered = true;
            Debug.Log("Mitad de penitentes alcanzada. Disparando onHalfPenitent.");

            FadeController.Instance.FadeIn(1.5f, () =>
            {
                Debug.Log("Fade out completo. Invocando onHalfPenitent.");
                FadeController.Instance.FadeOut(1.5f);
                onHalfPenitent?.Invoke();
            });
            //onHalfPenitent?.Invoke();
            return true; // hay más por hacer tras la pausa
        }
        bool more = todayPenintentIndex < todayPenitents.Length;
        if (!more)
        {
            Debug.Log("Todas las confesiones finalizadas.");
        }
        return more;
        //return todayPenintentIndex < todayPenitents.Length;
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

    // Llamado por el objeto externo al terminar su evento intermedio
    public void ContinueSecondPart()
    {
        Debug.Log("Reanudando segunda parte de confesiones.");
        //FadeController.Instance.FadeOut(1.5f, () =>
        //{
        Debug.Log("Fade in completo. Invocando SecondPartConfessions.");
        SecondPartConfessions?.Invoke();
        TriggerAction(); // lanzar siguiente diálogo inmediatamente
                         //FadeController.Instance.FadeIn(1.5f);
                         //});
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

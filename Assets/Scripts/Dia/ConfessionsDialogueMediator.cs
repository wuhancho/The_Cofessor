using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class ConfessionsDialogueMediator : MonoBehaviour
{
    [SerializeField] private A_confessions confessions;
    [SerializeField] private PlayerConversant playerConversant;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private EntrancePenitent entrancePenitent;

    private void Start()
    {
        dialogueUI.OnDialogueEnd += OnDialogueEnd;
        playerConversant.isTheLastNode += (bool onAccion) => OnDialogueFinalNode(onAccion);
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        dialogueUI.SetDialogueAllBoxVisible(false);
        Sprite sprite = confessions.GetCurrentPeninentSprite();
        dialogueUI.SetupSpeakerSprite(sprite);
        entrancePenitent.PlayEntranceAnimation();
        yield return new WaitForSeconds(entrancePenitent.EntranceDuration);
        dialogueUI.SetDialogueAllBoxVisible(true);
        confessions.StartConfession();
    }

    private void OnDialogueEnd()
    {
        dialogueUI.SetDialogueAllBoxVisible(false);
        bool morePenitents = confessions.ToNextPenitent();
        if (morePenitents)
        {
            StartCoroutine(ChangePenitent());
        }
        else
        {
            //acaba la tarde
            print("Ha acabado la tarde de confesiones.");
        }
    }
    private void OnDialogueFinalNode(bool onDialogue)
    {
        if (onDialogue)
        {
            dialogueUI.SetDialogueSpeakerBoxVisible(false);
            Debug.Log("Llega al nodo final de la conversación.");
        }
        else
        {
            Debug.Log("No es el nodo final de la conversación.");
        }
    }

    private IEnumerator ChangePenitent()
    {
        //yield return StartCoroutine(ExitPenitent());
        yield return ExitPenitent();
        StartCoroutine(StartDialogue());
    }

    private IEnumerator ExitPenitent()
    {
        entrancePenitent.PlayExitAnimation();
        yield return new WaitForSeconds(entrancePenitent.ExitDuration);
    }
}

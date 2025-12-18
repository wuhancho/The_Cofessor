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
        dialogueUI.onDialogueEnd += OnDialogueEnd;
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        dialogueUI.SetDialogueBoxVisible(false);
        Sprite sprite = confessions.GetCurrentPeninentSprite();
        dialogueUI.SetupSpeakerSprite(sprite);
        entrancePenitent.PlayEntranceAnimation();
        yield return new WaitForSeconds(entrancePenitent.EntranceDuration);
        dialogueUI.SetDialogueBoxVisible(true);
        confessions.StartConfession();
    }

    private void OnDialogueEnd()
    {
        dialogueUI.SetDialogueBoxVisible(false);
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

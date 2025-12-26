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
        dialogueUI.NextButton.gameObject.SetActive(false);
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
        bool morePenitents = confessions.ToNextPenitent();
        if (morePenitents)
        {
            entrancePenitent.IsChangeEntrance = false;
            dialogueUI.SetDialogueAIBoxVisible(true);
            dialogueUI.SetDialogueSpeakerBoxVisible(false);
            dialogueUI.BuildTextAI("El siguiente penitente va a entrar...");
            dialogueUI.NextButton.gameObject.SetActive(true);
            dialogueUI.NextButton.onClick.RemoveAllListeners();
            dialogueUI.NextButton.onClick.AddListener(OnNextPenitentButtonClicked);
            // Elimina el listener anterior si existe para evitar múltiples suscripciones
            //dialogueUI.NextButton.onClick.RemoveListener(OnNextPenitentButtonClicked);
            //dialogueUI.NextButton.onClick.AddListener(OnNextPenitentButtonClicked);

        }
        else
        {
            //acaba la tarde
            print("Ha acabado la tarde de confesiones.");
        }
    }

    // Nuevo método void para usar como listener
    private void OnNextPenitentButtonClicked()
    {
        dialogueUI.SetDialogueAllBoxVisible(false);
        entrancePenitent.IsChangeEntrance = true;
        ChangePenitent();
        dialogueUI.NextButton.gameObject.SetActive(false);
    }

    private void OnDialogueFinalNode(bool onDialogue)
    {
        if (onDialogue)
        {
            dialogueUI.SetDialogueSpeakerBoxVisible(false);
            StartCoroutine(ExitPenitent());
            Debug.Log("Llega al nodo final de la conversación.");
        }
        else
        {
            Debug.Log("No es el nodo final de la conversación.");
        }
    }

    private void ChangePenitent()
    {
        //yield return StartCoroutine(ExitPenitent());
        //yield return ExitPenitent();
        StartCoroutine(StartDialogue());
    }

    private IEnumerator ExitPenitent()
    {
        entrancePenitent.PlayExitAnimation();
        yield return new WaitForSeconds(entrancePenitent.ExitDuration);
    }
}

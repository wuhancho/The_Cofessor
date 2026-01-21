using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;
using UnityEngine.Events;

public class ConfessionsDialogueMediator : MonoBehaviour
{
    [SerializeField] private A_confessions confessions;
    [SerializeField] private PlayerConversant playerConversant;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private EntrancePenitent entrancePenitent;
    private bool isPausedAtHalf = false;

    public UnityEvent OnAllConfessionsEnded;

    private void Start()
    {
        dialogueUI.NextButton.gameObject.SetActive(false);
        dialogueUI.OnDialogueEnd += OnDialogueEnd;
        playerConversant.isTheLastNode += (bool onAccion) => OnDialogueFinalNode(onAccion);

        // Suscribirse a eventos de A_confessions
        confessions.onHalfPenitent.AddListener(OnHalfPenitentReached);
        confessions.SecondPartConfessions.AddListener(OnSecondPartStart);

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
            if (isPausedAtHalf)
            {
                return;
            }

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
            FadeController.Instance.FadeOut(2f);
            OnAllConfessionsEnded?.Invoke();
            print("Ha acabado la tarde de confesiones.");
        }
    }


    private void OnHalfPenitentReached()
    {
        isPausedAtHalf = true;
        // Oculta cajas y muestra mensaje de intermedio
        dialogueUI.SetDialogueAllBoxVisible(false);
        dialogueUI.SetDialogueAIBoxVisible(true);
        dialogueUI.BuildTextAI("Vale ... no hay nadie mas para las confeciones");
        dialogueUI.NextButton.gameObject.SetActive(false);

    }

    private void OnSecondPartStart()
    {
        isPausedAtHalf = false;
        // Reanudar flujo normal
        StartCoroutine(StartDialogue());
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

using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using TMPro;
using UnityEngine;

public class CombatDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textObject;
    [SerializeField] private float timeReading = 3f;
    private PlayerController playerController;
    private PlayerConversant playerConversant;
    private SPenitent penitent;
    private TextMeshProUGUI penitentText;
    private string currentNodeText;
    private string[] currentLines;
    private int currentLineIndex;
    

    public Action onDialogueUpdated;
    internal Action onDialogueFinished;

    private void Start()
    {
        onDialogueUpdated += UpdateUI;
    }
    public void Initialize(PlayerController controller, SPenitent penitent)
    {
        playerController = controller;
        playerConversant = controller.PlayerConversant;
        this.penitent = penitent;
    }
    void UpdateUI()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
        {
            return;
        }



        if (currentNodeText != playerConversant.GetText())
        {
            currentNodeText = playerConversant.GetText();
            currentLines = currentNodeText.Split("/n");
            currentLineIndex = 0;
        }
        BuildTextAI();
        StartCoroutine(TimeReading());
    }



    private void BuildTextAI()
    {
        if (currentLines == null || currentLines.Length == 0) return;

        penitentText.text = currentLines[currentLineIndex];
    }

    public void Next()
    {
        if (currentLines != null && currentLineIndex < currentLines.Length - 1)
        {
            currentLineIndex++;
            BuildTextAI();
        }
        else if (playerConversant.HasNext())
        {
            playerConversant.Next();
        }
        else
        {
            Debug.Log("CriptaDialogue - No more lines or choices. Ending conversation.");
        }

    }

    internal void StartDialogue(CombatPhase phase)
    {
        Debug.Log($"Starting dialogue for combat phase: {phase}");

    }

    private IEnumerator TimeReading()
    {
        yield return new WaitForSeconds(timeReading); // Espera 3 segundos (ajusta según el tiempo que quieras)
        Next(); // Avanza al siguiente diálogo después de la espera
    }
}

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
    private CanvasCombat canvas;


    public Action onDialogueUpdated;
    internal Action onDialogueFinished;
    private string typeDialogue;

    private void Start()
    {
        onDialogueUpdated += UpdateUI;
    }
    public void Initialize(PlayerController controller, SPenitent penitent, CanvasCombat canvas)
    {
        this.canvas = canvas;
        playerController = controller;
        playerConversant = controller.PlayerConversant;
        this.penitent = penitent;

        // Obtener el componente TextMeshProUGUI del textObject
        if (textObject != null)
        {
            penitentText = textObject.GetComponent<TextMeshProUGUI>();
        }
        if (penitentText == null)
        {
            Debug.LogError("[CombatDialogue] penitentText es null. Asegúrate de que textObject tiene un componente TextMeshProUGUI.");
        }
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
        if (penitentText == null) return;

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
            Debug.Log("CombatDialogue - No more lines or choices. Ending conversation.");
            // Desuscribir para no recibir más eventos
            playerConversant.OnConversationUpdated -= UpdateUI;
            onDialogueFinished?.Invoke();
        }

    }

    internal void StartDialogue(CombatPhase phase)
    {
        Debug.Log($"Starting dialogue for combat phase: {phase}");
        switch (phase)
        {
            case CombatPhase.Phase1:
                InitializeDecision(canvas.Decision.TypeDialogue);
                break;
            case CombatPhase.Phase2:
                InitializeDecision("CombatPhase2");
                break;
            case CombatPhase.Phase3:
                InitializeDecision("CombatPhase3");
                break;
            default:
                Debug.LogWarning($"CombatDialogue - Unhandled combat phase: {phase}");
                break;
        }

    }

    private IEnumerator TimeReading()
    {
        yield return new WaitForSeconds(timeReading);
        Next();
    }
    public void InitializeDecision(string TypeDialogue)
    {
        // Resetear estado del diálogo anterior
        currentNodeText = null;
        currentLines = null;
        currentLineIndex = 0;

        playerConversant.OnConversationUpdated += UpdateUI;

        Dialog dialogue = penitent.GetDialogByType(TypeDialogue);

        if (dialogue == null)
        {
            Debug.LogWarning($"CombatDialogue - No se encontró diálogo de tipo '{TypeDialogue}' " +
                             $"para penitente '{penitent.CharacterName}'. " +
                             $"Tipos disponibles en sus diálogos:");

            foreach (Dialog d in penitent.Dialogs)
            {
                if (d == null) continue;
                string t = SPenitent.GetDialogType(d);
                Debug.LogWarning($"  → '{d.name}' → tipo parseado: '{t}'");
            }
            // Si no hay diálogo, terminar inmediatamente para no bloquear el flujo
            onDialogueFinished?.Invoke();
            return;
        }

        Debug.Log($"CombatDialogue - Penitent: {penitent.CharacterName}, " +
                  $"Type: {TypeDialogue}, Dialog: {dialogue.name}");
        playerConversant.StartDialogue(dialogue);
    }

    internal void GiveType(string v)
    {
        typeDialogue = v;
    }
}

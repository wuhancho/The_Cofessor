using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CriptaDialogue : MonoBehaviour
{
    [SerializeField] GameObject penitentImage;
    [SerializeField] GameObject choicesPrefab;
    [SerializeField] TextMeshProUGUI penitentNameText;
    [SerializeField] TextMeshProUGUI penitentText;
    [SerializeField] Transform choicesRoot;
    [SerializeField] Button nextButton;
    private PlayerConversant playerConversant;
    [SerializeField] private float timeReading;
    private string currentNodeText;
    private string[] currentLines;
    private int currentLineIndex;
    private string typeDialogue;
    private bool isDecisionDialogue;

    //private Texture2D[] penitentImages;

    public event Action<bool> IsPunish;
    public event Action onDialogueDecisionEnd;
    public void Initialize(PlayerConversant pConversant)
    {
        playerConversant = pConversant;
        StartDialogueStandart();
    }
    public void InitializeDecision(PlayerConversant pConversant, string TypeDialogue)
    {
        this.typeDialogue = TypeDialogue;
        playerConversant = pConversant;
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerConversant.OnConversationUpdated += UpdateUIDecision;
        isDecisionDialogue = true;
        UpdateUIDecision();
    }

    void StartDialogueStandart()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerConversant.OnConversationUpdated += UpdateUIStandart;
        nextButton.onClick.AddListener(Next);
        isDecisionDialogue = false;
        UpdateUIStandart();
    }
    void UpdateUIStandart()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
        {
            return;
        }
        penitentNameText.text = playerConversant.GetCurrentSpeakerName();
        choicesRoot.gameObject.SetActive(playerConversant.IsChoosing());
        nextButton.gameObject.SetActive(!playerConversant.IsChoosing());
        if (playerConversant.IsChoosing())
        {
            BuildChoiseList();
        }
        else
        {
            if (currentNodeText != playerConversant.GetText())
            {
                currentNodeText = playerConversant.GetText();
                currentLines = currentNodeText.Split("/n");
                currentLineIndex = 0;
            }
            BuildTextAI();
        }

    }
    void UpdateUIDecision()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
        {
            return;
        }
        penitentNameText.text = playerConversant.GetCurrentSpeakerName();
        //choicesRoot.gameObject.SetActive(playerConversant.IsChoosing());
        //StartCoroutine(TimeReading());
        if (playerConversant.IsChoosing())
        {
            BuildChoiseList();
        }
        else
        {
            if (currentNodeText != playerConversant.GetText())
            {
                currentNodeText = playerConversant.GetText();
                currentLines = currentNodeText.Split("/n");
                currentLineIndex = 0;

            }
            BuildTextAI();
            StartCoroutine(TimeReading());
        }

    }
    public void SetupSpeakerSprite(Sprite sprite)
    {
        penitentImage.GetComponent<Image>().sprite = sprite;
    }


    private void BuildChoiseList()
    {
        choicesRoot.DetachChildren();
        foreach (DialogNode choice in playerConversant.GetChoices())
        {
            GameObject choiceInstance = Instantiate(choicesPrefab, choicesRoot);
            choiceInstance.GetComponentInChildren<TextMeshProUGUI>().text = choice.GetText();
            Button button = choiceInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
                playerConversant.SelectChoice(choice);
                Debug.Log($"CriptaDialogue - Choice selected: {choice.GetText()}");
                if (choice.GetText() == "Castigar")
                {
                    Debug.Log("CriptaDialogue - Castigar choice selected. Calling ChoiceCastigar method.");
                    ChoiceCastigar();

                }
                else if (choice.GetText() == "Perdonar")
                {
                    Debug.Log("CriptaDialogue - Perdonar choice selected. Calling ChoicePerdonar method.");
                    ChoicePerdonar();

                }
                else if (choice.GetText() == "Perdonar (+50$)")
                {
                    Debug.Log("CriptaDialogue - Perdonar (+50$) choice selected. Implement forgiveness with bribe logic here.");
                    Dia.Instance.SetSobornos(choice.GetSobornoCost());
                    ChoicePerdonar();
                    //DelateChoicesRootChilds();
                }
                DelateChoicesRootChilds();
                //onConversation.Invoke(choice);
            });
        }
    }

    private void DelateChoicesRootChilds()
    {
        foreach (Transform child in choicesRoot)
        {
            Destroy(child.gameObject);
        }
    }

    private void ChoicePerdonar()
    {
        Debug.Log("CriptaDialogue - Perdonar choice selected. Implement forgiveness logic here.");
        IsPunish.Invoke(false);
    }

    private void ChoiceCastigar()
    {
        Debug.Log("CriptaDialogue - Castigar choice selected. Implement punishment logic here.");
        IsPunish.Invoke(true);
    }

    private void BuildTextAI()
    {
        if (currentLines == null || currentLines.Length == 0) return;

        penitentText.text = currentLines[currentLineIndex];

    }
    public void BuildTextAI(string text)
    {
        penitentText.text = text;
    }
    public void Next()
    {
        if (currentLines != null && currentLineIndex < currentLines.Length - 1)
        {
            Debug.Log("CriptaDialogue - Advancing to next line of dialogue.");
            currentLineIndex++;
            BuildTextAI();
            if (isDecisionDialogue)
            {
                StartCoroutine(TimeReading());
            }
        }
        else if (playerConversant.HasNext())
        {
            Debug.Log("CriptaDialogue - No more lines. Advancing to next dialogue node.");
            playerConversant.Next();
        }
        else
        {
            nextButton.gameObject.SetActive(false);
            switch (typeDialogue)
            {
                case "F-Culpable":
                    Debug.Log("CriptaDialogue - Ending conversation. Invoking onDialogueDecisionEnd event.");
                    onDialogueDecisionEnd?.Invoke();
                    break;
                case "P-Culpable":
                    Debug.Log("CriptaDialogue - Ending conversation. Invoking onDialogueDecisionEnd event.");
                    onDialogueDecisionEnd?.Invoke();
                    break;
                default:
                    Debug.Log("CriptaDialogue - Ending conversation. No specific type dialogue actions defined.");
                    onDialogueDecisionEnd?.Invoke();
                    break;
            }

        }
    }
    private IEnumerator TimeReading()
    {
        yield return new WaitForSeconds(timeReading);
        Debug.Log("CriptaDialogue - Time reading finished. Calling Next method.");
        Next();
    }

    /// <summary>
    /// Unsubscribes event handlers from the <c>OnConversationUpdated</c> event of the player conversant.
    /// </summary>
    /// <remarks>Use this method to detach UI update handlers when they are no longer needed, such as during
    /// cleanup or when disabling conversation UI components.</remarks>
    /// <param name="standart"><see langword="true"/> to unsubscribe the standard UI update handler; otherwise, <see langword="false"/>.</param>
    /// <param name="decision"><see langword="true"/> to unsubscribe the decision UI update handler; otherwise, <see langword="false"/>.</param>
    public void DeSubcripcionEvent(bool standart = true, bool decision = true)
    {
        if (standart)
            playerConversant.OnConversationUpdated -= UpdateUIStandart;
        if (decision)
            playerConversant.OnConversationUpdated -= UpdateUIDecision;
    }
}

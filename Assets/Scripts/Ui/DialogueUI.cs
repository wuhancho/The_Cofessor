using UnityEngine;
using The_cofessor.Personajes.Dialogs;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Events;
using System;

public class DialogueUI : MonoBehaviour
{
    PlayerConversant playerConversant;
    [SerializeField] Image speakerImage;
    [SerializeField] TextMeshProUGUI AIText;
    [SerializeField] Button nextButton;
    [SerializeField] Button nextButton2;
    [SerializeField] GameObject CurrentSpeaker;
    [SerializeField] GameObject AIResponces;
    [SerializeField] Transform choicesRoot;
    [SerializeField] GameObject choicesPrefab;
    [SerializeField] Button quitButton;
    [SerializeField] UnityEvent onConversation;

    private string[] currentLines;
    private int currentLineIndex;
    private string currentNodeText;

    public Button NextButton { get => nextButton2; set => nextButton2 = value; }

    //private bool isBuildingText = false;

    public event Action OnDialogueEnd;

    void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerConversant.OnConversationUpdated += UpdateUI;
        nextButton.onClick.AddListener(Next);

        UpdateUI();
    }

    void Next()
    {
        //playerConversant.Next();
        if (currentLines != null && currentLineIndex < currentLines.Length - 1)
        {
            currentLineIndex++;
            //BuildImageAI();
            BuildTextAI();
        }
        else if(playerConversant.HasNext())
        {
            playerConversant.Next();
        }
        else
        {
            OnDialogueEnd?.Invoke();
        }

    }

    // Update is called once per frame
    void UpdateUI()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
        {
            return;
        }
        CurrentSpeaker.GetComponent<TextMeshProUGUI>().text = playerConversant.GetCurrentSpeakerName();
        //AIResponces.SetActive(!playerConversant.IsChoosing());
        nextButton.gameObject.SetActive(!playerConversant.IsChoosing());
        choicesRoot.gameObject.SetActive(playerConversant.IsChoosing());
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

            //BuildImageAI();
            BuildTextAI();
        }

    }

    private void BuildImageAI()
    {
        speakerImage.sprite = Sprite.Create(playerConversant.IconNPC, new Rect(0, 0, playerConversant.IconNPC.width, playerConversant.IconNPC.height), new Vector2(0.5f, 0.5f));
        //Debug.Log($"DialogueUI - Building speaker image. the name at the image is {speakerImage.sprite.name}");
    }

    public void SetupSpeakerSprite(Sprite sprite)
    {
        speakerImage.sprite = sprite;
    }

    private void BuildTextAI()
    {
        //foreach (string line in playerConversant.GetText().Split("/n"))
        //{
        //    Debug.Log(line);
        //}
        //AIText.text = playerConversant.GetText();

        //nextButton.gameObject.SetActive(playerConversant.HasNext());
        if (currentLines == null || currentLines.Length == 0) return;
        //isBuildingText = true;
        AIText.text = currentLines[currentLineIndex];
        

        //bool hasMoreLines = currentLineIndex < currentLines.Length - 1;
        //if(!playerConversant.HasNext() && hasMoreLines)
        //{
        //    playerConversant.isTheLastNode.Invoke(true);
        //}
        //nextButton.gameObject.SetActive(hasMoreLines || playerConversant.HasNext());
    }
    public void BuildTextAI(string text)
    {
        AIText.text = text;
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
                CurrentSpeaker.GetComponent<TextMeshProUGUI>().text = playerConversant.GetCurrentSpeakerName();
                playerConversant.SelectChoice(choice);
                //onConversation.Invoke(choice);
            });
        }
    }

    public void SetDialogueAllBoxVisible(bool isVisible)
    {
        CurrentSpeaker.SetActive(isVisible);
        AIResponces.SetActive(isVisible);
        //Debug.Log($"DialogueUI - {gameObject.GetComponent<Image>().name}");
        gameObject.GetComponent<Image>().enabled = isVisible;
    }
    public void SetDialogueSpeakerBoxVisible(bool isVisible)
    {
        CurrentSpeaker.SetActive(isVisible);
    }
    public void SetDialogueAIBoxVisible(bool isVisible)
    {
        nextButton.gameObject.SetActive(false);
        gameObject.GetComponent<Image>().enabled = isVisible;
        AIResponces.SetActive(isVisible);
    }

}

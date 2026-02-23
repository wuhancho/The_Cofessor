using System;
using The_cofessor.Personajes.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CriptaDialogue : MonoBehaviour
{
    [SerializeField] Image penitentImage;
    [SerializeField] GameObject choicesPrefab;
    [SerializeField] TextMeshProUGUI penitentNameText;
    [SerializeField] TextMeshProUGUI penitentText;
    [SerializeField] Transform choicesRoot;
    [SerializeField] Button nextButton;
    private PlayerConversant playerConversant;
    private string currentNodeText;
    private string[] currentLines;
    private int currentLineIndex;
    private Texture2D[] penitentImages;

    public void Initialize(PlayerConversant pConversant)
    {
        playerConversant = pConversant;
    }

    void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerConversant.OnConversationUpdated += UpdateUI;
        nextButton.onClick.AddListener(Next);
        UpdateUI();
    }
    private void UpdatePenitentImage(SPenitent penitent)
    {
        penitentImages = penitent.GetTextures2D();
        if (penitentImages != null && penitentImages.Length > 0)
        {
            playerConversant.SetIconNPC(penitentImages[0]);
        }
    }
    void UpdateUI()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
        {
            return;
        }
        penitentNameText.text = playerConversant.GetCurrentSpeakerName();
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
            BuildTextAI();
        }

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
                //CurrentSpeaker.GetComponent<TextMeshProUGUI>().text = playerConversant.GetCurrentSpeakerName();
                playerConversant.SelectChoice(choice);
                //onConversation.Invoke(choice);
            });
        }
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
        penitentText.text = currentLines[currentLineIndex];


        //bool hasMoreLines = currentLineIndex < currentLines.Length - 1;
        //if(!playerConversant.HasNext() && hasMoreLines)
        //{
        //    playerConversant.isTheLastNode.Invoke(true);
        //}
        //nextButton.gameObject.SetActive(hasMoreLines || playerConversant.HasNext());
    }
    public void BuildTextAI(string text)
    {
        penitentText.text = text;
    }
    public void Next()
    {
        //playerConversant.Next();
        if (currentLines != null && currentLineIndex < currentLines.Length - 1)
        {
            currentLineIndex++;
            //BuildImageAI();
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

    public void SetPenitentSprite(Sprite sprite)
    {
        if (sprite != null)
            penitentImage.sprite = sprite;
    }
}

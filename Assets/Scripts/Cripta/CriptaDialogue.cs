using System;
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
                //CurrentSpeaker.GetComponent<TextMeshProUGUI>().text = playerConversant.GetCurrentSpeakerName();
                playerConversant.SelectChoice(choice);
                //onConversation.Invoke(choice);
            });
        }
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

    //private void SetPenitentSprite(Sprite sprite)
    //{
       
    //    if (sprite != null)
    //        penitentImage.sprite = sprite;
    //    else
    //        Debug.LogWarning("CriptaDialogue - Attempted to set penitent sprite, but the provided sprite is null.");
    //}
}

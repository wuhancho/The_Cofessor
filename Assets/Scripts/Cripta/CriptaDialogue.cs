using The_cofessor.Personajes.Dialogs;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CriptaDialogue : MonoBehaviour
{
    [SerializeField] GameObject penitentImage;
    [SerializeField] TextMeshProUGUI penitentNameText;
    [SerializeField] TextMeshProUGUI penitentText;
    [SerializeField] Transform choicesRoot;
    [SerializeField] GameObject choicesPrefab;
    private PlayerConversant playerConversant;
    private string currentNodeText;
    private string[] currentLines;
    private int currentLineIndex;

    public void Initialize(PlayerConversant pConversant)
    {
        playerConversant = pConversant;
    }

    void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerConversant.OnConversationUpdated += UpdateUI;


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
        //AIResponces.SetActive(!playerConversant.IsChoosing());
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
}

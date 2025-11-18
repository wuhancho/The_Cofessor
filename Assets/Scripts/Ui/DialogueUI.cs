using UnityEngine;
using The_cofessor.Personajes.Dialogs;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogueUI : MonoBehaviour
{
    PlayerConversant playerConversant;
    [SerializeField] TextMeshProUGUI AIText;
    [SerializeField] Button nextButton;
    [SerializeField] GameObject AIResponces;
    [SerializeField] Transform choicesRoot;
    [SerializeField] GameObject choicesPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        nextButton.onClick.AddListener(Next);

        UpdateUI();
    }

    void Next()
    {
        playerConversant.Next();
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        if (!playerConversant.IsActive())
        {
            return;
        }
        AIResponces.SetActive(!playerConversant.IsChoosing());
        choicesRoot.gameObject.SetActive(playerConversant.IsChoosing());
        if (playerConversant.IsChoosing())
        {
            BuildChoiseList();
        }
        else
        {
            BuildTextAI();
        }

    }

    private void BuildTextAI()
    {
        foreach(string line in playerConversant.GetText().Split("."))
        {
            Debug.Log(line);
        }
        AIText.text = playerConversant.GetText();
        nextButton.gameObject.SetActive(playerConversant.HasNext());
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
                UpdateUI();
            });
        }
    }
}

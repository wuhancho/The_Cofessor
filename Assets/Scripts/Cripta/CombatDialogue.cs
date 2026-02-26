using The_cofessor.Personajes.Dialogs;
using TMPro;
using UnityEngine;

public class CombatDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textObject;
    private PlayerController playerController;
    private PlayerConversant playerConversant;
    private SPenitent penitent;
    private TextMeshProUGUI penitentText;
    private string currentNodeText;
    private string[] currentLines;
    private int currentLineIndex;

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
}

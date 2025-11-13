using UnityEngine;
using The_cofessor.Personajes.Dialogs;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    PlayerConversant playerConversant;
    [SerializeField] TextMeshProUGUI AIText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        AIText.text = playerConversant.GetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}

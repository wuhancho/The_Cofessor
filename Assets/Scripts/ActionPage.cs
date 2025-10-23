using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPage : MonoBehaviour
{
    [SerializeField] private Button actionButton;
    [SerializeField] private Button closeButton;
    //[SerializeField] private 
    [SerializeField] private TextMeshProUGUI actionNameText;
    
    private void Awake()
    {
        actionButton.onClick.AddListener(ActionP);
        closeButton.onClick.AddListener(ClosePage);
    }

    private void ClosePage()
    {
        Debug.Log("ActionPage closed.");

    }

    private void ActionP()
    {
        Debug.Log("ActionPage action executed.");
        
        actionNameText.text = ("que quiere");
    }

}

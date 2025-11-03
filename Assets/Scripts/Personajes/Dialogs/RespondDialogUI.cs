using UnityEngine;

public class RespondDialogUI : MonoBehaviour
{
    [SerializeField] private RespondDialog respondDialog;
    private void Start()
    {
        
        foreach (string response in respondDialog.GetResponses())
        {
            Debug.Log($"Has respond:{response}");
        }
    }
}

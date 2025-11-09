using UnityEngine;

public class Confessors : MonoBehaviour, IConfessions
{
    [SerializeField] SPenitent penitent;
    public void GetDialogs()
    {
        if (penitent != null) {
            Debug.Log($"Dialogs: {penitent.Dialogs.Length}");
        } else {
            Debug.Log("No penitent assigned.");
        }
    }

    public void SetDialogs(string[] dialogs)
    {
        
    }
    public SPenitent GetPenitent()
    {
        return penitent;
    }
    public void SetPenitent(SPenitent newPenitent)
    {
        penitent = newPenitent;
    }

}

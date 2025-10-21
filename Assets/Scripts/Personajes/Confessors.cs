using UnityEngine;

public class Confessors : MonoBehaviour, IConfessions
{
    [SerializeField] SPenitent penitent;
    public void GetDialogs()
    {
        
    }

    public void SetDialogs(string[] dialogs)
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {

    }
}

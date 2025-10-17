using UnityEngine;
public interface IExpecialDialogs
{
    //protected string[] expecialDialogs { get; set; }
    public void GetDialogsRamdom(string[] expecialDialog) 
    {
        int ramdomIndex = Random.Range(0, expecialDialog.Length);
        Debug.Log(expecialDialog[ramdomIndex]);
    }
}
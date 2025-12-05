using The_cofessor.Personajes.Dialogs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "", menuName = "Scriptable Objects/SPenitent", order = 1)]
public class SPenitent: ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string characterName;
    [SerializeField] private Texture2D[] characterImage;
    [SerializeField] private Dialog[] dialogs;
    [SerializeField] bool isTrueDialogue = false;
    [SerializeField] private int day;

    public string CharacterName { get => characterName; set => characterName = value; }

    public Dialog[] Dialogs { get => dialogs; set => dialogs = value; }
    public bool IsTrueDialogue { get => isTrueDialogue; set => isTrueDialogue = value; }
    public int Day { get => day; set => day = value; }
    public string Id { get => id; set => id = value; }
    
    public Texture2D[] GetTextures2D()
    {
        return characterImage;
    }

    public Dialog GetAllTrueDialogues()
    {
        if (isTrueDialogue)
        {
            foreach (var dialog in dialogs)
            {
                if(dialog.IsTrueDialogue)
                {
                    return dialog;
                }
            }
        }
        return null;
    }
    public Dialog GetAllFalseDialogues()
    {
        if (!isTrueDialogue)
        {
            foreach (var dialog in dialogs)
            {
                if (!dialog.IsTrueDialogue)
                {
                    return dialog;
                }
            }
        }
        return null;
    }
}

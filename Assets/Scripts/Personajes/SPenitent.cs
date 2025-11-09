using The_cofessor.Personajes.Dialogs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "", menuName = "Scriptable Objects/SPenitent", order = 1)]
public class SPenitent: ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private Texture2D characterImage;
    [SerializeField] private Dialog[] dialogs;
    [SerializeField] private Dialog dialogEvent;

    public string CharacterName { get => characterName; set => characterName = value; }
    public Texture2D CharacterImage { get => characterImage; set => characterImage = value; }
    public Dialog[] Dialogs { get => dialogs; set => dialogs = value; }
    public Dialog DialogEvent { get => dialogEvent; set => dialogEvent = value; }
}

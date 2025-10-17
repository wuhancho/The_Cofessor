using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "", menuName = "ScriptableObjects/SConfessors", order = 1)]
public class SPenitent: ScriptableObject
{
    string characterName;
    Image characterImage;
    INormalDialogs[] dialogs;
    IExpecialDialogs[] exDialogs;
}

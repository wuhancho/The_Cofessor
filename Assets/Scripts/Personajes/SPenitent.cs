using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "", menuName = "ScriptableObjects/SPenitent", order = 1)]
public class SPenitent: ScriptableObject
{
    [SerializeField] public string characterName;
    [SerializeField] public Texture2D characterImage;
    [SerializeField] public Dialog[] dialogs;
    [SerializeField] public Dialog dialogEvent;
}

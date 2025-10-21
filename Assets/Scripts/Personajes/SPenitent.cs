using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "", menuName = "ScriptableObjects/SPenitent", order = 1)]
public class SPenitent: ScriptableObject
{
    [SerializeField] string characterName;
    [SerializeField] Image characterImage;
    [SerializeField] Dialog[] dialogs;
    [SerializeField] Dialog dialogEvent;
    
}

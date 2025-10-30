using UnityEngine;
[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
public class Dialog : ScriptableObject, IDialogs
{
    [SerializeField] protected Frases[] normalDialogs;
    [SerializeField] protected string[] dialogsMenu;
    [SerializeField] protected int grade;
    int IDialogs.grade { get => grade; set => grade = value; }
    Frases[] IDialogs.Dialogs { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}

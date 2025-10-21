using UnityEngine;

public class Dialog : MonoBehaviour, IDialogs
{
    [SerializeField] protected string[] normalDialogs;
    [SerializeField] protected int grade;
    int IDialogs.grade { get => grade; set => grade = value; }
    string[] IDialogs.Dialogs { get => normalDialogs; set => normalDialogs = value; }
}

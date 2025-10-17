using UnityEngine;

public class D_Player: MonoBehaviour, INormalDialogs,IExpecialDialogs
{
    [SerializeField] string[] normalDialogs;
    [SerializeField] string[] expecialDialogs;
    string[] INormalDialogs.normalDialogs { get; set; }
    string[] IExpecialDialogs.expecialDialogs { get; set; }
}

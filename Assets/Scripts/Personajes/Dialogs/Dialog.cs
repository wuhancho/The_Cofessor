using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace The_cofessor.Pesonajes.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
    public class Dialog : ScriptableObject
    {
        [SerializeField] DialogNode[] nodes;
    }

}






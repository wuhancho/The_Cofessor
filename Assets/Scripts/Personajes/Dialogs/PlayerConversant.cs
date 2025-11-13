using System;
using UnityEngine;

namespace The_cofessor.Personajes.Dialogs
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialog currentDialog;

        internal string GetText()
        {
            if (currentDialog == null) return "";
            DialogNode rootNode = currentDialog.GetRootNode();
            return rootNode.GetText();
        }

    }
} 


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace The_cofessor.Personajes.Dialogs
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialog testDialog;
        Dialog currentDialog;
        DialogNode currentNode = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.002f);
            StartDialogue(testDialog);
        }

        public void StartDialogue(Dialog newDialogue)
        {
            currentDialog = newDialogue;
            currentNode = currentDialog.GetRootNode();
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialog != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        internal string GetText()
        {
            if (currentDialog == null) return "";
            return currentNode.GetText();
        }

        public IEnumerable<DialogNode> GetChoices()
        {
            return currentDialog.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialog.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }

            DialogNode[] children = currentDialog.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            currentNode = children[randomIndex];
            onConversationUpdated();
        }
        public bool HasNext()
        {
           
            return currentDialog.GetAllChildren(currentNode).Count() > 0;
        }

    }
} 


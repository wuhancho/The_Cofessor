using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace The_cofessor.Pesonajes.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Penitent", menuName = "Scriptable Objects/Dialogue/Dialogue penitent", order = 1)]
    public class Dialog : ScriptableObject
    {
        [SerializeField] List<DialogNode> nodes = new List<DialogNode>();
        Dictionary<string, DialogNode> nodeLookup = new Dictionary<string, DialogNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                DialogNode rootNode = new DialogNode();
                rootNode.uniqueID = Guid.NewGuid().ToString();
                nodes.Add(rootNode);
            }
            Debug.Log($"dialog {name} awake with {nodes.Count} nodes.");
        }
#endif
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogNode node in GetAllNodes())
            {
                nodeLookup[node.uniqueID] = node;
            }
        }
        public IEnumerable<DialogNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogNode> GetAllChildren(DialogNode parentNode)
        {
            foreach (string childID in parentNode.childrenIDs)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

        public void CreateNode(DialogNode parent)
        {
            DialogNode newNode = new DialogNode();
            newNode.uniqueID = Guid.NewGuid().ToString();
            parent.childrenIDs.Add(newNode.uniqueID);
            nodes.Add(newNode);
            OnValidate();
        }
        public void DeleteNode(DialogNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            CleanDanglingChildren(nodeToDelete);
            OnValidate();
        }

        private void CleanDanglingChildren(DialogNode nodeToDelete)
        {
            foreach (DialogNode node in GetAllNodes())
            {
                if (node.childrenIDs.Contains(nodeToDelete.uniqueID))
                {
                    node.childrenIDs.Remove(nodeToDelete.uniqueID);
                }
            }
        }
    }

}






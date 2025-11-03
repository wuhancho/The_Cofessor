using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace The_cofessor.Pesonajes.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Penitent", menuName = "Scriptable Objects/Dialogue/Dialogue penitent", order = 1)]
    public class Dialog : ScriptableObject
    {
        [SerializeField] List<DialogNode> nodes;
        Dictionary<string, DialogNode> nodeLookup = new Dictionary<string, DialogNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogNode());
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
            //foreach (string childID in parentNode.childrenIDs)
            //{
            //    DialogNode childNode = nodes.Find(n => n.uniqueID == childID);
            //    if (childNode != null)
            //    {
            //        yield return childNode;
            //    }
            //}
            foreach (string childID in parentNode.childrenIDs)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }
    }

}






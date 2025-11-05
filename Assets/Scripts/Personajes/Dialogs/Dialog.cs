using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


namespace The_cofessor.Personajes.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Penitent", menuName = "Scriptable Objects/Dialogue/Dialogue penitent", order = 1)]
    public class Dialog : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogNode> nodes = new List<DialogNode>();
        Dictionary<string, DialogNode> nodeLookup = new Dictionary<string, DialogNode>();

#if UNITY_EDITOR
        private void Awake()
        {
       
            Debug.Log($"dialog {name} awake with {nodes.Count} nodes.");
        }
#endif
        private void OnValidate()
        {

            nodeLookup.Clear();
            foreach (DialogNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
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
            DialogNode newNode = CreateInstance<DialogNode>();
            //newNode.SetID(Guid.NewGuid().ToString());
            newNode.name = Guid.NewGuid().ToString(); //newNode.uniqueID = Guid.NewGuid().ToString();
            Undo.RegisterCreatedObjectUndo(newNode, "Create Dialog Node");
            if (parent != null)
            {
                parent.childrenIDs.Add(newNode.name);
            }
            nodes.Add(newNode);
            //AssetDatabase.AddObjectToAsset(newNode, this);
            OnValidate();
        }
        public void DeleteNode(DialogNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogNode nodeToDelete)
        {
            foreach (DialogNode node in GetAllNodes())
            {
                node.childrenIDs.Remove(nodeToDelete.name);
            }
        }

        public void OnBeforeSerialize()
        {
            if (nodes.Count == 0)
            {
                CreateNode(null);
            }
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node)== "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
        }

        public void OnAfterDeserialize()
        {

        }
    }

}






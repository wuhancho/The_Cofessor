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
        [SerializeField] List<DialogNode> nodes = new();
        private readonly Dictionary<string, DialogNode> nodeLookup = new();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);

        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogNode node in GetAllNodes())
            {
                if (!nodeLookup.ContainsKey(node.name))
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
            foreach (string childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }
#if UNITY_EDITOR
        public void CreateNode(DialogNode parent)
        {
            DialogNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Create Dialog Node");
            Undo.RecordObject(this, "Create Dialog Node");
            AddNode(newNode);
        }

        

        public void DeleteNode(DialogNode nodeToDelete)
        {
            Undo.RecordObject(this, "Delete Dialog Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }
        private DialogNode MakeNode(DialogNode parent)
        {
            DialogNode newNode = CreateInstance<DialogNode>();
            //newNode.SetID(Guid.NewGuid().ToString());
            newNode.name = Guid.NewGuid().ToString(); //newNode.uniqueID = Guid.NewGuid().ToString();
            if (parent != null)
            {
                Undo.RecordObject(parent, "Add Child To Dialog Node");
                parent.AddChild(newNode.name);
                newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
                newNode.SetPosition(parent.GetRect().position + newNodeOffset);
                
            }

            return newNode;
        }

        private void AddNode(DialogNode newNode)
        {
            nodes.Add(newNode);
            //AssetDatabase.AddObjectToAsset(newNode, this);
            OnValidate();
        }
        private void CleanDanglingChildren(DialogNode nodeToDelete)
        {
            foreach (DialogNode node in GetAllNodes())
            {
                Undo.RecordObject(node, "Clean Dangling Children");
                node.RemoveChild(nodeToDelete.name);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogNode newNode = MakeNode(null);
                AddNode(newNode);
            }
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node)== "")
                    {
                        if(node != null)
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {

        }
    }

}






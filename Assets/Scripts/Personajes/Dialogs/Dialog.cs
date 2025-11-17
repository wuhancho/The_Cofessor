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
        [SerializeField] Vector2 newNodeOffset = new(250, 0);

        private void OnValidate()
        {
            //Debug.Log($"OnValidate Dialog {name}");
            //nodeLookup.Clear();
            //foreach (DialogNode node in GetAllNodes())
            //{
            //    //Debug.Log($"--loop");
            //    //Debug.Log($"--dictionary: {nodeLookup}");
            //    //Debug.Log($"--list: {nodes.Count}");
            //    //Debug.Log($"--nodo: {node}");
            //    //Debug.Log($"--nodo name: {node.name}");
            //    if (!nodeLookup.ContainsKey(node.name))
            //    {
            //        nodeLookup[node.name] = node;
            //    }
            //}

#if UNITY_EDITOR
            if (nodes == null || nodes.Count == 0)
                return;

            // Evitar validaciones durante importación
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (EditorApplication.isUpdating || EditorApplication.isCompiling)
                return;

            nodeLookup.Clear();

            foreach (DialogNode node in nodes)
            {
                if (node == null) continue;
                if (string.IsNullOrEmpty(node.GetID())) continue;

                if (!nodeLookup.ContainsKey(node.GetID()))
                    nodeLookup[node.GetID()] = node;
            }
#endif
        }
        private void OnEnable()
        {
#if UNITY_EDITOR
            if (nodes == null) return;

            // Si cargamos el asset y no tiene nodos → creamos el primero de forma segura
            if (nodes.Count == 0 && AssetDatabase.Contains(this))
            {
                Undo.RecordObject(this, "Create Root Dialog Node");

                var newNode = MakeNode(null);
                nodes.Add(newNode);

                AssetDatabase.AddObjectToAsset(newNode, this);
                AssetDatabase.SaveAssets();
            }

#endif
            RebuildLookUp();
        }
        private void RebuildLookUp()
        {
            nodeLookup.Clear();
            foreach (DialogNode node in GetAllNodes())
            {
                if (node != null && !string.IsNullOrEmpty(node.GetID()))
                {
                    nodeLookup[node.GetID()] = node;
                }
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
        public IEnumerable<DialogNode> GetAIChildren(DialogNode currentNode)
        {
            foreach (DialogNode childNode in GetAllChildren(currentNode))
            {
                if (!childNode.IsPlayerSpeaking())
                {
                    yield return childNode;
                }
            }
        }

        public IEnumerable<DialogNode> GetPlayerChildren(DialogNode currentNode)
        {
            foreach (DialogNode childNode in GetAllChildren(currentNode))
            {
                if (childNode.IsPlayerSpeaking())
                {
                    yield return childNode;
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
            newNode.SetID((nodes.Count + 1).ToString());
            //newNode.name = Guid.NewGuid().ToString(); //newNode.uniqueID = Guid.NewGuid().ToString();
            if (parent != null)
            {
                Undo.RecordObject(parent, "Add Child To Dialog Node");
                parent.AddChild(newNode.GetID());
                newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
                newNode.SetPosition(parent.GetRect().position + newNodeOffset);

            }

            return newNode;
        }

        private void AddNode(DialogNode newNode)
        {
            nodes.Add(newNode);
#if UNITY_EDITOR
            // si ya somos un asset real, agregamos el nodo ahora mismo
            if (AssetDatabase.Contains(this))
            {
                AssetDatabase.AddObjectToAsset(newNode, this);
                EditorUtility.SetDirty(this);
            }
#endif
            //AssetDatabase.AddObjectToAsset(newNode, this);
            OnValidate();
        }
        private void CleanDanglingChildren(DialogNode nodeToDelete)
        {
            foreach (DialogNode node in GetAllNodes())
            {
                Undo.RecordObject(node, "Clean Dangling Children");
                node.RemoveChild(nodeToDelete.GetID());
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            //if (nodes.Count == 0)
            //{
            //    DialogNode newNode = MakeNode(null);
            //    AddNode(newNode);
            //}
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        if (node != null)
                            AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            RebuildLookUp();
        }

     
    }

}
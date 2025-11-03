using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace The_cofessor.Pesonajes.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Penitent", menuName = "Scriptable Objects/Dialogue/Dialogue penitent", order = 1)]
    public class Dialog : ScriptableObject
    {
        [SerializeField] List<DialogNode> nodes;

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
        public IEnumerable<DialogNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogNode GetRootNode()
        {
            return nodes[0];
        }
    }

}






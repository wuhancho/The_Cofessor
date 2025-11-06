using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace The_cofessor.Personajes.Dialogs
{
    //[CreateAssetMenu(fileName = "New DialogNode", menuName = "Scriptable Objects/Dialogue/DialogNode", order = 1)]
    public class DialogNode : ScriptableObject
    {
        //public string uniqueID;
        [SerializeField] private string text;
        [SerializeField] private List<string> childrenIDs = new();
        [SerializeField] private Rect rect = new(0, 0, 300, 200);
        public Rect GetRect() 
        { 
            return rect; 
        }
        public string GetText()
        {
            return text;
        }
        public List<string> GetChildren()
        {
            return childrenIDs;
        }
#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Drag Dialog Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }
        public void SetText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Update Dialog Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }
        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Child To Dialog Node");
            childrenIDs.Add(childID);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Child From Dialog Node");
            childrenIDs.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
        //        private void OnValidate()
        //        {
        //            if (uniqueID != name)
        //            {
        //                uniqueID = name;
        //#if unity_editor
        //                unity_editor.EditorUtility.SetDirty(this);
        //#endif
        //            }
        //        }
        //        public void SetID(string id)
        //        {
        //            if (string.IsNullOrEmpty(id)) return;

        //            if (uniqueID != id || name != id)
        //            {
        //                uniqueID = id;
        //                name = id;
        //#if UNITY_EDITOR
        //                UnityEditor.EditorUtility.SetDirty(this);
        //#endif
        //            }
        //        }
        //    }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace The_cofessor.Personajes.Dialogs
{
    //[CreateAssetMenu(fileName = "New DialogNode", menuName = "Scriptable Objects/Dialogue/DialogNode", order = 1)]
    public class DialogNode : ScriptableObject
    {
        //public string uniqueID;
        public string text;
        public List<string> childrenIDs = new List<string>();
        public Rect rect = new Rect(0, 0, 300, 200);
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

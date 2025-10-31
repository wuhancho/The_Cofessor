using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace The_cofessor.Personajes.Dialogs.Editor
{
    public class DialogEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogEditor), false, "Dialogue Editor");

        }
    }
}

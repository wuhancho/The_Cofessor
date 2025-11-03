using PlasticGui.WorkspaceWindow;
using System;
using System.Collections;
using System.Collections.Generic;
using The_cofessor.Pesonajes.Dialogs;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
namespace The_cofessor.Personajes.Dialogs.Editor
{
    public class DialogEditor : EditorWindow
    {
        Dialog selectedDialog = null;
        GUIStyle nodeStyle;
        DialogNode draggingNode = null;
        Vector2 draggingOffsets;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogEditor), false, "Dialogue Editor");

        }

        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID,int line)
        {
            Dialog dialog = EditorUtility.InstanceIDToObject(instanceID) as Dialog;

            if (dialog != null)
            {
                ShowEditorWindow();
                Debug.Log("Abriendo DialogEditor");
                return true;
            }
            Debug.Log("no se abre abrir DialogEditor");
            return false;
        }
        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background =EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;

            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

        }

        private void OnSelectionChange()
        {
            Dialog newDialog = Selection.activeObject as Dialog;
            if (newDialog != null)
            {
                selectedDialog = newDialog;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialog == null)
            {

                EditorGUILayout.LabelField("No Dialogue Selected", EditorStyles.boldLabel);
                return;
            }
            else
            {
                ProcessEvents();
                foreach ( DialogNode node in selectedDialog.GetAllNodes())
                {
                    OnGUINode(node);
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode !=null)
                {
                    draggingOffsets = draggingNode.rect.position - Event.current.mousePosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialog, "Drag Dialog Node");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffsets;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
        }

      

        private void OnGUINode(DialogNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Dialog Node", EditorStyles.whiteBoldLabel);
            string nodeID = EditorGUILayout.TextField("Node ID", node.uniqueID);
            string newText = EditorGUILayout.TextField("dialog", node.text);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialog, "Update Dialog Text");
                node.uniqueID = nodeID;
                node.text = newText;
            }
            foreach (DialogNode childnode in selectedDialog.GetAllChildren(node))
            {
                EditorGUILayout.LabelField(childnode.text);
            }

            GUILayout.EndArea();
        }
        private DialogNode GetNodeAtPoint(Vector2 mousePoint)
        {
            DialogNode foundNode = null;
            foreach (DialogNode node in selectedDialog.GetAllNodes())
            {
                if (node.rect.Contains(mousePoint))
                {
                    foundNode = node;
                    
                }
            }
            return foundNode;
        }
    }
}

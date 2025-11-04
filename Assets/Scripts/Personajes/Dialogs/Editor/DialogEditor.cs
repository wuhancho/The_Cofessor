using Codice.CM.Common.Mount;
using PlasticGui.WorkspaceWindow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
namespace The_cofessor.Personajes.Dialogs.Editor
{
    public class DialogEditor : EditorWindow
    {
        Dialog selectedDialog = null;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] DialogNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffsets;
        [NonSerialized] DialogNode creatingNode = null;
        [NonSerialized] DialogNode deletingNode = null;
        [NonSerialized] DialogNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized] bool isDraggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;
        const float canvasSize = 4000f;
        const float backgroundSize = 50f;


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
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize,canvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach (DialogNode node in selectedDialog.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogNode node in selectedDialog.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    Undo.RecordObject(selectedDialog, "Create Dialog Node");
                    selectedDialog.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    Undo.RecordObject(selectedDialog, "Delete Dialog Node");
                    selectedDialog.DeleteNode(deletingNode);
                    deletingNode = null;
                }

            }
        }

        //private void ScrollView()
        //{
        //    List<DialogNode> allNodes = new List<DialogNode>(selectedDialog.GetAllNodes());
        //    float height = allNodes[allNodes.Count - 1].rect.yMax + 200;
        //    float width = allNodes[allNodes.Capacity - 1].rect.xMax + 200;
        //    Rect scrollViewRect = GUILayoutUtility.GetRect(height,width);
            
        //}


        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode !=null)
                {
                    draggingOffsets = draggingNode.rect.position - Event.current.mousePosition;
                }
                else
                {
                    isDraggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialog, "Drag Dialog Node");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffsets;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && isDraggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && isDraggingCanvas)
            {
                isDraggingCanvas = false;
            }
        }

      

        private void DrawNode(DialogNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("ID: " + node.uniqueID, EditorStyles.whiteBoldLabel);
            EditorGUILayout.LabelField("Dialogue:");
            string newText = EditorGUILayout.TextField(node.text);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialog, "Update Dialog Text");
                node.text = newText;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x"))
            {
                Debug.Log(GetNodeAtPoint(Event.current.mousePosition).uniqueID);
                deletingNode = node;
            }
            DrawLinkButtons(node);

            if (GUILayout.Button("+"))
            {
                Debug.Log("Add Child to node " + node.uniqueID);
                creatingNode = node;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.childrenIDs.Contains(node.uniqueID))
            {
                if (GUILayout.Button("Unlink"))
                {
                    Undo.RecordObject(selectedDialog, "unLink Dialog Nodes");
                    linkingParentNode.childrenIDs.Remove(node.uniqueID);
                    linkingParentNode = null;
                }
            }
            else /*if (!node.childrenIDs.Contains(linkingParentNode.uniqueID))*/
            {
                if (GUILayout.Button("Child"))
                {
                    Undo.RecordObject(selectedDialog, "Link Dialog Nodes");
                    linkingParentNode.childrenIDs.Add(node.uniqueID);
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogNode node)
        {
            Vector2 startPotition = new Vector2(node.rect.xMax-7, node.rect.center.y);
            foreach (DialogNode childNode in selectedDialog.GetAllChildren(node))
            {
                Vector2 endPotition = new Vector2(childNode.rect.xMin+7, childNode.rect.center.y);
                Vector2 controlPointOffset = endPotition - startPotition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPotition, endPotition, startPotition + controlPointOffset, endPotition - controlPointOffset, Color.white, null, 4f);

            }
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

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
        [NonSerialized] GUIStyle PlayerNodeStyle;
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
                return true;
            }
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

            PlayerNodeStyle = new GUIStyle();
            PlayerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            PlayerNodeStyle.normal.textColor = Color.blue;
            PlayerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            PlayerNodeStyle.border = new RectOffset(12, 12, 12, 12);

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
                Rect texCoords = new(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
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
                    selectedDialog.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialog.DeleteNode(deletingNode);
                    deletingNode = null;
                }

            }
        }
        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode !=null)
                {

                    draggingOffsets = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    isDraggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialog;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffsets);
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
            GUIStyle style = nodeStyle;
            if (node.IsPlayerSpeaking())
            {
                style = PlayerNodeStyle;
            }
            GUILayout.BeginArea(node.GetRect(), style);

            EditorGUILayout.LabelField("ID: " + node.name, EditorStyles.whiteBoldLabel);
            EditorGUILayout.LabelField("Dialogue:");
            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x"))
            {
                deletingNode = node;
            }
            DrawLinkButtons(node);

            if (GUILayout.Button("+"))
            {
                Debug.Log("Add Child to node " + node.name);
                creatingNode = node;
            }
            GUILayout.EndHorizontal();
            DrawStatePlayer(node);

            GUILayout.EndArea();
        }

        private static void DrawStatePlayer(DialogNode node)
        {
            if (node.IsPlayerSpeaking())
            {
                if (GUILayout.Toggle(node.IsPlayerSpeaking(), "Is Player"))
                {
                    node.SetPlayerSpeaking(!node.IsPlayerSpeaking());
                }
            }
            else
            {
                if (GUILayout.Toggle(node.IsPlayerSpeaking(), "Is NPC"))
                {
                    node.SetPlayerSpeaking(!node.IsPlayerSpeaking());
                }
            }
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
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                   
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else /*if (!node.childrenIDs.Contains(linkingParentNode.uniqueID))*/
            {
                if (GUILayout.Button("Child"))
                {
                    Undo.RecordObject(selectedDialog, "Link Dialog Nodes");
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogNode node)
        {
            Vector2 startPotition = new(node.GetRect().xMax-7, node.GetRect().center.y);
            foreach (DialogNode childNode in selectedDialog.GetAllChildren(node))
            {
                Vector2 endPotition = new(childNode.GetRect().xMin+7, childNode.GetRect().center.y);
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
                if (node.GetRect().Contains(mousePoint))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
}

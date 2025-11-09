using Codice.CM.Common.Mount;
using PlasticGui.WorkspaceWindow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.ShaderGraph.Serialization;
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

        [NonSerialized] float zoom = 1f;
        const float MinZoom = 0.4f;
        const float MaxZoom = 2.5f;
        const float ZoomStep = 0.1f;


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
            #region primera version
            //if (Event.current.type == EventType.MouseDown && draggingNode == null)
            //{
            //    draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
            //    if (draggingNode !=null)
            //    {

            //        draggingOffsets = draggingNode.GetRect().position - Event.current.mousePosition;
            //        Selection.activeObject = draggingNode;
            //    }
            //    else
            //    {
            //        isDraggingCanvas = true;
            //        draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
            //        Selection.activeObject = selectedDialog;
            //    }
            //}
            //else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            //{
            //    draggingNode.SetPosition(Event.current.mousePosition + draggingOffsets);
            //    GUI.changed = true;
            //}
            //else if (Event.current.type == EventType.MouseDrag && isDraggingCanvas)
            //{
            //    scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
            //    GUI.changed = true;
            //}
            //else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            //{
            //    draggingNode = null;
            //}
            //else if (Event.current.type == EventType.MouseUp && isDraggingCanvas)
            //{
            //    isDraggingCanvas = false;
            //}
            #endregion
            Event e = Event.current;

            // Zoom con rueda
            if (e.type == EventType.ScrollWheel && (e.control || e.command))
            {
                Vector2 mousePos = e.mousePosition;
                Vector2 contentPos = (mousePos + scrollPosition) / zoom;
                float delta = -e.delta.y; // rueda arriba positivo
                float target = Mathf.Clamp(zoom + delta * ZoomStep * 0.2f, MinZoom, MaxZoom);
                if (!Mathf.Approximately(target, zoom))
                {
                    scrollPosition = contentPos * target - mousePos;
                    zoom = target;
                    GUI.changed = true;
                    e.Use();
                }
            }

            if (e.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint((e.mousePosition + scrollPosition) / zoom);
                if (draggingNode != null)
                {
                    draggingOffsets = draggingNode.GetRect().position - ((e.mousePosition + scrollPosition) / zoom);
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    isDraggingCanvas = true;
                    draggingCanvasOffset = e.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialog;
                }
            }
            else if (e.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(((e.mousePosition + scrollPosition) / zoom) + draggingOffsets);
                GUI.changed = true;
            }
            else if (e.type == EventType.MouseDrag && isDraggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - e.mousePosition;
                GUI.changed = true;
            }
            else if (e.type == EventType.MouseUp && draggingNode != null)
                draggingNode = null;
            else if (e.type == EventType.MouseUp && isDraggingCanvas)
                isDraggingCanvas = false;
        }

      

        private void DrawNode(DialogNode node)
        {
            GUIStyle style = node.IsPlayerSpeaking() ? PlayerNodeStyle : nodeStyle;
            Rect r = node.GetRect();
            Rect drawRect = new(r.x * zoom, r.y * zoom, r.width * zoom, r.height * zoom);
            GUILayout.BeginArea(drawRect, style);

            //GUIStyle style = nodeStyle;
            //if (node.IsPlayerSpeaking())
            //{
            //    style = PlayerNodeStyle;
            //}
            //GUILayout.BeginArea(node.GetRect(), style);

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

        // Plan (pseudocódigo detallado):
        // 1. Obtener el estado actual: bool isPlayer = node.IsPlayerSpeaking()
        // 2. Mostrar un Toggle que refleje ese estado: bool newIsPlayer = GUILayout.Toggle(isPlayer, "Player Speaking")
        // 3. Si el estado cambió:
        //    a. Registrar el cambio para Undo: Undo.RecordObject(node, "Toggle Speaker")
        //    b. Aplicar el nuevo estado: node.SetPlayerSpeaking(newIsPlayer)
        //    c. Marcar el objeto como modificado para que Unity lo guarde: EditorUtility.SetDirty(node)
        // 4. (Opcional) Si se desea mostrar ambos estados como botones, se podría usar dos toggles o radio buttons,
        //    pero aquí se mantiene un toggle claro y sencillo: activo = Player, inactivo = NPC.

        private static void DrawStatePlayer(DialogNode node)
        {
            bool current = node.IsPlayerSpeaking();
            // Mostrar un toggle donde true = Player hablando, false = NPC hablando.
            string label = "Player Speaking";
            bool next = GUILayout.Toggle(current, label);

            if (next != current)
            {
                Undo.RecordObject(node, "Toggle Speaker");
                node.SetPlayerSpeaking(next);
                EditorUtility.SetDirty(node);
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
            //Vector2 startPotition = new(node.GetRect().xMax-7, node.GetRect().center.y);
            //foreach (DialogNode childNode in selectedDialog.GetAllChildren(node))
            //{
            //    Vector2 endPotition = new(childNode.GetRect().xMin+7, childNode.GetRect().center.y);
            //    Vector2 controlPointOffset = endPotition - startPotition;
            //    controlPointOffset.y = 0;
            //    controlPointOffset.x *= 0.8f;
            //    Handles.DrawBezier(startPotition, endPotition, startPotition + controlPointOffset, endPotition - controlPointOffset, Color.white, null, 4f);

            //}
            Rect r = node.GetRect();
            Vector2 start = new(r.xMax * zoom - 7 * zoom, r.center.y * zoom);
            foreach (var child in selectedDialog.GetAllChildren(node))
            {
                Rect cr = child.GetRect();
                Vector2 end = new(cr.xMin * zoom + 7 * zoom, cr.center.y * zoom);
                Vector2 cp = end - start;
                cp.y = 0; cp.x *= 0.8f;
                Handles.DrawBezier(start, end, start + cp, end - cp, Color.white, null, 4f);
            }
        }

        private DialogNode GetNodeAtPoint(Vector2 mousePoint)
        {
            //DialogNode foundNode = null;
            //foreach (DialogNode node in selectedDialog.GetAllNodes())
            //{
            //    if (node.GetRect().Contains(mousePoint))
            //    {
            //        foundNode = node;
            //    }
            //}
            //return foundNode;
            DialogNode found = null;
            foreach (var n in selectedDialog.GetAllNodes())
                if (n.GetRect().Contains(mousePoint))
                    found = n;
            return found;
        }
    }
}

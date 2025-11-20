using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace The_cofessor.Personajes.Dialogs
{
#if UNITY_EDITOR
    public static class Interpret
    {
        /// <summary>
        /// Importa un archivo Twine (Harlowe HTML exportado) y crea un ScriptableObject Dialog con DialogNode
        /// solo para las passages relacionadas (en la misma componente conectada) con cualquier passage cuyo
        /// nombre contenga "Tarde" (case-insensitive) si leftOnlyTardeRelated==true.
        /// </summary>
        public static Dialog ImportTwineHtml(string htmlFilePath, string assetPath, bool onlyTardeRelated = true)
        {
            if (!File.Exists(htmlFilePath))
            {
                Debug.LogError($"Interpret: no existe el fichero '{htmlFilePath}'");
                return null;
            }

            string html = File.ReadAllText(htmlFilePath);
            var passageRegex = new Regex(@"<tw-passagedata\b([^>]*)>([\s\S]*?)<\/tw-passagedata>", RegexOptions.IgnoreCase);
            var attrRegex = new Regex(@"(\w+)=""([^""]*)""");
            var linkRegex = new Regex(@"\[\[([^\]]+)\]\]", RegexOptions.Compiled);

            var matches = passageRegex.Matches(html);
            if (matches.Count == 0)
            {
                Debug.LogWarning("Interpret: no se encontraron <tw-passagedata> en el HTML.");
            }

            // Primero: parsear metadata y links sin crear assets todavía
            var passages = new List<(string pid, string name, string tags, string rawText, List<string> links)>();
            for (int i = 0; i < matches.Count; i++)
            {
                var attrString = matches[i].Groups[1].Value;
                var innerRaw = matches[i].Groups[2].Value;
                string pid = null;
                string name = null;
                string tags = null;
                foreach (Match a in attrRegex.Matches(attrString))
                {
                    var key = a.Groups[1].Value;
                    var val = WebUtility.HtmlDecode(a.Groups[2].Value);
                    if (key.Equals("pid", StringComparison.OrdinalIgnoreCase)) pid = val;
                    if (key.Equals("name", StringComparison.OrdinalIgnoreCase)) name = val;
                    if (key.Equals("tags", StringComparison.OrdinalIgnoreCase)) tags = val;
                }
                if (string.IsNullOrEmpty(name))
                {
                    name = pid ?? $"passage_{i}";
                }

                string text = WebUtility.HtmlDecode(innerRaw).Trim();

                var linkMatches = linkRegex.Matches(text);
                var linkTargets = new List<string>();
                foreach (Match lm in linkMatches)
                {
                    var linkContent = lm.Groups[1].Value.Trim();
                    string passageName = null;
                    if (linkContent.Contains("->"))
                    {
                        var parts = linkContent.Split(new[] { "->" }, 2, StringSplitOptions.None);
                        passageName = parts[1].Trim();
                    }
                    else if (linkContent.Contains("<-"))
                    {
                        var parts = linkContent.Split(new[] { "<-" }, 2, StringSplitOptions.None);
                        passageName = parts[0].Trim();
                    }
                    else
                    {
                        passageName = linkContent.Trim();
                    }
                    passageName = WebUtility.HtmlDecode(passageName).Trim().Trim('"');
                    if (!string.IsNullOrEmpty(passageName))
                        linkTargets.Add(passageName);
                }

                passages.Add((pid, name, tags, text, linkTargets));
            }

            // Construir grafo no dirigido de conexiones entre passages
            var nameToIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < passages.Count; i++)
                nameToIndex[passages[i].name] = i;

            var adj = new List<HashSet<int>>(passages.Count);
            for (int i = 0; i < passages.Count; i++) adj.Add(new HashSet<int>());

            for (int i = 0; i < passages.Count; i++)
            {
                foreach (var target in passages[i].links)
                {
                    if (nameToIndex.TryGetValue(target, out int j))
                    {
                        adj[i].Add(j);
                        adj[j].Add(i); // no dirigido para "relacion"
                    }
                }
            }

            // Determinar conjunto de passages a importar: componentes conectadas a los que contengan "Tarde"
            var toInclude = new HashSet<int>();
            if (onlyTardeRelated)
            {
                var seeds = new Queue<int>();
                for (int i = 0; i < passages.Count; i++)
                {
                    if (passages[i].name.IndexOf("Tarde", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        seeds.Enqueue(i);
                        toInclude.Add(i);
                    }
                }

                if (seeds.Count == 0)
                {
                    Debug.LogWarning("Interpret: no se encontraron passages con 'Tarde' en el nombre. No se importará nada.");
                }
                else
                {
                    // BFS sobre grafo no dirigido
                    while (seeds.Count > 0)
                    {
                        var cur = seeds.Dequeue();
                        foreach (var nb in adj[cur])
                        {
                            if (!toInclude.Contains(nb))
                            {
                                toInclude.Add(nb);
                                seeds.Enqueue(nb);
                            }
                        }
                    }
                }
            }
            else
            {
                // incluir todos
                for (int i = 0; i < passages.Count; i++) toInclude.Add(i);
            }

            // Crear asset Dialog y los DialogNode solo para los indices incluidos
            var dialog = ScriptableObject.CreateInstance<Dialog>();
            AssetDatabase.CreateAsset(dialog, assetPath);
            AssetDatabase.SaveAssets();

            var so = new SerializedObject(dialog);
            var nodesProp = so.FindProperty("nodes");

            var includedIndices = toInclude.OrderBy(i => i).ToList();
            nodesProp.arraySize = includedIndices.Count;
            so.ApplyModifiedProperties();

            var nodeByName = new Dictionary<string, DialogNode>(StringComparer.OrdinalIgnoreCase);
            for (int k = 0; k < includedIndices.Count; k++)
            {
                int i = includedIndices[k];
                var p = passages[i];

                var node = ScriptableObject.CreateInstance<DialogNode>();
                AssetDatabase.AddObjectToAsset(node, dialog);

                node.SetID(p.name);
                node.SetText(p.rawText);

                if (!string.IsNullOrEmpty(p.tags) && p.tags.IndexOf("player", StringComparison.OrdinalIgnoreCase) >= 0)
                    node.SetPlayerSpeaking(true);

                nodeByName[p.name] = node;

                var element = nodesProp.GetArrayElementAtIndex(k);
                element.objectReferenceValue = node;
            }

            so.ApplyModifiedProperties();

            // Crear relaciones children solo cuando el target está incluido
            foreach (var kv in nodeByName)
            {
                var srcName = kv.Key;
                var srcNode = kv.Value;
                int srcIndex = nameToIndex[srcName];
                var p = passages[srcIndex];
                foreach (var target in p.links)
                {
                    if (nodeByName.ContainsKey(target))
                    {
                        srcNode.AddChild(target);
                    }
                }
            }

            EditorUtility.SetDirty(dialog);
            AssetDatabase.SaveAssets();

            Debug.Log($"Interpret: importado '{htmlFilePath}' → '{assetPath}' ({nodeByName.Count} nodos). onlyTardeRelated={onlyTardeRelated}");
            return dialog;
        }

        [MenuItem("Assets/TheCofessor/Import Twine HTML to Dialog (Tarde related)", priority = 200)]
        private static void MenuImportTarde()
        {
            string htmlPath = EditorUtility.OpenFilePanel("Selecciona Twine HTML", "", "html");
            if (string.IsNullOrEmpty(htmlPath)) return;
            string assetPath = EditorUtility.SaveFilePanelInProject("Guardar Dialog asset", "NewDialog", "asset", "Ruta para el Dialog asset");
            if (string.IsNullOrEmpty(assetPath)) return;
            ImportTwineHtml(htmlPath, assetPath, true);
        }
    }
#endif
}

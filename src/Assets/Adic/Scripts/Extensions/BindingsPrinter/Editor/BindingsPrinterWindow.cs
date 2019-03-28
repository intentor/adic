using System.Text;
using UnityEngine;
using UnityEditor;

namespace Adic.Extenstions.BindingsPrinter {
    /// <summary>
    /// Prints bindings from containers in the current scene.
    /// </summary>
    public class BindingsPrinterWindow : EditorWindow {
        /// <summary>Window margin value.</summary>
        private const float WINDOW_MARGIN = 10.0f;
        /// <summary>Current editor.</summary>
        private static BindingsPrinterWindow editor;

        /// <summary>Current scroll positioning.</summary>
        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/Adic/Bindings Printer")]
        protected static void Init() {
            editor = EditorWindow.GetWindow<BindingsPrinterWindow>("Bindings Printer", typeof(SceneView));
        }

        protected void OnGUI() {
            if (!editor) {
                editor = (BindingsPrinterWindow) EditorWindow.GetWindow<BindingsPrinterWindow>();
            }

            if (!Application.isPlaying) {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Please execute the bindings printer on Play Mode", EditorStyles.message);
                GUILayout.FlexibleSpace();
                return;
            }

            if (ContextRoot.containersData == null || ContextRoot.containersData.Count == 0) {
                GUILayout.FlexibleSpace();
                GUILayout.Label("There are no containers in the current scene", EditorStyles.message);
                GUILayout.FlexibleSpace();
                return;
            }

            // Add window margin.
            GUILayout.BeginHorizontal();
            GUILayout.Space(WINDOW_MARGIN);
            GUILayout.BeginVertical();
			
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);

            GUILayout.Space(WINDOW_MARGIN);
            GUILayout.Label("Adic Bindings Printer", EditorStyles.title);
            GUILayout.Label("Displays all bindings of all available containers", EditorStyles.containerInfo);            

            if (GUILayout.Button("Copy to clipboard")) {
                this.CopyToClipboard();
            }

            // Display the containers and their bindings.
            for (int dataIndex = 0; dataIndex < ContextRoot.containersData.Count; dataIndex++) {
                var data = ContextRoot.containersData[dataIndex];
                var bindings = data.container.GetBindings();

                GUILayout.Space(20f);
                GUILayout.Label("CONTAINER", EditorStyles.containerInfo);
                GUILayout.Label(
                    string.Format("[{1}] {0} ({2} | {3})", data.container.GetType().FullName, dataIndex,
                        data.container.identifier, (data.destroyOnLoad ? "destroy on load" : "singleton")
                    ),
                    EditorStyles.title
                );

                GUILayout.Space(10f);

                // Add indentation.
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUILayout.BeginVertical();

                for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
                    var binding = bindings[bindingIndex];

                    GUILayout.Label(binding.ToString(), EditorStyles.bindinds);
                }
				
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(WINDOW_MARGIN);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void CopyToClipboard()
        {
            var sb = new StringBuilder();

            for (int dataIndex = 0; dataIndex < ContextRoot.containersData.Count; dataIndex++) {
                var data = ContextRoot.containersData[dataIndex];
                var bindings = data.container.GetBindings();
                sb.AppendFormat("[{1}] {0} ({2} | {3})\n", data.container.GetType().FullName, dataIndex,
                    data.container.identifier, data.destroyOnLoad ? "destroy on load" : "singleton");

                for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
                    var binding = bindings[bindingIndex];

                    sb.AppendLine(binding.ToString());
                }
            }

            var te = new TextEditor();
            te.text = sb.ToString();
            te.SelectAll();
            te.Copy();
        }
    }
}
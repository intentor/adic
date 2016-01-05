using UnityEngine;
using UnityEditor;
using System;

namespace Adic {
	/// <summary>
	/// Context root MonoBehaviour editor.
	/// </summary>
	[CustomEditor(typeof(ContextRoot), true)]
	public class ContextRootEditor : Editor {
		/// <summary>Default script execution order time.</summary>
		protected const int DEFAULT_EXECUTION_ORDER_TIME = -100;

		/// <summary>Object to be edited.</summary>
		protected ContextRoot editorItem;

		protected void OnEnable() {
			this.editorItem = (ContextRoot)this.target;
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.HelpBox("Use the button below to ensure the context root " +
				"will be executed before any other injectable MonoBehaviour.", MessageType.Info);
			if (GUILayout.Button("Set execution order")) {
				var scriptType = this.editorItem.GetType();
				var executionOrder = this.SetCurrentScriptExecutionOrder(scriptType);

				EditorUtility.DisplayDialog("Script execution order",
					string.Format("{0} execution order set to {1}.", scriptType.Name, executionOrder),
					"Ok");
			}
		}

		/// <summary>
		/// Sets the execution order for the current script.
		/// </summary>
		/// <param name="scriptType">Script type.</param>
		/// <returns>The execution order that was set.</returns>
		protected int SetCurrentScriptExecutionOrder(Type scriptType) {
			var executionOrder = DEFAULT_EXECUTION_ORDER_TIME;
			MonoScript selectedScript = null;

			//Gets the first available execution order.
			var available = false;
			while (!available) {
				available = true;

				foreach (var script in MonoImporter.GetAllRuntimeMonoScripts()) {
					if (selectedScript == null && script.GetClass() == scriptType) {
						selectedScript = script;
					}

					if (script.GetClass() != scriptType && MonoImporter.GetExecutionOrder(script) == executionOrder) {
						executionOrder += DEFAULT_EXECUTION_ORDER_TIME;
						available = false;
						continue;
					}
				}
			}

			//Sets the execution order.
			MonoImporter.SetExecutionOrder(selectedScript, executionOrder);

			return executionOrder;
		}
	}
}
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Adic.Extensions.ContextRoots.Utils;
using Adic.Util;

namespace Adic.Extensions.ContextRoots.Editors {
	/// <summary>
	/// Context root MonoBehaviour editor.
	/// </summary>
	[CustomEditor(typeof(ContextRoot), true)]
	public class ContextRootEditor : Editor {
		/// <summary>Default context root script execution order time.</summary>
		protected const int DEFAULT_EXECUTION_ORDER = -100;

		/// <summary>Object to be edited.</summary>
		protected ContextRoot editorItem;
		/// <summary>Custom script types.</summary>
		protected string[] customScripts;

		protected void OnEnable() {
			this.editorItem = (ContextRoot)this.target;

			var customTypes = TypeUtils.GetAssignableTypes(typeof(MonoBehaviour));
			var customScriptsNames = new List<string>();
			foreach (var customType in customTypes) {
				//Prevent Adic MonoBehaviours from entering the list.
				if (!customType.FullName.StartsWith("Adic")) {
					customScriptsNames.Add(customType.FullName);
				}
			}
			this.customScripts = customScriptsNames.ToArray();
		}

		public override void OnInspectorGUI() {
			//Injection type.
			this.editorItem.injectionType = (ContextRoot.MonoBehaviourInjectionType)
				EditorGUILayout.EnumPopup(new GUIContent("Injection type", "Type of injection on MonoBehaviours."),
					this.editorItem.injectionType);

			//Base injection type name.
			if (this.editorItem.injectionType == ContextRoot.MonoBehaviourInjectionType.BaseType) {
				var index = Array.IndexOf<string>(this.customScripts, this.editorItem.baseBehaviourTypeName);
				index = EditorGUILayout.Popup("Base behaviour type", index, this.customScripts);
				if (index >= 0) this.editorItem.baseBehaviourTypeName = this.customScripts[index];
			} else {
				this.editorItem.baseBehaviourTypeName = string.Empty;
			}

			//Set execution order.
			EditorGUILayout.HelpBox("Use the button below to ensure the context root " +
				"will be executed before any other injectable MonoBehaviour.", MessageType.Info);
			if (GUILayout.Button("Set execution order")) {
				var contextRootType = this.editorItem.GetType();
				var contextRootOrder = ExecutionOrderUtils.SetScriptExecutionOrder(contextRootType, DEFAULT_EXECUTION_ORDER);
				var message = string.Format("{0} execution order set to {1}.", contextRootType.Name, contextRootOrder);

				EditorUtility.DisplayDialog("Script execution order", message, "Ok");
			}
		}
	}
}
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adic.Commander.Behaviours.Editors {
	/// <summary>
	/// Abstract type for editors that use namespace and type name lookup for commands.
	/// </summary>
	public abstract class NamespaceCommandEditor<T> : Editor where T : NamespaceCommandBehaviour {
		/// <summary>Component to be edited.</summary>
		protected T component;
		/// <summary>The available commands' names, ordered by namespace.</summary>
		protected Dictionary<string, IList<string>> types;
		/// <summary>The available commands' namespace names.</summary>
		protected string[] namespaceNames;

		protected void OnEnable () {
			this.component = (T)this.target;
			
			var availableCommands = CommanderUtils.GetAvailableCommands();
			this.types = CommanderUtils.GetTypesAsString(availableCommands);
			this.namespaceNames = this.types.Keys.ToArray();
		}
		
		public override void OnInspectorGUI() {			
			//Namespace.
			var namespaceIndex = this.GetNamespaceIndex(this.component.commandNamespace);
			namespaceIndex = EditorGUILayout.Popup("Namespace", namespaceIndex, this.namespaceNames);
			this.component.commandNamespace = this.namespaceNames[namespaceIndex];
			
			//Command.
			var commands = this.types[this.component.commandNamespace];
			var commandIndex = commands.IndexOf(this.component.commandName);
			if (commandIndex < 0) commandIndex = 0;
			commandIndex = EditorGUILayout.Popup("Command", commandIndex, commands.ToArray());
			this.component.commandName = commands[commandIndex];
			
			//Asks the editor to update the target.
			EditorUtility.SetDirty(this.target);
		}
		
		/// <summary>
		/// Gets the index of a namespace.
		/// </summary>
		/// <param name="namespaceName">Namespace name.</param>
		/// <returns>The namespace index.</returns>
		protected int GetNamespaceIndex(string namespaceName) {
			var selectedIndex = 0;
			
			for (var index = 0; index < this.namespaceNames.Length; index++) {
				if (this.namespaceNames[index] == namespaceName) {
					selectedIndex = index;
					break;
				}
			}
			
			return selectedIndex;
		}
	}
}
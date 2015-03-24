using UnityEngine;
using System;
using Adic.Container;

namespace Adic.Commander.Behaviours {
	/// <summary>
	/// Provides a routine to call a given command.
	/// 
	/// To dispatch the command, the script looks for a container that
	/// has a binding for the given command.
	/// </summary>
	[AddComponentMenu("Adic/Commander/Command dispatch")]
	public class CommandDispatch : NamespaceCommandBehaviour {		
		/// <summary>The type of the command to be called.</summary>
		protected Type commandType;
		
		/// <summary>
		/// Called when the script is awaken.
		/// </summary>
		protected void Awake() {
			//Finds the type based on the namespace and command name.
			if (!string.IsNullOrEmpty(this.commandName)) {
				if (string.IsNullOrEmpty(this.commandNamespace) ||
				    this.commandNamespace == "-") {
					this.commandType = Type.GetType(this.commandName);
				} else {
					this.commandType = Type.GetType(
						string.Format("{0}.{1}", this.commandNamespace, this.commandName));
				}
			}
		}
		
		/// <summary>
		/// Dispatches a command.
		/// </summary>
		public void DispatchCommand() {
			CommanderUtils.DispatchCommand(this.commandType);
		}
	}
}
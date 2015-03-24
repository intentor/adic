using UnityEngine;
using System.Collections;

namespace Adic.Commander.Behaviours {
	/// <summary>
	/// Abstract class for defining behaviours that referes to a command by namespace.
	/// </summary>
	[AddComponentMenu("")]
	public abstract class NamespaceCommandBehaviour : MonoBehaviour {	
		/// <summary>The command namespace.</summary>
		public string commandNamespace;
		/// <summary>The command name.</summary>
		public string commandName;
	}
}

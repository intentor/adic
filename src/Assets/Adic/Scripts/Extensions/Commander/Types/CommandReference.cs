using System;
using Adic.Commander;
using Adic.Util;

namespace Adic {
	/// <summary>
	///	Represents a reference to a command that can be called by code.
	/// 
	/// It's useful when used as a public property on MonoBehaviours.
	/// </summary>
	[Serializable]
	public class CommandReference  {
		/// <summary>The command namespace.</summary>
		public string commandNamespace;
		/// <summary>The command name.</summary>
		public string commandName;
		
		/// <summary>
		/// Dispatches a command.
		/// </summary>
		public void DispatchCommand() {
			var type = TypeUtils.GetType(this.commandNamespace, this.commandName);
			CommanderUtils.DispatchCommand(type);
		}
	}
}
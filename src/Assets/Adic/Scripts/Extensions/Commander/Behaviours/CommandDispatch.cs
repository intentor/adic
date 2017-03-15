using UnityEngine;
using System;
using Adic.Container;
using Adic.Util;

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
            this.commandType = TypeUtils.GetType(this.commandNamespace, this.commandName);
        }

        /// <summary>
        /// Dispatches the command.
        /// </summary>
        public void DispatchCommand() {
            CommanderUtils.DispatchCommand(this.commandType);
        }

        /// <summary>
        /// Dispatches the command.
        /// </summary>
        /// <param name="parameters">Command parameters.</param>
        public void DispatchCommand(params object[] parameters) {
            CommanderUtils.DispatchCommand(this.commandType, parameters);
        }
    }
}
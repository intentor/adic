using UnityEngine;
using System;
using Adic.Container;

namespace Adic.Commander.Behaviours {
    /// <summary>
    /// Dispatches a command based on a timer.
    /// 
    /// To dispatch the command, the script looks for a container that
    /// has a binding for the given command.
    /// </summary>
    [AddComponentMenu("Adic/Commander/Timed command dispatch")]
    public class TimedCommandDispatch : CommandDispatch {
        /// <summary>The duration of the timer, in seconds.</summary>
        public float timer;

        /// <summary>
        /// Called when the script is enabled.
        /// </summary>
        protected void OnEnable() {
            this.Invoke("DispatchCommand", this.timer);
        }
    }
}
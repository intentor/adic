using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adic.Examples.Events {
    /// <summary>
    /// Event receiver behaviour.
    /// </summary>
    public class EventReceiver : IUpdatable, ILateUpdatable, IFixedUpdatable, IFocusable, IPausable, IQuitable {
        public void Update() {
            Debug.Log("Updating...");
        }

        public void LateUpdate() {
            Debug.Log("Late updating...");
        }

        public void FixedUpdate() {
            Debug.Log("Fixed updating...");
        }

        public void OnApplicationFocus(bool hasFocus) {
            Debug.LogFormat("Has focus? {0}", hasFocus);
        }

        public void OnApplicationPause(bool isPaused) {
            Debug.LogFormat("Is paused? {0}", isPaused);
        }

        public void OnApplicationQuit() {
            Debug.Log("Game quit");
        }
    }
}

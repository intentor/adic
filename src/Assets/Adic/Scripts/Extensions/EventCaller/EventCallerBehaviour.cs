using UnityEngine;
using System.Collections.Generic;

namespace Adic {
    /// <summary>
    /// Trigger events on objects added by the Event Caller Container Extension.
    /// </summary>
    public class EventCallerBehaviour : MonoBehaviour {
        /// <summary>Event Caller extension related to this behaviour.</summary>
        public EventCallerContainerExtension extension { get; set; }

        /// <summary>
        /// Called once per frame.
        /// </summary>
        protected void Update() {
            // If the game is paused, Update is not called.
            if (Mathf.Approximately(Time.deltaTime, 0)) {
                return;
            }

            for (var objIndex = 0; objIndex < extension.updateable.Count; objIndex++) {
                extension.updateable[objIndex].Update();
            }
        }

        /// <summary>
        /// Called once per frame after Update has finished.
        /// </summary>
        protected void LateUpdate() {
            // If the game is paused, LateUpdate is not called.
            if (Mathf.Approximately(Time.deltaTime, 0)) {
                return;
            }

            for (var objIndex = 0; objIndex < extension.lateUpdateable.Count; objIndex++) {
                extension.lateUpdateable[objIndex].LateUpdate();
            }
        }

        /// <summary>
        /// Called on a reliable time. Can be called more frequently than Update.
        /// </summary>
        protected void FixedUpdate() {
            for (var objIndex = 0; objIndex < extension.fixedUpdateable.Count; objIndex++) {
                extension.fixedUpdateable[objIndex].FixedUpdate();
            }
        }

        /// <summary>
        /// Called when the application focus is changing.
        /// </summary>
        /// <param name="hasFocus">If set to <c>true</c> has focus.</param>
        protected void OnApplicationFocus(bool hasFocus) {
            for (var objIndex = 0; objIndex < extension.focusable.Count; objIndex++) {
                extension.focusable[objIndex].OnApplicationFocus(hasFocus);
            }
        }

        /// <summary>
        /// Called when the application is pausing.
        /// </summary>
        /// <param name="isPaused">If set to <c>true</c> is paused.</param>
        protected void OnApplicationPause(bool isPaused) {
            for (var objIndex = 0; objIndex < extension.pausable.Count; objIndex++) {
                extension.pausable[objIndex].OnApplicationPause(isPaused);
            }
        }

        /// <summary>
        /// Called when the application is quitting.
        /// </summary>
        protected void OnApplicationQuit() {
            for (var objIndex = 0; objIndex < extension.quitable.Count; objIndex++) {
                extension.quitable[objIndex].OnApplicationQuit();
            }
        }

        /// <summary>
        /// Called when the component is destroyed.
        /// </summary>
        protected void OnDestroy() {
            foreach (var obj in extension.disposable) {
                obj.Dispose();
            }
        }
    }
}
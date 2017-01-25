using UnityEngine;
using System.Collections.Generic;

namespace Adic {
    /// <summary>
    /// Trigger events on objects added by the Event Caller Container Extension.
    /// </summary>
    public class EventCallerBehaviour : MonoBehaviour {
        /// <summary>
        /// Called once per frame.
        /// </summary>
        protected void Update() {
            //It the game is paused, Update is not called.
            if (Mathf.Approximately(Time.deltaTime, 0)) {
                return;
            }

            for (var objIndex = 0; objIndex < EventCallerContainerExtension.updateable.Count; objIndex++) {
                EventCallerContainerExtension.updateable[objIndex].Update();
            }
        }

        /// <summary>
        /// Called once per frame after Update has finished.
        /// </summary>
        protected void LateUpdate() {
            //It the game is paused, LateUpdate is not called.
            if (Mathf.Approximately(Time.deltaTime, 0)) {
                return;
            }

            for (var objIndex = 0; objIndex < EventCallerContainerExtension.lateUpdateable.Count; objIndex++) {
                EventCallerContainerExtension.lateUpdateable[objIndex].LateUpdate();
            }
        }
        /// <summary>
        /// Called on a reliable time. Can be called more frequently than Update.
        /// </summary>
        protected void FixedUpdate() {
            for (var objIndex = 0; objIndex < EventCallerContainerExtension.fixedUpdateable.Count; objIndex++) {
                EventCallerContainerExtension.fixedUpdateable[objIndex].FixedUpdate();
            }
        }

        /// <summary>
        /// Called when the application focus is changing.
        /// </summary>
        /// <param name="hasFocus">If set to <c>true</c> has focus.</param>
        protected void OnApplicationFocus(bool hasFocus) {
            for (var objIndex = 0; objIndex < EventCallerContainerExtension.focusable.Count; objIndex++) {
                EventCallerContainerExtension.focusable[objIndex].OnApplicationFocus(hasFocus);
            }
        }

        /// <summary>
        /// Called when the application is pausing.
        /// </summary>
        /// <param name="isPaused">If set to <c>true</c> is paused.</param>
        protected void OnApplicationPause(bool isPaused) {
            for (var objIndex = 0; objIndex < EventCallerContainerExtension.pausable.Count; objIndex++) {
                EventCallerContainerExtension.pausable[objIndex].OnApplicationPause(isPaused);
            }
        }

        /// <summary>
        /// Called when the application is quitting.
        /// </summary>
        protected void OnApplicationQuit() {
            for (var objIndex = 0; objIndex < EventCallerContainerExtension.quitable.Count; objIndex++) {
                EventCallerContainerExtension.quitable[objIndex].OnApplicationQuit();
            }
        }

        /// <summary>
        /// Called when the component is destroyed.
        /// </summary>
        protected void OnDestroy() {
            foreach (var obj in EventCallerContainerExtension.disposable) {
                obj.Dispose();
            }

            EventCallerContainerExtension.disposable.Clear();
            EventCallerContainerExtension.updateable.Clear();
            EventCallerContainerExtension.focusable.Clear();
            EventCallerContainerExtension.pausable.Clear();
            EventCallerContainerExtension.quitable.Clear();
        }
    }
}
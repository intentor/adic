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
			if (Mathf.Approximately(Time.deltaTime, 0)) return;

			for (var objIndex = 0; objIndex < EventCallerContainerExtension.updateable.Count; objIndex++) {
				EventCallerContainerExtension.updateable[objIndex].Update();
			}
		}

		/// <summary>
		/// Called when the application's focus is changing
		/// </summary>
		protected void OnApplicationFocus(bool hasFocus)
		{
			for (var objIndex = 0; objIndex < EventCallerContainerExtension.focusable.Count; objIndex++) {
				EventCallerContainerExtension.focusable[objIndex].OnApplicationFocus(hasFocus);
			}
		}

		/// <summary>
		/// Called when the application is pausing
		/// </summary>
		protected void OnApplicationPause(bool isPaused)
		{
			for (var objIndex = 0; objIndex < EventCallerContainerExtension.pausable.Count; objIndex++) {
				EventCallerContainerExtension.pausable[objIndex].OnApplicationPause(isPaused);
			}
		}

		/// <summary>
		/// Called when the application is quitting
		/// </summary>
		protected void OnApplicationQuit()
		{
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
			EventCallerContainerExtension.pausable.Clear();
			EventCallerContainerExtension.quitable.Clear();
		}
	}
}
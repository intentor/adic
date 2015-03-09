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
			if (Time.deltaTime == 0) return;

			for (var objIndex = 0; objIndex < EventCallerContainerExtension.updateable.Count; objIndex++) {
				EventCallerContainerExtension.updateable[objIndex].Update();
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
		}
	}
}
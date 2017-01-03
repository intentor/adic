using UnityEngine;
using System;
using System.Collections.Generic;
using Adic.Binding;
using Adic.Container;
using Adic.Injection;

namespace Adic {
	/// <summary>
	/// Event caller container extension.
	/// 
	/// Intercepts bindings and resolutions to check whether objects
	/// should be added to receive update/dispose events.
	/// </summary>
	public class EventCallerContainerExtension : IContainerExtension {
		/// <summary>The disposable objects.</summary>
		public static List<IDisposable> disposable = new List<IDisposable>();
		/// <summary>The updateable objects.</summary>
		public static List<IUpdatable> updateable = new List<IUpdatable>();
		/// <summary>The focusable objects.</summary>
		public static List<IFocusable> focusable = new List<IFocusable>();
		/// <summary>The pausable objects.</summary>
		public static List<IPausable> pausable = new List<IPausable>();
		/// <summary>The quitable objects.</summary>
		public static List<IQuitable> quitable = new List<IQuitable>();
		/// <summary>The event caller.</summary>
		public static EventCallerBehaviour eventCaller;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.EventCallerContainerExtension"/> class.
		/// </summary>
		public EventCallerContainerExtension() {
			//Creates a new game object for UpdateableBehaviour.
			var gameObject = new GameObject();
			gameObject.name = "EventCaller";
			eventCaller = gameObject.AddComponent<EventCallerBehaviour>();
		}

		public void OnRegister(IInjectionContainer container) {
			//Adds the container to the disposable list.
			disposable.Add(container);

			//Checks whether a binding for the ICommandDispatcher exists.
			if (container.ContainsBindingFor<ICommandDispatcher>()) {
				var dispatcher = container.Resolve<ICommandDispatcher>();
				BindUnityExtension(disposable, dispatcher);
			}

			container.afterAddBinding += this.OnAfterAddBinding;
			container.bindingResolution += this.OnBindingResolution;
		}
		
		public void OnUnregister(IInjectionContainer container) {
			container.afterAddBinding -= this.OnAfterAddBinding;
			container.bindingResolution -= this.OnBindingResolution;

			disposable.Clear();
			updateable.Clear();
			focusable.Clear();
			pausable.Clear();
			quitable.Clear();
			MonoBehaviour.Destroy(eventCaller);
		}

		/// <summary>
		/// handles the after add binding event.
		/// 
		/// Used to check whether singleton instances should be added to the updater.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="binding">Binding.</param>
		protected void OnAfterAddBinding(IBinder source, ref BindingInfo binding) {
			if (binding.instanceType == BindingInstance.Singleton) {				
				//Do not add commands.
				if (binding.value is ICommand) return;
				BindUnityExtension(disposable, binding.value);
				BindUnityExtension(updateable, binding.value);
				BindUnityExtension(focusable, binding.value);
				BindUnityExtension(pausable, binding.value);
				BindUnityExtension(quitable, binding.value);
			}
		}

		/// <summary>
		/// Handles the binding resolution event.
		/// 
		/// Used to check whether the resolved instance should be added to the updater.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="binding">Binding.</param>
		/// <param name="instance">Instance.</param>
		protected void OnBindingResolution(IInjector source, ref BindingInfo binding, ref object instance) {
			//Do not add commands.
			if (binding.instanceType == BindingInstance.Singleton || instance is ICommand) return;
			BindUnityExtension(disposable, instance);
			BindUnityExtension(updateable, instance);
			BindUnityExtension(focusable, instance);
			BindUnityExtension(pausable, instance);
			BindUnityExtension(quitable, instance);
		}

		protected void BindUnityExtension<T>(List<T> unityEventList, object value)
		{
			if (value is T && !unityEventList.Contains((T)value)) {
				unityEventList.Add((T)value);
			}
		}

	}
}
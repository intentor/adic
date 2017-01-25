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
        /// <summary>The disposable instances.</summary>
        public static List<IDisposable> disposable = new List<IDisposable>();
        /// <summary>The updateable instances.</summary>
        public static List<IUpdatable> updateable = new List<IUpdatable>();
        /// <summary>The late updateable instances.</summary>
        public static List<ILateUpdatable> lateUpdateable = new List<ILateUpdatable>();
        /// <summary>The fixed updateable instances.</summary>
        public static List<IFixedUpdatable> fixedUpdateable = new List<IFixedUpdatable>();
        /// <summary>The focusable instances.</summary>
        public static List<IFocusable> focusable = new List<IFocusable>();
        /// <summary>The pausable instances.</summary>
        public static List<IPausable> pausable = new List<IPausable>();
        /// <summary>The quitable instances.</summary>
        public static List<IQuitable> quitable = new List<IQuitable>();
        /// <summary>The event caller behaviour.</summary>
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
            lateUpdateable.Clear();
            fixedUpdateable.Clear();
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
                if (binding.value is ICommand) {
                    return;
                }

                this.BindUnityExtension(disposable, binding.value);
                this.BindUnityExtension(updateable, binding.value);
                this.BindUnityExtension(lateUpdateable, binding.value);
                this.BindUnityExtension(fixedUpdateable, binding.value);
                this.BindUnityExtension(focusable, binding.value);
                this.BindUnityExtension(pausable, binding.value);
                this.BindUnityExtension(quitable, binding.value);
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
            if (binding.instanceType == BindingInstance.Singleton || instance is ICommand) {
                return;
            }

            this.BindUnityExtension(disposable, instance);
            this.BindUnityExtension(updateable, instance);
            this.BindUnityExtension(lateUpdateable, binding.value);
            this.BindUnityExtension(fixedUpdateable, binding.value);
            this.BindUnityExtension(focusable, instance);
            this.BindUnityExtension(pausable, instance);
            this.BindUnityExtension(quitable, instance);
        }

        /// <summary>
        /// Binds the unity extension.
        /// </summary>
        /// <param name="instances">List of event instances.</param>
        /// <param name="instance">Instance to be bound to.</param>
        /// <typeparam name="T">Type of the instances.</typeparam>
        protected void BindUnityExtension<T>(List<T> instances, object instance) {
            if (instance is T && !instances.Contains((T) instance)) {
                instances.Add((T) instance);
            }
        }
    }
}
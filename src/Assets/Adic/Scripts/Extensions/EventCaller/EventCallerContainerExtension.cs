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
        public List<IDisposable> disposable { get; private set; }

        /// <summary>The updateable instances.</summary>
        public List<IUpdatable> updateable { get; private set; }

        /// <summary>The late updateable instances.</summary>
        public List<ILateUpdatable> lateUpdateable { get; private set; }

        /// <summary>The fixed updateable instances.</summary>
        public List<IFixedUpdatable> fixedUpdateable { get; private set; }

        /// <summary>The focusable instances.</summary>
        public List<IFocusable> focusable { get; private set; }

        /// <summary>The pausable instances.</summary>
        public List<IPausable> pausable { get; private set; }

        /// <summary>The quitable instances.</summary>
        public List<IQuitable> quitable { get; private set; }

        /// <summary>The event caller behaviour.</summary>
        public EventCallerBehaviour behaviour { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.EventCallerContainerExtension"/> class.
        /// </summary>
        public EventCallerContainerExtension() {
            this.disposable = new List<IDisposable>();
            this.updateable = new List<IUpdatable>();
            this.lateUpdateable = new List<ILateUpdatable>();
            this.fixedUpdateable = new List<IFixedUpdatable>();
            this.focusable = new List<IFocusable>();
            this.pausable = new List<IPausable>();
            this.quitable = new List<IQuitable>();
        }

        public void Init(IInjectionContainer container) {
            // Does nothing.
        }

        public void OnRegister(IInjectionContainer container) {
            this.CreateBehaviour(container.identifier);

            // Check whether a binding for the ICommandDispatcher exists.
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

            if (behaviour != null && behaviour.gameObject != null) {
                MonoBehaviour.DestroyImmediate(behaviour.gameObject);
            }
            behaviour = null;

            disposable.Clear();
            updateable.Clear();
            lateUpdateable.Clear();
            fixedUpdateable.Clear();
            focusable.Clear();
            pausable.Clear();
            quitable.Clear();
        }

        /// <summary>
        /// Create the EventCalledBehaviour. At any time there should be a single behaviour on the scene.
        /// </summary>
        /// <param name="containerID">Container ID.</param>
        private void CreateBehaviour(object containerID) {
            if (behaviour == null) {
                // Create a new game object for UpdateableBehaviour.
                var gameObject = new GameObject();
                gameObject.name = String.Format("EventCaller ({0})", containerID);

                // The behaviour should only be removed during unregister.
                MonoBehaviour.DontDestroyOnLoad(gameObject);

                behaviour = gameObject.AddComponent<EventCallerBehaviour>();
                behaviour.extension = this;
            }
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
            if (binding.instanceType == BindingInstance.Singleton || instance is ICommand) {
                return;
            }

            this.BindUnityExtension(disposable, instance);
            this.BindUnityExtension(updateable, instance);
            this.BindUnityExtension(lateUpdateable, instance);
            this.BindUnityExtension(fixedUpdateable, instance);
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
using UnityEngine;
using System;
using System.Collections;
using Adic.Binding;
using Adic.Container;
using Adic.Exceptions;
using Adic.Injection;
using Adic.Util;

namespace Adic {
    /// <summary>
    /// Container extension for the Unity Binding Adic Extension.
    /// 
    /// Ensures that the resolution of transient prefabs instantiates them.
    /// </summary>
    public class UnityBindingContainerExtension : IContainerExtension {
        protected const string CANNOT_RESOLVE_MONOBEHAVIOUR = "A MonoBehaviour cannot be resolved directly.";

        public void Init(IInjectionContainer container) {
            // Does nothing.
        }

        public void OnRegister(IInjectionContainer container) {
            container.beforeAddBinding += this.OnBeforeAddBinding;
            container.bindingEvaluation += this.OnBindingEvaluation;
        }

        public void OnUnregister(IInjectionContainer container) {
            container.beforeAddBinding -= this.OnBeforeAddBinding;
            container.bindingEvaluation -= this.OnBindingEvaluation;
        }

        /// <summary>
        /// Handles the before add binding container event.
        /// 
        /// Used to ensure the binding value is a <see cref="UnityEngine.MonoBehaviour"/>.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="binding">Binding.</param>
        protected void OnBeforeAddBinding(IBinder source, ref BindingInfo binding) {
            if (binding.value is Type &&
                TypeUtils.IsAssignable(typeof(MonoBehaviour), binding.value as Type)) {
                throw new BindingException(CANNOT_RESOLVE_MONOBEHAVIOUR);
            }
        }

        /// <summary>
        /// Handles the binding evaluation container event.
        /// 
        /// Used to instantiate prefabs.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="binding">Binding.</param>
        protected object OnBindingEvaluation(IInjector source, ref BindingInfo binding) {
            // Check whether a prefab should be instantiated.
            if (binding.value is PrefabBinding &&
                binding.instanceType == BindingInstance.Transient) {
                var prefabBinding = (PrefabBinding) binding.value;
                var gameObject = (GameObject) MonoBehaviour.Instantiate(prefabBinding.prefab);

                if (prefabBinding.type.Equals(typeof(GameObject))) {
                    return gameObject;
                } else {
                    var component = gameObject.GetComponent(prefabBinding.type);

                    if (component == null) {
                        component = gameObject.AddComponent(prefabBinding.type);
                    }

                    return component;
                }
            } else {
                return null;
            }
        }
    }
}
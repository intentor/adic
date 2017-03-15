using UnityEngine;
using System;
using Adic.Util;

namespace Adic.Extensions.ContextRoots {
    /// <summary>
    /// Scene injection helper class.
    /// </summary>
    [RequireComponent(typeof(ContextRoot))]
    public class SceneInjector : MonoBehaviour {
        private void Awake() {
            var contextRoot = this.GetComponent<ContextRoot>();
            var baseType = (contextRoot.baseBehaviourTypeName == "UnityEngine.MonoBehaviour" ?
				typeof(MonoBehaviour) : TypeUtils.GetType(contextRoot.baseBehaviourTypeName));

            switch (contextRoot.injectionType) {
                case ContextRoot.MonoBehaviourInjectionType.Children:
                    this.InjectOnChildren(baseType);
                break;

                case ContextRoot.MonoBehaviourInjectionType.BaseType:
                    this.InjectFromBaseType(baseType);
                break;
            }
        }

        /// <summary>
        /// Performs injection on all children of the current GameObject.
        /// </summary>
        /// <param name="baseType">Base type to perform injection.</param>
        public void InjectOnChildren(Type baseType) {
            var sceneInjectorType = this.GetType();
            var components = this.GetComponent<Transform>().GetComponentsInChildren(baseType, true);
            foreach (var component in components) {
                var componentType = component.GetType();
                if (componentType == sceneInjectorType ||
                    TypeUtils.IsAssignable(typeof(ContextRoot), componentType))
                    continue;
				
                ((MonoBehaviour) component).Inject();
            }
        }

        /// <summary>
        /// Performs injection on all behaviours of a given <paramref name="baseType"/>.
        /// </summary>
        /// <param name="baseType">Base type to perform injection.</param>
        public void InjectFromBaseType(Type baseType) {
            var components = (MonoBehaviour[]) Resources.FindObjectsOfTypeAll(baseType);

            for (var index = 0; index < components.Length; index++) {
                components[index].Inject();
            }
        }
    }
}
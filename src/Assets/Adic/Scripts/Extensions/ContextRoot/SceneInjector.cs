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

			switch (contextRoot.injectionType) {
				case ContextRoot.MonoBehaviourInjectionType.Children: {
					this.InjectOnChildren();
				}
				break;

				case ContextRoot.MonoBehaviourInjectionType.BaseType: {
					var baseType = TypeUtils.GetType(contextRoot.baseBehaviourTypeName);
					this.InjectFromBaseType(baseType);
				}
				break;
			}
		}

		/// <summary>
		/// Performs injection on all children of the current GameObject.
		/// </summary>
		public void InjectOnChildren() {
			var sceneInjectorType = this.GetType();
			var components = this.GetComponent<Transform>().GetComponentsInChildren<MonoBehaviour>();
			foreach (var component in components) {
				//If the component is a ContextRoot or this component, ignores injection on it.
				var componentType = component.GetType();
				if (componentType == sceneInjectorType || 
					TypeUtils.IsAssignable(typeof(ContextRoot), componentType)) continue;
				
				component.Inject();
			}
		}

		/// <summary>
		/// Performs injection on all behaviours of a given <paramref name="baseType"/>.
		/// </summary>
		/// <param name="baseType">Base type to perform injection.</param>
		public void InjectFromBaseType(Type baseType) {
			var components = (MonoBehaviour[])Resources.FindObjectsOfTypeAll(baseType);

			for (var index = 0; index < components.Length; index++) {
				components[index].Inject();
			}
		}
	}
}
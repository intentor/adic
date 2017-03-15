using UnityEngine;
using System;
using Adic;
using Adic.Container;

namespace Adic.Extensions.MonoInjection {
	/// <summary>
	/// Injection utils.
	/// </summary>
	public static class InjectionUtil {
		/// <summary>
		/// Injects into a specified object using container details.
		/// </summary>
		/// <param name="obj">Target object of the injection.</param>
		public static void Inject(object obj) {
			var attributes = obj.GetType().GetCustomAttributes(true);
			
			if (attributes.Length == 0) {
				Inject(obj, null);
			} else {
				var containInjectFromContainer = false;
				
				for (var attributeIndex = 0; attributeIndex < attributes.Length; attributeIndex++) {
					var attribute = attributes[attributeIndex];
					
					if (attribute is InjectFromContainer) {
						Inject(obj, (attribute as InjectFromContainer).identifier);
						containInjectFromContainer = true;
					}
				}
				
                // If no InjectFromContainer attribute has been found, does regular injection.
				if (!containInjectFromContainer) {
					Inject(obj, null);
				}
			}
		}
		
		/// <summary>
		/// Does dependency injection on a script from a container with a given identifier.
		/// </summary>
		/// <param name="obj">Target object of the injection.</param>
		/// <param name="identifier">Container identifier. If empty, no container restrictions are applied.</param>
		public static void Inject(object obj, object identifier) {
			var containers = ContextRoot.containersData;
			
			for (int index = 0; index < containers.Count; index++) {
				var container = containers[index].container;
				var injectOnContainer = (container.identifier != null && container.identifier.Equals(identifier));
				
				if ((identifier == null || injectOnContainer) && !IsSingletonOnContainer(obj, container)) {
					container.Inject(obj);
				}
			}
		}
		
		/// <summary>
		/// Determines if the object is a singleton in a given container.
		/// </summary>
		/// <param name="obj">Target object to check.</param>
		/// <param name="container">Container to check for bindings.</param>
		/// <returns><c>true</c> if is singleton on container the specified obj container; otherwise, <c>false</c>.</returns>
		public static bool IsSingletonOnContainer(object obj, IInjectionContainer container) {
			var isSingleton = false;
			var bindings = container.GetBindingsFor(obj.GetType());
			
			if (bindings == null) return false;
			
			for (var index = 0; index < bindings.Count; index++) {
				var binding = bindings[index];
				
				if (binding.value == obj) {
					isSingleton = true;
				}
			}
			
			return isSingleton;
		}
	}
}
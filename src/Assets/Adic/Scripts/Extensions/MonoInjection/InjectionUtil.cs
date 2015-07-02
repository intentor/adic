using UnityEngine;
using System;
using Adic;

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
				
				//If no attribute InjectFromContainer has been found, does regular injection.
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
				
				if (identifier == null || (container.identifier != null && container.identifier.Equals(identifier))) {
					container.Inject(obj);
				}
			}
		}
	}
}
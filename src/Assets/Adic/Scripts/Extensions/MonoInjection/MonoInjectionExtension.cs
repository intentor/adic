using UnityEngine;
using System;
using Adic;

/// <summary>
/// Provides an Inject method to <see cref="UnityEngine.MonoBehaviour"/>.
/// 
/// It assumes that the ContextRoot Extension is in use.
/// </summary>
/// <remarks>
/// DEPENDENCIES
/// 
/// - MonoInjection Extension
/// </remarks>
public static class MonoInjectionExtension {
	/// <summary>
	/// Does dependency injection on a MonoBehaviour.
	/// </summary>
	/// <param name="script">Target script of the injection.</param>
	public static void Inject(this MonoBehaviour script) {
		var attributes = script.GetType().GetCustomAttributes(true);

		if (attributes.Length == 0) {
			Inject(script, string.Empty);
		} else {
			for (var attributeIndex = 0; attributeIndex < attributes.Length; attributeIndex++) {
				var attribute = attributes[attributeIndex];

				if (attribute is InjectFromContainer) {
					Inject(script, (attribute as InjectFromContainer).identifier);
				}
			}
		}
	}

	/// <summary>
	/// Does dependency injection on a MonoBehaviour from a container with a given identifier.
	/// </summary>
	/// <param name="script">Target script of the injection.</param>
	/// <param name="identifier">Container identifier. If empty, no container restrictions are applied.</param>
	private static void Inject(MonoBehaviour script, string identifier) {
		var containers = ContextRoot.containersData;

		for (int index = 0; index < containers.Count; index++) {
			var container = containers[index].container;

			if (string.IsNullOrEmpty(identifier) || container.identifier == identifier) {
				container.Inject(script);
			}
		}
	}
}
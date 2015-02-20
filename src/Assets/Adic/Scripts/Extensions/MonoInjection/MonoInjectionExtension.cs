using UnityEngine;
using System;
using Intentor.Adic;

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
		var containers = ContextRoot.containersData;

		for (int index = 0; index < containers.Count; index++) {
			containers[index].container.Inject(script);
		}
	}
}
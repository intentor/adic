using UnityEngine;
using System;
using Adic;

/// <summary>
/// MonoBehaviour inject extensions.
/// </summary>
public static class MonoInjection {
	/// <summary>
	/// Does dependency injection on a MonoBehaviour.
	/// </summary>
	/// <param name="script">Target script of the injection.</param>
	public static void Inject(this MonoBehaviour script) {
		var contexts = ContextRoot.instance.contexts;

		for (int i = 0; i < contexts.Length; i++) {
			contexts[i].Inject(script);
		}
	}		
}
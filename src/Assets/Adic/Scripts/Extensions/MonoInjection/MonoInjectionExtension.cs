using UnityEngine;
using Adic;
using Adic.Extensions.MonoInjection;

/// <summary>
/// Provides an Inject method to <see cref="UnityEngine.MonoBehaviour"/>.
/// 
/// It assumes that the ContextRoot Extension is in use.
/// </summary>
public static class MonoInjectionExtension {
    /// <summary>
    /// Does dependency injection on a MonoBehaviour.
    /// </summary>
    /// <param name="script">Target script of the injection.</param>
    public static void Inject(this MonoBehaviour script) {
        InjectionUtil.Inject(script);
    }
}
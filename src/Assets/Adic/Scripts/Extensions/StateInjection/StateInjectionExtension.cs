using UnityEngine;
using Adic;
using Adic.Extensions.MonoInjection;

#if UNITY_5

/// <summary>
/// Provides an Inject method to <see cref="UnityEngine.StateMachineBehaviour"/>.
/// 
/// It assumes that the ContextRoot Extension is in use.
/// </summary>
/// <remarks>
/// DEPENDENCIES
/// 
/// - MonoInjection Extension
/// </remarks>
public static class StateInjectionExtension {
    /// <summary>
    /// Does dependency injection on a StateMachineBehaviour.
    /// </summary>
    /// <param name="script">Target script of the injection.</param>
    public static void Inject(this StateMachineBehaviour script) {
        InjectionUtil.Inject(script);
    }
}

#endif
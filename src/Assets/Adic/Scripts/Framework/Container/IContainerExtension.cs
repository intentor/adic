using System;

namespace Adic.Container {
    /// <summary>
    /// Defines an extension for an <see cref="Adic.InjectionContainer"/>.
    /// </summary>
    public interface IContainerExtension {
        /// <summary>
        /// Initializes the extension. Called after all extensions have been registered.
        /// </summary>
        /// <param name="container">The container into which the extension is registered.</param>
        void Init(IInjectionContainer container);

        /// <summary>
        /// Called when the extension is registered on the container.
        /// 
        /// When the method is called, subscribe to any events the container may provide.
        /// </summary>
        /// <param name="container">The container into which the extension is being registered.</param>
        void OnRegister(IInjectionContainer container);

        /// <summary>
        /// Called when the extension is unregistered from the container.
        /// 
        /// When the method is called, unsubscribe from any events on the container.
        /// </summary>
        /// <param name="container">The container from which the extension is being unregistered.</param>
        void OnUnregister(IInjectionContainer container);
    }
}
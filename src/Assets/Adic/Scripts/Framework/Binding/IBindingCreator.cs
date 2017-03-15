using System;

namespace Adic.Binding {
    /// <summary>
    /// Defines a bindind creator.
    /// </summary>
    public interface IBindingCreator {
        /// <summary>
        /// Binds a type to another type or instance.
        /// </summary>
        /// <typeparam name="T">The type to bind to.</typeparam>
        /// <returns>The binding.</returns>
        IBindingFactory Bind<T>();

        /// <summary>
        /// Binds a type to another type or instance.
        /// </summary>
        /// <param name="type">The type to bind to.</param>
        /// <returns>The binding.</returns>
        IBindingFactory Bind(Type type);
    }
}
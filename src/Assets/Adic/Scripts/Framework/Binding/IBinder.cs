using System;
using System.Collections.Generic;

namespace Adic.Binding {
    /// <summary>
    /// Defines a binder that binds a type to another type or instance.
    /// </summary>
    public interface IBinder : IBindingCreator {
        /// <summary>Occurs before a binding is added.</summary>
        event BindingAddedHandler beforeAddBinding;
        /// <summary>Occurs after a binding is added.</summary>
        event BindingAddedHandler afterAddBinding;
        /// <summary>Occurs before a binding is removed.</summary>
        event BindingRemovedHandler beforeRemoveBinding;
        /// <summary>Occurs after a binding is removed.</summary>
        event BindingRemovedHandler afterRemoveBinding;

        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="binding">The binding to be added.</param>
        void AddBinding(BindingInfo binding);

        /// <summary>
        /// Gets all bindings.
        /// </summary>
        /// <returns>Bindings list.</returns>
        IList<BindingInfo> GetBindings();

        /// <summary>
        /// Gets the bindings for a given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the bindings from.</typeparam>
        /// <returns>The bindings for the desired type.</returns>
        IList<BindingInfo> GetBindingsFor<T>();

        /// <summary>
        /// Gets the bindings for a given <param name="type">.
        /// </summary>
        /// <param name="type">The type to get the bindings from.</param>
        /// <returns>The bindings for the desired type.</returns>
        IList<BindingInfo> GetBindingsFor(Type type);

        /// <summary>
        /// Gets the bindings for a given <param name="identifier">.
        /// </summary>
        /// <param name="identifier">The identifier to get the bindings from.</param>
        /// <returns>The bindings for the desired type.</returns>
        IList<BindingInfo> GetBindingsFor(object identifier);

        /// <summary>
        /// Gets the bindings to a given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the bindings from.</typeparam>
        /// <returns>The bindings to the desired type.</returns>
        IList<BindingInfo> GetBindingsTo<T>();

        /// <summary>
        /// Gets the bindings to a given <param name="type">.
        /// </summary>
        /// <param name="type">The type to get the bindings from.</param>
        /// <returns>The bindings to the desired type.</returns>
        IList<BindingInfo> GetBindingsTo(Type type);

        /// <summary>
        /// Checks whether this binder contains a binding for a given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="type">The type to be checked.</typeparam>
        /// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
        bool ContainsBindingFor<T>();

        /// <summary>
        /// Checks whether this binder contains a binding for a given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
        bool ContainsBindingFor(Type type);

        /// <summary>
        /// Checks whether this binder contains a binding for a given <paramref name="identifier"/>.
        /// </summary>
        /// <param name="type">The identifier to be checked.</param>
        /// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
        bool ContainsBindingFor(object identifier);

        /// <summary>
        /// Unbinds any bindings to a certain <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to be unbound.</typeparam>
        void Unbind<T>();

        /// <summary>
        /// Unbinds any bindings to a certain <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to be unbound.</param>
        void Unbind(Type type);

        /// <summary>
        /// Unbinds any bindings to a certain <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier">The identifier to be unbound.</param>
        void Unbind(object identifier);

        /// <summary>
        /// Unbinds any bindings that holds the given instance, either as a value or on conditions.
        /// </summary>
        /// <param name="instance">Instance.</param>
        void UnbindInstance(object instance);

        /// <summary>
        /// Unbinds any bindings that contains the given tag.
        /// </summary>
        /// <param name="tag">Tag value.</param>
        void UnbindByTag(string tag);
    }
}
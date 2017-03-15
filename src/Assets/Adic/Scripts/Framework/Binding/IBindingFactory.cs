using System;

namespace Adic.Binding {
    /// <summary>
    /// Defines a binding factory.
    /// 
    /// The binding factory also provides binding capabilities for chaining.
    /// </summary>
    public interface IBindingFactory {
        /// <summary>Binder used by the Binding Factory.</summary>
        IBinder binder { get; }

        /// <summary>The type being bound.</summary>
        Type bindingType { get; }

        /// <summary>
        /// Binds the key type to a transient of itself. The key must be a class. 
        /// </summary>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToSelf();

        /// <summary>
        /// Binds the key type to a singleton of itself. The key must be a class.
        /// </summary>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToSingleton();

        /// <summary>
        /// Binds the key type to a singleton of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The related type.</typeparam>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToSingleton<T>() where T : class;

        /// <summary>
        /// Binds the key type to a singleton of type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The related type.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToSingleton(Type type);

        /// <summary>
        /// Binds the key type to a transient of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to bind to.</typeparam>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory To<T>() where T : class;

        /// <summary>
        /// Binds the key type to a transient of type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The related type.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory To(Type type);

        /// <summary>
        /// Binds the key type to an <paramref name="instance"/>.
        /// </summary>
        /// <typeparam name="T">The related type.</typeparam>
        /// <param name="instance">The instance to bind the type to.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory To<T>(T instance);

        /// <summary>
        /// Binds the key type to an <paramref name="instance"/>.
        /// </summary>
        /// <param name="type">The related type.</param>
        /// <param name="instance">The instance to bind the type to.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory To(Type type, object instance);

        /// <summary>
        /// Binds the key type to all assignable types in a given <paramref name="namespaceName"/> 
        /// as transient bindings.
        /// </summary>
        /// <param name="namespaceName">Namespace name.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToNamespace(string namespaceName);

        /// <summary>
        /// Binds the key type to all assignable types in a given <paramref name="namespaceName"/> 
        /// as transient bindings.
        /// </summary>
        /// <param name="namespaceName">Namespace name.</param>
        /// <param name="includeChildren">Indicates whether child namespaces should be included.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToNamespace(string namespaceName, bool includeChildren);

        /// <summary>
        /// Binds the key to all assignable types in a given <paramref name="namespaceName"/>
        /// as singleton bindings.
        /// </summary>
        /// <param name="namespaceName">Namespace name.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToNamespaceSingleton(string namespaceName);

        /// <summary>
        /// Binds the key to all assignable types in a given <paramref name="namespaceName"/>
        /// as singleton bindings.
        /// </summary>
        /// <param name="namespaceName">Namespace name.</param>
        /// <param name="includeChildren">Indicates whether child namespaces should be included.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToNamespaceSingleton(string namespaceName, bool includeChildren);

        /// <summary>
        /// Binds the key type to a <typeparamref name="T"/> factory.
        /// </summary>
        /// <typeparam name="T">The factory type.</typeparam>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToFactory<T>() where T : IFactory;

        /// <summary>
        /// Binds the key type to a factory of a certain <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The factory type.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToFactory(Type type);

        /// <summary>
        /// Binds the key type to a <paramref name="factory"/>.
        /// </summary>
        /// <param name="factory">Factory to be bound to.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        IBindingConditionFactory ToFactory(IFactory factory);

        /// <summary>
        /// Creates a binding.
        /// </summary>
        /// <returns>The binding.</returns>
        /// <param name="value">Binding value.</param>
        /// <param name="instanceType">Binding instance type.</param>
        IBindingConditionFactory AddBinding(object value, BindingInstance instanceType);
    }
}
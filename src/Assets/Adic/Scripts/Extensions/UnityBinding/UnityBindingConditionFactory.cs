using UnityEngine;
using System;
using Adic.Binding;

namespace Adic {
    /// <summary>
    /// Binding condition factory for Unity bindings. It uses the original binding and adds another actions to it.
    /// </summary>
    public class UnityBindingConditionFactory : IBindingConditionFactory {
        /// <summary>Original binding condition factory.</summary>
        private IBindingConditionFactory bindingConditionFactory;
        /// <summary>Related Unity Object name.</summary>
        private string objectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.UnityBindingConditionFactory"/> class.
        /// </summary>
        /// <param name="bindingConditionFactory">Original binding condition factory.</param>
        /// <param name="objectName">Related Unity Object name.</param>
        public UnityBindingConditionFactory(IBindingConditionFactory bindingConditionFactory, string objectName) {
            this.bindingConditionFactory = bindingConditionFactory;
            this.objectName = objectName;
        }

        /// <summary>
        /// Binds a type to another type or instance.
        /// </summary>
        /// <typeparam name="T">The type to bind to.</typeparam>
        /// <returns>The binding.</returns>
        public IBindingFactory Bind<T>() {
            return this.bindingConditionFactory.Bind<T>();
        }

        /// <summary>
        /// Binds a type to another type or instance.
        /// </summary>
        /// <param name="type">The type to bind to.</param>
        /// <returns>The binding.</returns>
        public IBindingFactory Bind(Type type) {
            return this.bindingConditionFactory.Bind(type);
        }

        //// <summary>
        /// Conditions the binding to be injected through an identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the binding.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory As(object identifier) {
            return this.bindingConditionFactory.As(identifier);
        }

        //// <summary>
        /// Conditions the binding to be injected through an identifier defined by the name of the related Unity Object.
        /// </summary>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory AsObjectName() {
            return this.bindingConditionFactory.As(this.objectName);
        }

        /// <summary>
        /// Conditions the binding to be injected only if BindingConditionEvaluator returns true.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory When(BindingCondition condition) {
            return this.bindingConditionFactory.When(condition);
        }

        /// <summary>
        /// Conditions the binding to be injected only when into an object of a certain type <typeparamref name="T">.
        /// </summary>
        /// <typeparam name="T">The enclosing type.</typeparam>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory WhenInto<T>() {
            return this.bindingConditionFactory.WhenInto<T>();
        }

        /// <summary>
        /// Conditions the binding to be injected only when into an object of a certain type <paramref name="type"/>.
        /// </summary>
        /// </summary>
        /// <param name="type">The enclosing type.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory WhenInto(Type type) {
            return this.bindingConditionFactory.WhenInto(type);
        }

        /// <summary>
        /// Conditions the binding to be injected only when into a certain <paramref name="instance"/>.
        /// </summary>
        /// </summary>
        /// <param name="instance">The enclosing instance.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory WhenIntoInstance(object instance) {
            return this.bindingConditionFactory.WhenIntoInstance(instance);
        }

        //// <summary>
        /// Adds tags to the binding.
        /// </summary>
        /// <param name="tags">Tags of the binding.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory Tag(params string[] tags) {
            return this.bindingConditionFactory.Tag(tags);
        }
    }
}


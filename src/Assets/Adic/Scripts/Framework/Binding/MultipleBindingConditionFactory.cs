using System;

namespace Adic.Binding {
    /// <summary>
    /// Binding condition factory for multiple binding factories.
    /// </summary>
    public class MultipleBindingConditionFactory : IBindingConditionFactory {
        /// <summary>Binding factories.</summary>
        protected IBindingConditionFactory[] bindingConditionFactories;
        /// <summary>Binding creator. Used for chaining.</summary>
        protected IBindingCreator bindindCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Binding.MultipleBindingConditionFactory"/> class.
        /// </summary>
        /// <param name="bindingConditionFactories">Binding factories.</param>
        public MultipleBindingConditionFactory(
            IBindingConditionFactory[] bindingConditionFactories, IBindingCreator bindindCreator) {
            this.bindingConditionFactories = bindingConditionFactories;
            this.bindindCreator = bindindCreator;
        }

        /// <summary>
        /// Conditions the binding to be injected through an identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the binding.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory As(object identifier) {
            for (var index = 0; index < this.bindingConditionFactories.Length; index++) {
                this.bindingConditionFactories[index].As(identifier);
            }

            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only if BindingConditionEvaluator returns true.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory When(BindingCondition condition) {
            for (var index = 0; index < this.bindingConditionFactories.Length; index++) {
                this.bindingConditionFactories[index].When(condition);
            }

            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only when into an object of a certain type <typeparamref name="T">.
        /// </summary>
        /// <typeparam name="T">The enclosing type.</typeparam>
        /// <returns>The current binding condition.</returns>		
        public IBindingConditionFactory WhenInto<T>() {
            for (var index = 0; index < this.bindingConditionFactories.Length; index++) {
                this.bindingConditionFactories[index].WhenInto<T>();
            }
			
            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only when into an object of a certain type <paramref name="type"/>.
        /// </summary>
        /// </summary>
        /// <param name="type">The enclosing type.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory WhenInto(Type type) {
            for (var index = 0; index < this.bindingConditionFactories.Length; index++) {
                this.bindingConditionFactories[index].WhenInto(type);
            }
			
            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only when into a certain <paramref name="instance"/>.
        /// </summary>
        /// </summary>
        /// <param name="instance">The enclosing instance.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory WhenIntoInstance(object instance) {
            for (var index = 0; index < this.bindingConditionFactories.Length; index++) {
                this.bindingConditionFactories[index].WhenIntoInstance(instance);
            }
			
            return this;
        }

        /// <summary>
        /// Adds tags to the binding.
        /// </summary>
        /// <param name="tags">Tags of the binding.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory Tag(params string[] tags) {
            for (var index = 0; index < this.bindingConditionFactories.Length; index++) {
                this.bindingConditionFactories[index].Tag(tags);
            }

            return this;
        }

        /// <summary>
        /// Binds a type to another type or instance. Used for chaining.
        /// </summary>
        /// <typeparam name="T">The type to bind to.</typeparam>
        /// <returns>The binding.</returns>
        public IBindingFactory Bind<T>() {
            return this.bindindCreator.Bind<T>();
        }

        /// <summary>
        /// Binds a type to another type or instance. Used for chaining.
        /// </summary>
        /// <param name="type">The type to bind to.</param>
        /// <returns>The binding.</returns>
        public IBindingFactory Bind(Type type) {
            return this.bindindCreator.Bind(type);
        }
    }
}
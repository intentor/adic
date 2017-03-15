using System;

namespace Adic.Binding {
    /// <summary>
    /// Binding condition factory for a single binding.
    /// </summary>
    public class SingleBindingConditionFactory : IBindingConditionFactory {
        /// <summary>Binding to have its conditions defined.</summary>
        protected BindingInfo binding;
        /// <summary>Binding creator. Used for chaining.</summary>
        protected IBindingCreator bindindCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Binding.SingleBindingConditionFactory"/> class.
        /// </summary>
        /// <param name="binding">The binding to have its conditions settled.</param>
        /// <param name="bindindCreator">Binding creator. Used for chaining.</param>
        public SingleBindingConditionFactory(BindingInfo binding, IBindingCreator bindindCreator) {
            this.binding = binding;
            this.bindindCreator = bindindCreator;
        }

        /// <summary>
        /// Conditions the binding to be injected through an identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the binding.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory As(object identifier) {
            this.binding.identifier = identifier;

            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only if BindingConditionEvaluator returns true.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory When(BindingCondition condition) {
            this.binding.condition = condition;

            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only when into an object of a certain type <typeparamref name="T">.
        /// </summary>
        /// <typeparam name="T">The enclosing type.</typeparam>
        /// <returns>The current binding condition.</returns>		
        public IBindingConditionFactory WhenInto<T>() {
            this.binding.condition = context => context.parentType == typeof(T);
			
            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only when into an object of a certain type <paramref name="type"/>.
        /// </summary>
        /// </summary>
        /// <param name="type">The enclosing type.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory WhenInto(Type type) {
            this.binding.condition = context => context.parentType == type;
			
            return this;
        }

        /// <summary>
        /// Conditions the binding to be injected only when into a certain <paramref name="instance"/>.
        /// </summary>
        /// </summary>
        /// <param name="instance">The enclosing instance.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory WhenIntoInstance(object instance) {
            this.binding.condition = context => context.parentInstance == instance;
			
            return this;
        }

        /// <summary>
        /// Adds tags to the binding.
        /// <para />
        /// Subsequent calls to the Tag method will replace old tags.
        /// </summary>
        /// <param name="tags">Tags of the binding.</param>
        /// <returns>The current binding condition.</returns>
        public IBindingConditionFactory Tag(params string[] tags) {
            this.binding.tags = tags;

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
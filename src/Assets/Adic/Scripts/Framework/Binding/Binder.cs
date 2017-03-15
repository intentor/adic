using System;
using System.Collections.Generic;
using Adic.Injection;
using Adic.Exceptions;

namespace Adic.Binding {
    /// <summary>
    /// Binds a type to another type or an instance.
    /// </summary>
    public class Binder : IBinder {
        /// <summary>Occurs before a binding is added.</summary>
        public event BindingAddedHandler beforeAddBinding;
        /// <summary>Occurs after a binding is added.</summary>
        public event BindingAddedHandler afterAddBinding;
        /// <summary>Occurs before a binding is removed.</summary>
        public event BindingRemovedHandler beforeRemoveBinding;
        /// <summary>Occurs after a binding is removed.</summary>
        public event BindingRemovedHandler afterRemoveBinding;

        /// <summary>
        /// Checks whether a binding can be removed.
        /// </summary>
        /// <param name="binding">Binding to be evaluated.</param>
        public delegate bool CanRemoveBindingHandler(BindingInfo binding);

        /// <summary>Type bindings of the binder.</summary>
        protected Dictionary<Type, IList<BindingInfo>> typeBindings = new Dictionary<Type, IList<BindingInfo>>();

        /// <summary>
        /// Binds a type to another type or instance.
        /// </summary>
        /// <typeparam name="T">The type to bind to.</typeparam>
        /// <returns>The binding.</returns>
        public IBindingFactory Bind<T>() {
            return this.Bind(typeof(T));
        }

        /// <summary>
        /// Binds a type to another type or instance.
        /// </summary>
        /// <param name="type">The type to bind to.</param>
        /// <returns>The binding.</returns>
        public IBindingFactory Bind(Type type) {
            return this.BindingFactoryProvider(type);
        }

        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="binding">The binding to be added.</param>
        public void AddBinding(BindingInfo binding) {
            if (binding == null) {
                throw new BinderException(BinderException.NULL_BINDING);
            } else if (binding.value is Type && (binding.value as Type).IsInterface) {
                throw new BinderException(BinderException.BINDING_TO_INTERFACE);
            }

            //If binding to singleton from other type, binds the singleton first.
            var valueType = binding.GetValueType();
            if (!valueType.Equals(typeof(InjectionContainer))) {
                var isSingleton = (binding.instanceType == BindingInstance.Singleton);
                var isBindingToOtherType = !binding.type.Equals(valueType);
                var valueTypeBound = this.typeBindings.ContainsKey(valueType);
                if (isSingleton && isBindingToOtherType && !valueTypeBound) {
                    this.AddBindingToDictionary(new BindingInfo(valueType, binding.value, BindingInstance.Singleton, binding));
                }
            }

            this.AddBindingToDictionary(binding);
        }

        /// <summary>
        /// Adds the binding to the internal dictionary.
        /// </summary>
        /// <param name="binding">The binding to be added.</param>
        protected void AddBindingToDictionary(BindingInfo binding) {
            if (this.beforeAddBinding != null) {
                this.beforeAddBinding(this, ref binding);
            }

            if (this.typeBindings.ContainsKey(binding.type)) {
                this.typeBindings[binding.type].Add(binding);
            } else {
                var bindingList = new List<BindingInfo>(1);
                bindingList.Add(binding);
                this.typeBindings.Add(binding.type, bindingList);
            }

            if (this.afterAddBinding != null) {
                this.afterAddBinding(this, ref binding);
            }
        }

        /// <summary>
        /// Gets all bindings.
        /// </summary>
        /// <returns>Bindings list.</returns>
        public IList<BindingInfo> GetBindings() {
            var bindings = new List<BindingInfo>();
			
            foreach (var binding in this.typeBindings) {
                bindings.AddRange(binding.Value);
            }
			
            return bindings;
        }

        /// <summary>
        /// Gets the bindings for a certain <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the bindings from.</typeparam>
        /// <returns>The bindings for the desired type.</returns>
        public IList<BindingInfo> GetBindingsFor<T>() {
            return this.GetBindingsFor(typeof(T));
        }

        /// <summary>
        /// Gets the bindings for a certain <param name="type">.
        /// </summary>
        /// <param name="type">The type to get the bindings from.</param>
        /// <returns>The bindings for the desired type.</returns>
        public IList<BindingInfo> GetBindingsFor(Type type) {
            if (this.typeBindings.ContainsKey(type)) {
                return this.typeBindings[type];
            } else {
                return null;
            }
        }

        /// <summary>
        /// Gets the bindings for a given <param name="identifier">.
        /// </summary>
        /// <param name="identifier">The identifier to get the bindings from.</param>
        /// <returns>The bindings for the desired type.</returns>
        public IList<BindingInfo> GetBindingsFor(object identifier) {
            var bindings = new List<BindingInfo>();
			
            foreach (var entry in this.typeBindings) {
                for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
                    var binding = entry.Value[bindingIndex];
					
                    if (binding.identifier != null && binding.identifier.Equals(identifier)) {
                        bindings.Add(binding);
                    }
                }
            }
			
            return bindings;
        }

        /// <summary>
        /// Gets the bindings to a given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the bindings from.</typeparam>
        /// <returns>The bindings to the desired type.</returns>
        public IList<BindingInfo> GetBindingsTo<T>() {
            return this.GetBindingsTo(typeof(T));
        }

        /// <summary>
        /// Gets the bindings to a given <param name="type">.
        /// </summary>
        /// <param name="type">The type to get the bindings from.</param>
        /// <returns>The bindings to the desired type.</returns>
        public IList<BindingInfo> GetBindingsTo(Type type) {
            IList<BindingInfo> bindings = new List<BindingInfo>();

            foreach (var entry in this.typeBindings) {
                for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
                    var binding = entry.Value[bindingIndex];
                    if (binding.GetValueType().Equals(type)) {
                        bindings.Add(binding);
                    }
                }
            }

            return (bindings.Count == 0 ? null : bindings);
        }

        /// <summary>
        /// Checks whether this binder contains a binding for a given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="type">The type to be checked.</typeparam>
        /// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
        public bool ContainsBindingFor<T>() {
            return this.ContainsBindingFor(typeof(T));
        }

        /// <summary>
        /// Checks whether this binder contains a binding for a given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
        public bool ContainsBindingFor(Type type) {
            return this.typeBindings.ContainsKey(type);
        }

        /// <summary>
        /// Checks whether this binder contains a binding for a given <paramref name="identifier"/>.
        /// </summary>
        /// <param name="type">The identifier to be checked.</param>
        /// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
        public bool ContainsBindingFor(object identifier) {
            var contains = false;

            foreach (var entry in this.typeBindings) {
                for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
                    var id = entry.Value[bindingIndex].identifier;

                    if (id != null && id.Equals(identifier)) {
                        contains = true;
                        break;
                    }
                }

                if (contains) {
                    break;
                }
            }

            return contains;
        }

        /// <summary>
        /// Unbinds any bindings to a certain <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to be unbound.</typeparam>
        public void Unbind<T>() {
            this.Unbind(typeof(T));
        }

        /// <summary>
        /// Unbinds any bindings to a certain <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to be unbound.</param>
        public void Unbind(Type type) {
            if (!this.ContainsBindingFor(type))
                return;

            IList<BindingInfo> bindings = new List<BindingInfo>();
            IList<Type> keys = new List<Type>();

            foreach (var entry in this.typeBindings) {
                for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
                    var binding = entry.Value[bindingIndex];
                    if (binding.type.Equals(type) || binding.GetValueType().Equals(type)) {
                        bindings.Add(binding);
                        keys.Add(entry.Key);
                    }
                }
            }

            if (this.beforeRemoveBinding != null) {
                this.beforeRemoveBinding(this, type, bindings);
            }

            foreach (var key in keys) {
                this.typeBindings.Remove(key);
            }

            if (this.afterRemoveBinding != null) {
                this.afterRemoveBinding(this, type, bindings);
            }
        }

        /// <summary>
        /// Unbinds any bindings to a certain <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier">The identifier to be unbound.</param>
        public void Unbind(object identifier) {
            this.Unbind(binding => binding.identifier != null && binding.identifier.Equals(identifier));  
        }

        /// <summary>
        /// Unbinds any bindings that holds the given instance, either as a value or on conditions.
        /// </summary>
        /// <param name="instance">Instance.</param>
        public void UnbindInstance(object instance) {
            this.Unbind(binding => binding.value == instance
                || (binding.condition != null && binding.condition(new InjectionContext() { parentInstance = instance })));
        }

        /// <summary>
        /// Unbinds any bindings that contains the given tag.
        /// </summary>
        /// <param name="tag">Tag value.</param>
        public void UnbindByTag(string tag) {
            if (!string.IsNullOrEmpty(tag)) {
                this.Unbind(binding => binding.tags != null
                    && Array.Exists(binding.tags, element => element != null && element.Equals(tag)));
            }
        }

        /// <summary>
        /// Unbinds bindings using a given condition.
        /// </summary>
        /// <param name="canRemoveBinding">Condition to check for bindings removal.</param>
        protected void Unbind(CanRemoveBindingHandler canRemoveBinding) {
            var bindingsToRemove = new List<BindingInfo>();

            foreach (var entry in this.typeBindings) {
                for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
                    var binding = entry.Value[bindingIndex];
                    bindingsToRemove.Clear();

                    if (canRemoveBinding(binding)) {                  
                        bindingsToRemove.Add(binding);

                        if (this.beforeRemoveBinding != null) {
                            this.beforeRemoveBinding(this, binding.type, bindingsToRemove);
                        }

                        entry.Value.RemoveAt(bindingIndex--);                       

                        if (this.afterRemoveBinding != null) {
                            this.afterRemoveBinding(this, binding.type, bindingsToRemove);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolves the binding provider.
        /// </summary>
        /// <param name="type">The type being bound.</param>
        /// <returns>The binding provider.</returns>
        protected virtual IBindingFactory BindingFactoryProvider(Type type) {
            return new BindingFactory(type, this);
        }
    }
}
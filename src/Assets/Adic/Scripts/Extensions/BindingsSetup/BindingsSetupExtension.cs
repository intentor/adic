using System;
using System.Linq;
using Adic.Container;
using Adic.Util;

namespace Adic {
    /// <summary>
    /// Provides bindings setup capabilities to <see cref="Adic.Container.IInjectionContainer"/>.
    /// </summary>
    public static class BindingsSetupExtension {
        /// <summary>
        /// Represents a prioritized binding setup.
        /// 
        /// Used for sorting priorities.
        /// </summary>
        private class PrioritizedBindingSetup {
            /// <summary>The binding setup.</summary>
            public IBindingsSetup setup;
            /// <summary>The given priority setup.</summary>
            public int priority;
        }

        /// <summary>
        /// Setups bindings in the container.
        /// </summary>
        /// <typeparam name="T">The bindings setup object type.</typeparam>
        /// <param name="container">Container in which the bindings will be setup.</param>
        /// <param name="setup">The bindings setup.</param>
        /// <returns>The injection container for chaining.</returns>
        public static IInjectionContainer SetupBindings<T>(this IInjectionContainer container) where T : IBindingsSetup, new() {
            container.SetupBindings(typeof(T));
			
            return container;
        }

        /// <summary>
        /// Setups bindings in the container.
        /// </summary>
        /// <param name="container">Container in which the bindings will be setup.</param>
        /// <param name="type">The bindings setup object type.</param>
        /// <returns>The injection container for chaining.</returns>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container, Type type) {
            var setup = container.Resolve(type);
            container.SetupBindings((IBindingsSetup) setup);
			
            return container;
        }

        /// <summary>
        /// Setups bindings in the container.
        /// </summary>
        /// <param name="container">Container in which the bindings will be setup.</param>
        /// <param name="setup">The bindings setup.</param>
        /// <returns>The injection container for chaining.</returns>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container, IBindingsSetup setup) {
            setup.SetupBindings(container);
			
            return container;
        }

		
        /// <summary>
        /// Setups bindings in the container from a given namespace and its children namespaces.
        /// </summary>
        /// <param name="container">Container in which the bindings will be setup.</param>
        /// <param name="namespaceName">Namespace name.</param>
        /// <param name="setup">The bindings setup.</param>
        /// <returns>The injection container for chaining.</returns>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container, string namespaceName) {
            container.SetupBindings(namespaceName, true);
			
            return container;
        }

        /// <summary>
        /// Setups bindings in the container from a given <paramref name="namespace"/>.
        /// </summary>
        /// <param name="container">Container in which the bindings will be setup.</param>
        /// <param name="namespaceName">Namespace name.</param>
        /// <param name="includeChildren">Indicates whether child namespaces should be included.</param>
        /// <param name="setup">The bindings setup.</param>
        /// <returns>The injection container for chaining.</returns>
        public static IInjectionContainer SetupBindings(this IInjectionContainer container,
                                                  string namespaceName,
                                                  bool includeChildren) {
            var setups = TypeUtils.GetAssignableTypes(
                    typeof(IBindingsSetup), namespaceName, includeChildren);
            var prioritizedSetups = new PrioritizedBindingSetup[setups.Length];
			
            // Add setups to "priority" with priority definitions.
            for (var setupIndex = 0; setupIndex < setups.Length; setupIndex++) {
                var setup = (IBindingsSetup) container.Resolve(setups[setupIndex]);
                var attributes = setup.GetType().GetCustomAttributes(typeof(BindingPriority), true);
				
                if (attributes.Length > 0) {
                    var bindindPriority = attributes[0] as BindingPriority;
                    prioritizedSetups[setupIndex] = new PrioritizedBindingSetup() {
                        setup = setup,
                        priority = bindindPriority.priority
                    };
                } else {
                    // If the binding has no priority, saves it with priority 0.
                    prioritizedSetups[setupIndex] = new PrioritizedBindingSetup() {
                        setup = setup,
                        priority = 0
                    };
                }
            }
			
            // Order the priority list and executes the setups.
            prioritizedSetups = prioritizedSetups.OrderByDescending(setup => setup.priority).ToArray();
            for (var setupIndex = 0; setupIndex < prioritizedSetups.Length; setupIndex++) {
                prioritizedSetups[setupIndex].setup.SetupBindings(container);
            }

            return container;
        }
    }
}
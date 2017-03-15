using System;

namespace Adic {
    /// <summary>
    /// Indicates that a certaing <see cref="Adic.IBindingsSetup"/> class has priority when used from
    /// SetupBindings() with namespace.
    /// </summary>
    /// <remarks>
    /// It's possible to pass a number to indicate the priority. Higher values indicates higher priorities.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = false,
        Inherited = true)]
    public class BindingPriority : Attribute {
        /// <summary>Binding priority.</summary>
        public int priority;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.BindingPriority"/> class.
        /// </summary>
        public BindingPriority() {
            this.priority = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.BindingPriority"/> class.
        /// </summary>
        /// <param name="priority">inding priority.</param>
        public BindingPriority(int priority) {
            this.priority = priority;
        }
    }
}
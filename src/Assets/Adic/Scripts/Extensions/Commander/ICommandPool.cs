using System;

namespace Adic {
    /// <summary>
    /// Defines a command pool.
    /// </summary>
    public interface ICommandPool {
        /// <summary>
        /// Adds a command of type <paramref name="type"/>.
        /// 
        /// Commands are always added for late registration and pooling.
        /// </summary>
        /// <param name="type">The type of the command to be added.</param>
        void AddCommand(Type type);

        /// <summary>
        /// Pools a command of a given type that was already added.
        /// </summary>
        /// <param name="commandType">Command type.</param>
        void PoolCommand(Type commandType);

        /// <summary>
        /// Gets a command from the pool.
        /// </summary>
        /// <param name="commandType">Command type.</param>
        /// <param name="pool">Pool from which the command will be taken.</param>
        /// <returns>Command or NULL.</returns>
        ICommand GetCommandFromPool(Type commandType);
    }
}
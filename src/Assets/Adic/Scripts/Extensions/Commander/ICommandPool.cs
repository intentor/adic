using System;

namespace Adic {
	/// <summary>
	/// Defines a command pool.
	/// </summary>
	public interface ICommandPool {
		/// <summary>
		/// Pools a command of a given type.
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
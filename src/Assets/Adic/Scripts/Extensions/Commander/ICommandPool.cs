using System;

namespace Adic {
	/// <summary>
	/// Defines a command pool.
	/// </summary>
	public interface ICommandPool {
		/// <summary>
		/// Pools all commands.
		/// </summary>
		void Pool();

		/// <summary>
		/// Gets a command from the pool.
		/// </summary>
		/// <param name="commandType">Command type.</param>
		/// <param name="pool">Pool from which the command will be taken.</param>
		/// <returns>Command or NULL.</returns>
		ICommand GetCommandFromPool(Type commandType);
	}
}
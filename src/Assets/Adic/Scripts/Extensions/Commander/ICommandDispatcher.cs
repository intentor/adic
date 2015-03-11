using System;

namespace Adic {
	/// <summary>
	/// Defines a command dispatcher.
	/// </summary>
	public interface ICommandDispatcher {
		/// <summary>
		/// Dispatches a command by type.
		/// </summary>
		/// <typeparam name="T">The type of the command to be dispatched.</typeparam>
		/// <param name="parameters">Command parameters.</param>
		void Dispatch<T>(params object[] parameters) where T : ICommand;
		
		/// <summary>
		/// Dispatches a command by type.
		/// </summary>
		/// <param name="type">The type of the command to be dispatched.</typeparam>
		/// <param name="parameters">Command parameters.</param>
		void Dispatch(Type type, params object[] parameters);

		/// <summary>
		/// Releases a command.
		/// </summary>
		/// <param name="command">Command to be released.</param>
		void Release(ICommand command);
		
		/// <summary>
		/// Releases all commands that are running.
		/// </summary>
		void ReleaseAll();
	}
}
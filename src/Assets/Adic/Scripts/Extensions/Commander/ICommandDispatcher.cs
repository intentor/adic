using System;
using System.Collections.Generic;

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
		/// Dispatches a command by type after a given time in seconds.
		/// </summary>
		/// <typeparam name="T">The type of the command to be dispatched.</typeparam>
		/// <param name="time">Time to dispatch the command (seconds).</param>
		/// <param name="parameters">Command parameters.</param>
		void Dispatch<T>(float time, params object[] parameters) where T : ICommand;
		
		/// <summary>
		/// Dispatches a command by type.
		/// </summary>
		/// <param name="type">The type of the command to be dispatched.</typeparam>
		/// <param name="parameters">Command parameters.</param>
		void Dispatch(Type type, params object[] parameters);
		
		/// <summary>
		/// Dispatches a command by type after a given time in seconds.
		/// </summary>
		/// <param name="type">The type of the command to be dispatched.</typeparam>
		/// <param name="time">Time to dispatch the command (seconds).</param>
		/// <param name="parameters">Command parameters.</param>
		void Dispatch(Type type, float time, params object[] parameters);
		
		/// <summary>
		/// Releases a command.
		/// </summary>
		/// <param name="command">Command to be released.</param>
		void Release(ICommand command);
		
		/// <summary>
		/// Releases all commands that are running.
		/// </summary>
		void ReleaseAll();
		
		/// <summary>
		/// Checks whether a given command of <typeparamref name="T"/> is registered.
		/// </summary>
		/// <typeparam name="T">Command type.</typeparam>
		/// <returns><c>true</c>, if registration exists, <c>false</c> otherwise.</returns>
		bool ContainsRegistration<T>() where T : ICommand;
		
		/// <summary>
		/// Checks whether a given command of <paramref name="type"/> is registered.
		/// </summary>
		/// <param name="type">Command type.</param>
		/// <returns><c>true</c>, if registration exists, <c>false</c> otherwise.</returns>
		bool ContainsRegistration(Type type);

		/// <summary>
		/// Gets all commands registered in the command dispatcher.
		/// </summary>
		/// <returns>All available registrations.</returns>
		Type[] GetAllRegistrations();
	}
}
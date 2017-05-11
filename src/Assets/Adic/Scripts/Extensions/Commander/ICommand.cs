using UnityEngine;
using System.Collections;

namespace Adic {
	/// <summary>
	/// Represents a command.
	/// </summary>
	public interface ICommand {
		/// <summary>The command dispatcher that dispatched this command.</summary>
        ICommandDispatcher dispatcher { get; set; }
        /// <summary>Command tag.</summary>
        string tag { get; set; }
		/// <summary>Indicates whether the command is running.</summary>
		bool running { get; set; }
		/// <summary>Indicates whether the command must be kept alive even after its execution.</summary>
		bool keepAlive { get; set; }
		/// <summary>
		/// Indicates whether this command is a singleton (there's only one instance of it).
		/// 
		/// Singleton commands improve performance and are the recommended approach when, for every execution
		/// of a command, there's no need to reinject dependencies and/or all parameters the command needs
		/// are passed through the <code>Execute()</code> method.
		/// </summary>
		bool singleton { get; }
		/// <summary>The quantity of the command to preload on pool.</summary>
		int preloadPoolSize { get; }
		/// <summary>The maximum size pool for this command.</summary>
		int maxPoolSize { get; }

		/// <summary>
		/// Executes the command.
		/// <param name="parameters">Command parameters.</param>
		void Execute(params object[] parameters);

		/// <summary>
		/// Retains the command as in use, not disposing it after execution.
		/// 
		/// Always call Release() after the command has terminated.
		/// </summary>
		void Retain();

		/// <summary>
		/// Release this command.
		/// </summary>
		void Release();
	}
}
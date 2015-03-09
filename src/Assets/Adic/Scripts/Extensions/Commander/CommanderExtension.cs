using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Adic.Commander.Exceptions;
using Adic.Container;
using Adic.Exceptions;

namespace Adic {
	/// <summary>
	/// Provides command registration capabilities to containers.
	/// </summary>
	public static class CommanderExtensions  {
		/// <summary>
		/// Register a command of type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="container">The container in which the command will be registered.</param>
		/// <typeparam name="T">The type of the command to be registered.</typeparam>
		public static void RegisterCommand<T>(this IInjectionContainer container) where T : ICommand, new() {
			container.RegisterCommand(typeof(T));
		}
		
		/// <summary>
		/// Register a command of type <paramref name="type"/>.
		/// 
		/// After all commands have been registered, call <code>PoolCommands()</code> to pool
		/// all commands.
		/// 
		/// If <code>RegisterCommands()</code> is used, the commands are already pooled.
		/// </summary>
		/// <param name="container">The container in which the command will be registered.</param>
		/// <param name="type">The type of the command to be registered.</param>
		public static void RegisterCommand(this IInjectionContainer container, Type type) {
			if (!type.IsClass && type.IsAssignableFrom(typeof(ICommand))) {
				throw new CommandException(CommandException.TYPE_NOT_A_COMMAND);
			}

			container.Bind<ICommand>().To(type);
		}

		/// <summary>
		/// Register all commands from a given namespace.
		/// 
		/// After registration, all commands are pooled.
		/// </summary>
		/// <param name="container">The container in which the command will be registered.</param>
		/// <param name="namespaceName">Namespace name.</param>
		public static void RegisterCommands(this IInjectionContainer container, string namespaceName) {
			var commandType = typeof(ICommand);
			var commands = 
				from t in Assembly.GetExecutingAssembly().GetTypes()
				where t.IsClass && t.Namespace == namespaceName && commandType.IsAssignableFrom(t)
				select t;
			
			foreach (var type in commands) {
				container.Bind<ICommand>().To(type);
			}

			PoolCommands(container);
		}

		/// <summary>
		/// Pools all commands on the container.
		/// </summary>
		/// <param name="container">The container in which the commands have been registered.</param>
		public static void PoolCommands(this IInjectionContainer container) {
			var dispatcher = container.Resolve<CommandDispatcher>();
			dispatcher.Pool();
		}
	}
}
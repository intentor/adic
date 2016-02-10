using System;
using System.Collections;
using Adic.Commander.Exceptions;
using Adic.Container;
using Adic.Exceptions;
using Adic.Util;

namespace Adic {
	/// <summary>
	/// Provides command registration capabilities to containers.
	/// </summary>
	public static class CommanderExtension  {
		/// <summary>
		/// Gets the command dispatcher in the container.
		/// </summary>
		/// <param name="container">The container in which the command is registered.</param>
		/// <returns>The command dispatcher.</returns>
		public static ICommandDispatcher GetCommandDispatcher(this IInjectionContainer container) {
			return container.Resolve<ICommandDispatcher>();
		}

		/// <summary>
		/// Register a command of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of the command to be registered.</typeparam>
		/// <param name="container">The container in which the command will be registered.</param>
		/// <returns>The injection container for chaining.</returns>
		public static IInjectionContainer RegisterCommand<T>(this IInjectionContainer container) where T : ICommand, new() {
			container.RegisterCommand(typeof(T));

			return container;
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
		/// <returns>The injection container for chaining.</returns>
		public static IInjectionContainer RegisterCommand(this IInjectionContainer container, Type type) {
			if (!TypeUtils.IsClass(type) || !TypeUtils.IsAssignable(type, typeof(ICommand))) {
				throw new CommandException(CommandException.TYPE_NOT_A_COMMAND);
			}

			container.Bind<ICommand>().To(type);
			
			return container;
		}
		
		/// <summary>
		/// Register all commands from a given namespace and its children namespaces.
		/// 
		/// After registration, all commands are pooled.
		/// </summary>
		/// <param name="container">The container in which the command will be registered.</param>
		/// <param name="namespaceName">Namespace name.</param>
		/// <returns>The injection container for chaining.</returns>
		public static IInjectionContainer RegisterCommands(this IInjectionContainer container, string namespaceName) {
			container.RegisterCommands(namespaceName, true);
			
			return container;
		}

		/// <summary>
		/// Register all commands from a given namespace.
		/// 
		/// After registration, all commands are pooled.
		/// </summary>
		/// <param name="container">The container in which the command will be registered.</param>
		/// <param name="includeChildren">Indicates whether child namespaces should be included.</param>
		/// <param name="namespaceName">Namespace name.</param>
		/// <returns>The injection container for chaining.</returns>
		public static IInjectionContainer RegisterCommands(this IInjectionContainer container,
		    string namespaceName,
		    bool includeChildren) {
			var commands = TypeUtils.GetAssignableTypes(typeof(ICommand), namespaceName, includeChildren);
			
			if (commands.Length > 0) {
				for (var cmdIndex = 0; cmdIndex < commands.Length; cmdIndex++) {
					var commandType = commands[cmdIndex];
					if (!TypeUtils.IsAbsract(commandType)) {
						container.Bind<ICommand>().To(commandType);
					}
				}
				
				PoolCommands(container);
			}
			
			return container;
		}

		/// <summary>
		/// Pools all commands on the container.
		/// </summary>
		/// <param name="container">The container in which the commands have been registered.</param>
		/// <returns>The injection container for chaining.</returns>
		public static IInjectionContainer PoolCommands(this IInjectionContainer container) {
			var commandPool = container.Resolve<ICommandPool>();
			commandPool.Pool();
			
			return container;
		}
	}
}
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
		/// </summary>
		/// <param name="container">The container in which the command will be registered.</param>
		/// <param name="type">The type of the command to be registered.</param>
		/// <returns>The injection container for chaining.</returns>
		public static IInjectionContainer RegisterCommand(this IInjectionContainer container, Type type) {
            container.Resolve<ICommandPool>().AddCommand(type);
			return container;
		}
		
		/// <summary>
		/// Register all commands from a given namespace and its children namespaces.
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
                var pool = container.Resolve<ICommandPool>();

				for (var cmdIndex = 0; cmdIndex < commands.Length; cmdIndex++) {
                    pool.AddCommand(commands[cmdIndex]);
				}
			}
			
			return container;
		}
	}
}
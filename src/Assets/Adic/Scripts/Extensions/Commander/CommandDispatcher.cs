using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adic.Commander.Exceptions;
using Adic.Container;

namespace Adic {
	/// <summary>
	/// Dispatches, releases and handles execution of commands.
	/// </summary>
	public class CommandDispatcher : IDisposable, ICommandDispatcher, ICommandPool {
		/// <summary>The available commands, including singletons and pooled.</summary>
		protected Dictionary<Type, object> commands;
		/// <summary>The container from which the command dispatcher was created.</summary>
		protected IInjectionContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.CommandDispatcher"/> class.
		/// </summary>
		/// <param name="container">Dependency injection container that created the command dispatcher.</param>
		public CommandDispatcher(IInjectionContainer container) {
			this.commands = new Dictionary<Type, object>();
			this.container = container;
		}

		/// <summary>
		/// Dispatches a command
		/// </summary>
		/// <typeparam name="T">The type of the command to be dispatched.</typeparam>
		/// <param name="parameters">Command parameters.</param>
		public void Dispatch<T>(params object[] parameters) where T : ICommand {
			this.Dispatch(typeof(T), parameters);
		}
		
		/// <summary>
		/// Dispatches a command
		/// </summary>
		/// <param name="type">The type of the command to be dispatched.</typeparam>
		/// <param name="parameters">Command parameters.</param>
		public void Dispatch(Type type, params object[] parameters) {
			if (this.ContainsRegistration(type)) {
				ICommand command = null;

				var item = this.commands[type];
				if (item is ICommand) {
					//Singleton.
					command = (ICommand)item;
				} else {
					//Transient.
					command = this.GetCommandFromPool(type, (List<ICommand>)item);
					this.container.Inject(command);
				}

				command.dispatcher = this;
				command.running = true;
				command.Execute(parameters);

				if (command.keepAlive) {
					//If the command has IUpdatable interface, adds it to the EventCaller extension.
					if (command is IUpdatable && !EventCallerContainerExtension.updateable.Contains((IUpdatable)command)) {
						EventCallerContainerExtension.updateable.Add((IUpdatable)command);
					}
				} else {
					this.Release(command);
				}
			} else {
				throw new CommandException(
					string.Format(CommandException.NO_COMMAND_FOR_TYPE, type));
			}
		}
		
		/// <summary>
		/// Dispatches a command by type after a given time in seconds.
		/// </summary>
		/// <typeparam name="T">The type of the command to be dispatched.</typeparam>
		/// <param name="time">Time to dispatch the command (seconds).</param>
		/// <param name="parameters">Command parameters.</param>
		public void InvokeDispatch<T>(float time, params object[] parameters) where T : ICommand {
			EventCallerContainerExtension.eventCaller.StartCoroutine(this.DispatchInvoke(typeof(T), time, parameters));
		}
		
		/// <summary>
		/// Dispatches a command by type after a given time in seconds.
		/// </summary>
		/// <param name="type">The type of the command to be dispatched.</typeparam>
		/// <param name="time">Time to dispatch the command (seconds).</param>
		/// <param name="parameters">Command parameters.</param>
		public void InvokeDispatch(Type type, float time, params object[] parameters) {
			EventCallerContainerExtension.eventCaller.StartCoroutine(this.DispatchInvoke(type, time, parameters));
		}

		/// <summary>
		/// Dispatches a command by type after a given time in seconds.
		/// </summary>
		/// <typeparam name="T">The type of the command to be dispatched.</typeparam>
		/// <param name="time">Time to dispatch the command (seconds).</param>
		/// <param name="parameters">Command parameters.</param>
		protected IEnumerator DispatchInvoke(Type type, float time, params object[] parameters) {
			yield return new UnityEngine.WaitForSeconds(time);
			this.Dispatch(type, parameters);
		}

		/// <summary>
		/// Releases a command.
		/// </summary>
		/// <param name="command">Command to be released.</param>
		public void Release(ICommand command) {
			if (!command.running) return;

			//If the command has IUpdatable interface, and is on the EventCaller extension, removes it.
			if (command is IUpdatable && EventCallerContainerExtension.updateable.Contains((IUpdatable)command)) {
				EventCallerContainerExtension.updateable.Remove((IUpdatable)command);
			}
			//If the command has IDisposable interface, calls the Dispose() method. 
			if (command is IDisposable) {
				((IDisposable)command).Dispose();
			}

			command.running = false;
			command.keepAlive = false;
		}
		
		/// <summary>
		/// Releases all commands that are running.
		/// </summary>
		public void ReleaseAll() {
			foreach (var entry in this.commands) {
				if (entry.Value is ICommand) {
					this.Release((ICommand)entry.Value);
				} else {
					var pool = (List<ICommand>)entry.Value;
					for (int poolIndex = 0; poolIndex < pool.Count; poolIndex++) {					
						this.Release((ICommand)pool[poolIndex]);
					}
				}
			}
		}

		/// <summary>
		/// Releases all commands that are running.
		/// </summary>
		/// <typeparam name="T">The type of the commands to be released.</typeparam>
		public void ReleaseAll<T>() where T : ICommand {
			this.ReleaseAll(typeof(T));
		}
		
		/// <summary>
		/// Releases all commands that are running.
		/// </summary>
		/// <param name="type">The type of the commands to be released.</param>
		public void ReleaseAll(Type type) {
			foreach (var entry in this.commands) {
				if (entry.Value is ICommand && entry.Value.GetType().Equals(type)) {
					this.Release((ICommand)entry.Value);
				} else {
					var pool = (List<ICommand>)entry.Value;
					for (int poolIndex = 0; poolIndex < pool.Count; poolIndex++) {
						var command = (ICommand)pool[poolIndex];

						if (command.GetType().Equals(type)) {
							this.Release(command);
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Pools all commands.
		/// </summary>
		public void Pool() {
			var resolvedCommands = container.ResolveAll<ICommand>();

			for (var cmdIndex = 0; cmdIndex < resolvedCommands.Length; cmdIndex++) {
				var command = resolvedCommands[cmdIndex];
				var commandType = command.GetType();

				//If the type already exists in the pool, goes to the next type.
				if (this.commands.ContainsKey(commandType)) continue;

				if (command.singleton) {
					this.commands.Add(commandType, command);
				} else {
					var commandPool = new List<ICommand>(command.preloadPoolSize);

					//Adds the currently resolved command.
					commandPool.Add(command);

					//Adds other commands until matches preloadPoolSize.
					if (command.preloadPoolSize > 1) {
						for (int itemIndex = 1; itemIndex < command.preloadPoolSize; itemIndex++) {
							commandPool.Add((ICommand)container.Resolve(commandType));
						}
					}
							
					this.commands.Add(commandType, commandPool);
				}
			}
		}
		
		/// <summary>
		/// Checks whether a given command of <typeparamref name="T"/> is registered.
		/// </summary>
		/// <typeparam name="T">Command type.</typeparam>
		/// <returns><c>true</c>, if registration exists, <c>false</c> otherwise.</returns>
		public bool ContainsRegistration<T>() where T : ICommand {
			return this.commands.ContainsKey(typeof(T));
		}

		/// <summary>
		/// Checks whether a given command of <paramref name="type"/> is registered.
		/// </summary>
		/// <param name="type">Command type.</param>
		/// <returns><c>true</c>, if registration exists, <c>false</c> otherwise.</returns>
		public bool ContainsRegistration(Type type) {
			return this.commands.ContainsKey(type);
		}

		/// <summary>
		/// Gets all commands registered in the command dispatcher.
		/// </summary>
		/// <returns>All available registrations.</returns>
		public Type[] GetAllRegistrations() {
			return this.commands.Keys.ToArray();
		}
		
		/// <summary>
		/// Gets a command from the pool.
		/// </summary>
		/// <param name="commandType">Command type.</param>
		/// <param name="pool">Pool from which the command will be taken.</param>
		/// <returns>Command or NULL.</returns>
		public ICommand GetCommandFromPool(Type commandType) {
			ICommand command = null;
			
			if (this.commands.ContainsKey(commandType)) {
				var item = this.commands[commandType];
				command = this.GetCommandFromPool(commandType, (List<ICommand>)item);
			}
			
			return command;
		}
		
		/// <summary>
		/// Gets a command from the pool.
		/// </summary>
		/// <param name="commandType">Command type.</param>
		/// <param name="pool">Pool from which the command will be taken.</param>
		/// <returns>Command or NULL.</returns>
		public ICommand GetCommandFromPool(Type commandType, List<ICommand> pool) {
			ICommand command = null;
			
			//Finds the first available command on the list.
			for (int cmdIndex = 0; cmdIndex < pool.Count; cmdIndex++) {
				var commandOnPool = pool[cmdIndex];
				
				if (!commandOnPool.running) {
					command = commandOnPool;
					break;
				}
			}
			
			//If no command is found, instantiates a new one until command.maxPoolSize is reached.
			if (command == null) {
				if (pool.Count > 0 && pool.Count >= pool[0].maxPoolSize) {							
					throw new CommandException(
						string.Format(CommandException.MAX_POOL_SIZE, pool[0].ToString()));
				}
				
				command = (ICommand)this.container.Resolve(commandType);
				pool.Add(command);
			}
			
			return command;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Adic.CommandDispatcher"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Adic.CommandDispatcher"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Adic.CommandDispatcher"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Adic.CommandDispatcher"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Adic.CommandDispatcher"/> was occupying.</remarks>
		public void Dispose() {
			this.ReleaseAll();
			this.commands.Clear();
		}
	}
}
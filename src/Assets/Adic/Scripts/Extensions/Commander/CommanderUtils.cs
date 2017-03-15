using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Adic.Commander.Exceptions;
using Adic.Util;

namespace Adic.Commander {
	/// <summary>
	/// Commander utils.
	/// </summary>
	public static class CommanderUtils {
		/// <summary>
		/// Gets all the available command types.
		/// </summary>
		public static Type[] GetAvailableCommands() {
			var types = new List<Type>();
			
			// Look for assignable commands in all available assemblies.
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int assemblyIndex = 0; assemblyIndex < assemblies.Length; assemblyIndex++) {
				var assemly = assemblies[assemblyIndex];
				
				if (assemly.FullName.StartsWith("Unity") ||
				    assemly.FullName.StartsWith("Boo") ||
				    assemly.FullName.StartsWith("Mono") ||
				    assemly.FullName.StartsWith("System") ||
				    assemly.FullName.StartsWith("mscorlib")) {
					continue;
				}

				var commandType = typeof(ICommand);
				var allTypes = assemblies[assemblyIndex].GetTypes();
				for (int typeIndex = 0; typeIndex < allTypes.Length; typeIndex++) {
					var type = allTypes[typeIndex];

					if (type.Namespace != "Adic" &&
					    type.IsClass &&
					    TypeUtils.IsAssignable(commandType, type)) {
						types.Add(type);
					}
				}
			}
			
			return types.ToArray();
		}

		/// <summary>
		/// Dispatches a command.
		/// </summary>
		/// <param name="type">Command type.</param>
		/// <param name="parameters">Command parameters.</param>
		public static void DispatchCommand(Type type, params object[] parameters) {
			var bindingFound = false;
			var containers = ContextRoot.containersData;
			
			for (int index = 0; index < containers.Count; index++) {
				var container = containers[index].container;
				
				if (container.ContainsBindingFor<ICommandDispatcher>()) {
					var dispatcher = container.GetCommandDispatcher();
					
					if (dispatcher.ContainsRegistration(type)) {
						bindingFound = true;
						dispatcher.Dispatch(type, parameters);
						break;
					}
				}
			}
			
			if (!bindingFound) {
				throw new CommandException(string.Format(CommandException.NO_COMMAND_FOR_TYPE, type));
			}
		}

		/// <summary>
		/// Gets a distinct list of namespaces from types.
		/// </summary>
		/// <param name="types">Types to get the namespace from.</param>
		/// <returns>The namespaces.</returns>
		public static Dictionary<string, IList<string>> GetTypesAsString(Type[] types) {
			var typesList = new Dictionary<string, IList<string>>();
			IList<string> typeNames;

			for (var typeIndex = 0; typeIndex < types.Length; typeIndex++) {
				var type = types[typeIndex];
				var key = "-";

				if (!string.IsNullOrEmpty(type.Namespace)) {
					key = type.Namespace;
				}

				if (typesList.ContainsKey(key)) {
					typeNames = typesList[key];
				} else {
					typeNames = new List<string>();
					typesList.Add(key, typeNames);
				}

				typeNames.Add(type.Name);
			}

			return typesList;
		}
	}
}
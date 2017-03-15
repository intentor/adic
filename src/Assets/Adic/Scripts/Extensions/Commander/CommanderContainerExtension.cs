using UnityEngine;
using System;
using System.Collections;
using Adic.Binding;
using Adic.Container;
using Adic.Exceptions;
using Adic.Injection;
using Adic.Util;

namespace Adic {
	/// <summary>
	/// Container extension for the Commander Adic Extension.
	/// 
	/// Ensures the <see cref="Adic.CommandDispatcher"/> is added to the container.
	/// </summary>
    public class CommanderContainerExtension : IContainerExtension {	
        public void Init(IInjectionContainer container) {
            container.Resolve<ICommandDispatcher>().Init();
        }

		public void OnRegister(IInjectionContainer container) {			
			// Bind the command dispatcher to a singleton, so every command can receive the instance.
			container.Bind<ICommandDispatcher>().ToSingleton<CommandDispatcher>();
			// Bind the command pool to the CommandDispatcher.
			var dispatcher = (CommandDispatcher)container.Resolve<ICommandDispatcher>();
			container.Bind<ICommandPool>().To<CommandDispatcher>(dispatcher);
		}
		
		public void OnUnregister(IInjectionContainer container) {
			// Unbind the command dispatcher and pool.
			container.Unbind<ICommandDispatcher>();
			container.Unbind<ICommandPool>();

			// Unbind all commands, if there's any.
			container.Unbind<ICommand>();
		}
	}
}
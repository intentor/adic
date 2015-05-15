using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adic {
	/// <summary>
	/// Basic implementation of a command.
	/// </summary>
	public abstract class Command : ICommand, IDisposable {
		/// <summary>The command dispatcher that dispatched this command.</summary>
		public ICommandDispatcher dispatcher { get; set; }
		/// <summary>Indicates whether the command is running.</summary>
		public bool running { get; set; }
		/// <summary>Indicates whether the command must be kept alive even after its execution.</summary>
		public bool keepAlive { get; set; }
		/// <summary>
		/// Indicates whether this command is a singleton (there's only one instance of it).
		/// 
		/// Singleton commands improve performance and are the recommended approach when, for every execution
		/// of your command, there's no need to reinject every dependency or all parameters the command needs
		/// are passed through the <code>Execute()</code> method.
		/// </summary>
		public virtual bool singleton { get { return false; } }
		/// <summary>The quantity of the command to preload on pool (default: 1).</summary>
		public virtual int preloadPoolSize { get { return 1; } }
		/// <summary>The maximum size pool for this command (default: 10).</summary>
		public virtual int maxPoolSize { get { return 10; } }

		/// <summary>Coroutines started on this command.</summary>
		private List<Coroutine> coroutines = new List<Coroutine>(1);
		
		/// <summary>
		/// Executes the command.
		/// <param name="parameters">Command parameters.</param>
		public abstract void Execute(params object[] parameters);
		
		/// <summary>
		/// Retains the command as in use, not disposing it after execution.
		/// 
		/// Always call Release() after the command has terminated.
		/// </summary>
		public virtual void Retain() {
			this.keepAlive = true;
		}
		
		/// <summary>
		/// Release this command.
		/// </summary>
		public virtual void Release() {
			this.keepAlive = false;

			this.dispatcher.Release(this);
		}

		/// <summary>
		/// Called when a command is disposed.
		/// </summary>
		public virtual void Dispose() {
			while (this.coroutines.Count > 0) {
				this.StopCoroutine(this.coroutines[0]);
			}
		}

		/// <summary>
		/// Invokes the specified method after a specific time in seconds.
		/// </summary>
		/// <param name="method">Method to be called.</param>
		/// <param name="time">Time to call the method (seconds).</param>
		protected void Invoke(Action method, float time) {
			var routine = this.MethodInvoke(method, time);
			this.StartCoroutine(routine);
		}

		/// <summary>
		/// Starts a coroutine.
		/// </summary>
		/// <param name="routine">Routine to be started.</param>
		/// <returns>The coroutine.</returns>
		protected Coroutine StartCoroutine(IEnumerator routine) {
			var coroutine = EventCallerContainerExtension.eventCaller.StartCoroutine(routine);
			this.coroutines.Add(coroutine);
			this.Retain();

			return coroutine;
		}

		/// <summary>
		/// Stops a coroutine.
		/// </summary>
		/// <param name="coroutine">Coroutine to be stopped.</param>
		protected void StopCoroutine(Coroutine coroutine) {
			EventCallerContainerExtension.eventCaller.StopCoroutine(coroutine);
			this.coroutines.Remove(coroutine);
		}

		/// <summary>
		/// Invokes the specified method after a specific time in seconds using a coroutine.
		/// </summary>
		/// <param name="method">Method to be called.</param>
		/// <param name="time">Time to call the method (seconds).</param>
		private IEnumerator MethodInvoke(Action method, float time) {
			yield return new WaitForSeconds(time);
			method();
		}
	}
}
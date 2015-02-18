using System;
using System.Collections.Generic;

namespace Adic {
	/// <summary>
	/// Context root MonoBehaviour.
	/// </summary>
	public abstract class ContextRoot : UnityEngine.MonoBehaviour, IContextRoot {
		/// <summary>Instance of the context root.</summary>
		public static ContextRoot instance;

		/// <summary>Contexts list.</summary>
		public IContext[] contexts { get; private set; }

		protected void Awake() {
			instance = this;
			this.SetupContexts();
			this.Init();
		}
		
		/// <summary>
		/// Adds the specified context.
		/// </summary>
		/// <typeparam name="T">The type of the context to be added.</typeparam>
		public void AddContext<T>() where T : IContext {
			var context = (IContext)Activator.CreateInstance(typeof(T));

			var list = new List<IContext>();
			if (this.contexts != null) list.AddRange(this.contexts);
			list.Add(context);
			this.contexts = list.ToArray();
		}

		/// <summary>
		/// Setup the contexts.
		/// </summary>
		public abstract void SetupContexts();
		
		/// <summary>
		/// Init the game.
		/// </summary>
		public abstract void Init();
	}
}
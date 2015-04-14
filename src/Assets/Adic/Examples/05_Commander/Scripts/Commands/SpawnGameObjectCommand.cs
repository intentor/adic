using UnityEngine;
using Adic.Container;

namespace Adic.Examples.Commander.Commands {
	/// <summary>
	/// Spawns a cube and makes it rotate.
	/// </summary>
	public class SpawnGameObjectCommand : Command {
		/// <summary>Container that dispatched the command.</summary>
		[Inject]
		public IInjectionContainer container;

		public override void Execute(params object[] parameters) {
			var prefab = container.Resolve<Transform>();
			this.dispatcher.Dispatch<RotateGameObjectCommand>(prefab);
		}
	}
}
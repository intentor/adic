using UnityEngine;
using Adic.Container;

namespace Adic.Examples.Commander.Commands {
	/// <summary>
	/// Spawns a cube that is on the container.
	/// </summary>
	public class SpawnGameObjectCommand : Command {
		[Inject]
		public IInjectionContainer container;

		public override void Execute(params object[] parameters) {
			var prefab = container.Resolve<Transform>();
			this.dispatcher.Dispatch<RotateGameObjectCommand>(prefab);
		}
	}
}
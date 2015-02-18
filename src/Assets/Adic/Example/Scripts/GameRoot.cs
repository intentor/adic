using UnityEngine;

namespace Adic.Example {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Adic.ContextRoot {
		public override void SetupContexts() {
			this.AddContext<GameContext>();
		}
		
		public override void Init() {
			var plane = Resources.Load("Plane");
			Instantiate(plane);
		}
	}
}
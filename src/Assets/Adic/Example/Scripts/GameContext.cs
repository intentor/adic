using UnityEngine;

namespace Adic.Example {
	/// <summary>
	/// Dependency injection context.
	/// </summary>
	public class GameContext : Adic.Context {
		public override void SetupBindings() {
			this.Bind("cube").To<Transform>(GameObject.Find("Cube").GetComponent<Transform>());
			this.Bind<Dependency>().AsSingleton();
		}
	}
}
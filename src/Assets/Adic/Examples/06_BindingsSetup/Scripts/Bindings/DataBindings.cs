using UnityEngine;
using Adic.Container;
using Adic.Examples.BindingsSetup.Data;

namespace Adic.Examples.BindingsSetup.Bindings {
	/// <summary>
	/// Bindings for data related objects.
	/// </summary>
	/// <remarks>
	/// This bindings have priority because they must be in place before any prefab bindings.
	/// </remarks>
	[BindingPriority]
	public class DataBindings : Adic.IBindingsSetup {
		public void SetupBindings(IInjectionContainer container) {
			container
				//Bind the rotation data for "CubeA".
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(0.5f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeA"))
				//Bind the rotation data for "CubeB".
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(2.0f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeB"))
				//Bind the rotation data for "CubeC".
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(4.5f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeC"));
		}		
	}
}
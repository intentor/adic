using UnityEngine;
using System.Collections;

namespace Adic {
	/// <summary>
	/// Allows an object that is binded on Adic to receive updates.
	/// </summary>
	public interface IUpdateable {
		/// <summary>
		/// Called every frame.
		/// </summary>
		void Update();
	}
}
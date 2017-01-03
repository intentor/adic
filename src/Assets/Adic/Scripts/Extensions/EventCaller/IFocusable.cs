namespace Adic {
	/// <summary>
	/// Allows an object that is binded on Adic to receive OnApplicationFocus events.
	/// </summary>
	public interface IFocusable {
		/// <summary>
		/// Called when the application's focus is changing.
		/// </summary>
		void OnApplicationFocus(bool hasFocus);
	}
}
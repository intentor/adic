namespace Adic {
    /// <summary>
    /// Allows an object that is binded on Adic to receive OnApplicationFocus events.
    /// </summary>
    public interface IFocusable {
        /// <summary>
        /// Called when the application focus is changing.
        /// </summary>
        /// <param name="hasFocus">If set to <c>true</c> has focus.</param>
        void OnApplicationFocus(bool hasFocus);
    }
}
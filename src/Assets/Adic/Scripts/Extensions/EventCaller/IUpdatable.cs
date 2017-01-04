namespace Adic {
    /// <summary>
    /// Allows an object that is binded on Adic to receive Update events.
    /// </summary>
    public interface IUpdatable {
        /// <summary>
        /// Called every frame.
        /// </summary>
        void Update();
    }
}
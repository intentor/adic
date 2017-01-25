using Adic;
using UnityEngine;

namespace Assets.Test.MultiScene {
    /// <summary>
    /// Displays a log on update.
    /// </summary>
    public class UpdateLogger : IUpdatable {
        public void Update() {
            Debug.Log("Updating...");
        }
    }
}

using UnityEngine;
using System.Collections;
using Adic;

namespace Adic.Examples.Commander.Commands {
    /// <summary>
    /// Game object rotator.
    /// 
    /// Receives the Transform component of the game object to be rotated as
    /// a parameter during execution.
    /// </summary>
    public class RotateGameObjectCommand : Command, IUpdatable {
        /// <summary>Object to rotate.</summary>
        protected Transform objectToRotate;

        public override void Execute(params object[] parameters) {
            this.objectToRotate = (Transform) parameters[0];

            // Call "Retain()" to keep the command running after the "Execute()" method is called.
            // This way the command can receive update events.
            // In this example the command will not be released manually. However, depending on the action being
            // executed (e.g. some network call) you'll have to release the command manually by calling "Release()".
            this.Retain();
        }

        public void Update() {
            if (this.objectToRotate != null) {
                this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
            }
        }

        public override void Dispose() {
            base.Dispose();
            Debug.Log("RotateGameObjectCommand released");
        }
    }
}
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adic.Commander.Behaviours.Editors {
    /// <summary>
    /// Inspector editor for <see cref="Adic.Commander.Behaviours.TimedCommandDispatch"/>.
    /// </summary>
    [CustomEditor(typeof(TimedCommandDispatch))]
    public class TimedCommandDispatchEditor : NamespaceCommandEditor<TimedCommandDispatch> {
        public override void OnInspectorGUI() {
            this.component.timer = EditorGUILayout.FloatField("Timer (seconds)", this.component.timer);

            base.OnInspectorGUI();
        }
    }
}
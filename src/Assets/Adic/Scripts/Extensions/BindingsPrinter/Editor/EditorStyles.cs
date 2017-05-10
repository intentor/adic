using UnityEngine;
using UnityEditor;
using System;

namespace Adic.Extenstions.BindingsPrinter {
    /// <summary>
    /// Editor styles for GUI.
    /// </summary>
    public static class EditorStyles {
        /// <summary>Message style.</summary>
        public static GUIStyle message {
            get {
                var style = new GUIStyle();
                style.fontSize = 16;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        /// <summary>Styles for titles.</summary>
        public static GUIStyle title {
            get {
                var style = new GUIStyle();
                style.fontSize = 13;
                style.fontStyle = FontStyle.Bold;	
                style.alignment = TextAnchor.MiddleLeft;			
                return style;
            }
        }

        /// <summary>Styles for container's info.</summary>
        public static GUIStyle containerInfo {
            get {
                var style = new GUIStyle();
                style.fontSize = 9;
                style.alignment = TextAnchor.MiddleLeft;			
                return style;
            }
        }

        /// <summary>Styles for container's names.</summary>
        public static GUIStyle containerName {
            get {
                var style = new GUIStyle();
                style.fontSize = 12;
                style.fontStyle = FontStyle.Bold;	
                style.alignment = TextAnchor.UpperLeft;			
                return style;
            }
        }

        /// <summary>Styles for binding's data..</summary>
        public static GUIStyle bindinds {
            get {
                var style = new GUIStyle();
                style.fontSize = 12;
                style.alignment = TextAnchor.UpperLeft;			
                return style;
            }
        }
    }
}
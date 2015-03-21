using UnityEngine;
using UnityEditor;
using Adic;

namespace Adic.Extenstions.BindingsPrinter {
	/// <summary>
	/// Prints bindings from containers in the current scene.
	/// </summary>
	public class BindingsPrinterWindow : EditorWindow {
		/// <summary>Current editor.</summary>
		private static BindingsPrinterWindow editor;
		
		/// <summary>Current scroll positioning.</summary>
		private Vector2 scrollPosition = Vector2.zero;

		[MenuItem ("Window/Adic/Bindings Printer")]
		protected static void Init () {
			editor = EditorWindow.GetWindow<BindingsPrinterWindow>("Bindings Printer",  typeof(SceneView));
		}
		
		protected void OnGUI () {
			if (!editor) {
				editor = (BindingsPrinterWindow)EditorWindow.GetWindow<BindingsPrinterWindow>();
			}

			if (!Application.isPlaying) {
				GUI.Label(new Rect(0, 0, Screen.width, Screen.height), 
					"Please execute the bindings printer on Play Mode", EditorStyles.message);
				return;
			}

			if (ContextRoot.containersData == null || ContextRoot.containersData.Count == 0) {
				GUI.Label(new Rect(0, 0, Screen.width, Screen.height), 
					"There are no containers in the current scene", EditorStyles.message);
				return;
			}

			//Calculates the window size.
			float size = 0;
			for (int dataIndex = 0; dataIndex < ContextRoot.containersData.Count; dataIndex++) {
				var data = ContextRoot.containersData[dataIndex];
				var bindings = data.container.GetBindings();
				size = 20 + 10 + 15 + 30;

				for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
					size += 80;
				}
			}
			if (size == 0) {
				size = editor.position.height;
			}

			//Begins scroll.
			this.scrollPosition = GUI.BeginScrollView(
				new Rect(0, 0, editor.position.width, editor.position.height),
				this.scrollPosition,
				new Rect(0, 0, editor.position.width - 30, size)
			);

			GUI.Label(new Rect(5, 5, 200, 15), 
          		"Adic Bindings Printer", EditorStyles.title);
			GUI.Label(new Rect(5, 22, 200, 10), 
				"Displays all bindings of all containers into the current ContextRoot", EditorStyles.containerInfo);

			//Displays the containers and their bindings.
			var currentY = 20;
			for (int dataIndex = 0; dataIndex < ContextRoot.containersData.Count; dataIndex++) {
				var data = ContextRoot.containersData[dataIndex];
				var bindings = data.container.GetBindings();
				
				currentY += 10;
				GUI.Label(new Rect(5, currentY, 200, 30), "CONTAINER", EditorStyles.containerInfo);
				currentY += 15;
				GUI.Label(
					new Rect(5, currentY, 200, 30),
					string.Format(
						"{0} (index: {1}, {2})",
              			data.container.GetType().FullName, dataIndex,
						(data.destroyOnLoad ? "destroy on load" : "singleton")
	              	),
	          		EditorStyles.title
				);
				
				currentY += 30;
				for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
					var binding = bindings[bindingIndex];

					GUI.Label(new Rect(15, currentY, 200, 30), binding.ToString(), EditorStyles.bindinds);
					currentY += 80;
				}
			}
			
			GUI.EndScrollView();
		}
	}
}
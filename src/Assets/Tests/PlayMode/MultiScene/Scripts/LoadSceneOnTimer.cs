using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Test.MultiScene {
    /// <summary>
    /// Loads a scene based on a timer.
    /// </summary>
    public class LoadSceneOnTimer : MonoBehaviour {
        [Tooltip("Time to wait to load the scene (seconds).")]
        public int timer = 1;
        [Tooltip("Name of the scene to load")]
        public string sceneName = "TestMultiSceneScene2";

        private void Start() {
            this.Invoke("LoadScene", this.timer);
        }

        private void LoadScene() {
            SceneManager.LoadScene(this.sceneName);
        }
    }
}

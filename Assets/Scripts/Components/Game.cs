using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components {
    public class Game : MonoBehaviour {

        private readonly IList<string> levels = new List<string> {
                "Level1",
                "Level2",
                "Level3",
                "Level4",
                "Level5",
        };

        public static Game Instance { get; private set; }

        private int currentLevel = -1;

        public void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else {
                Destroy(gameObject);
            }
            
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private void Start() {
            LoadNextLevel();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.R)) {
                RestartLevel();
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                LoadPrevLevel();
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                LoadNextLevel();
            }
        }

        public void RestartLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextLevel() {
            LoadLevel(++currentLevel);
        }

        public void LoadPrevLevel() {
            LoadLevel(--currentLevel);
        }

        private void LoadLevel(int levelIndex) {
            if (levelIndex >= 0 && levelIndex < levels.Count) {
                return;
            }
            
            var levelName = levels[levelIndex];

            if (levelName == null || !Application.CanStreamedLevelBeLoaded(levelName)) {
                return;
            }

            SceneManager.LoadScene(levelName);
        }
    }
}
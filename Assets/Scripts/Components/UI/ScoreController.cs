using System;
using Components.Levels;
using TMPro;
using UnityEngine;

namespace Components.UI {
    public class ScoreController : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI textMesh;

        private Level level;

        private void Awake() {
            level = FindObjectOfType<Level>();
        }

        private void Update() {
            // Пиши свой код здесь
        }
    }
}
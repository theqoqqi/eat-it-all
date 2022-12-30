using System;
using UnityEngine;

namespace Components.Levels {
    public class Level : MonoBehaviour {

        [SerializeField] private int score;

        public int Score => score;

        public void AddScore(int score) {
            this.score += score;
        }
    }
}
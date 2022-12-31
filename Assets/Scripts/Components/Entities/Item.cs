using Components.Entities.Players;
using UnityEngine;

namespace Components.Entities {
    public class Item : EntityBehaviour {

        [SerializeField] private Player player;

        [SerializeField] private Animator animator;

        [SerializeField] public int pickupScore = 1;
        
        private bool isCollected;

        protected override void Awake() {
            base.Awake();
            player = FindObjectOfType<Player>();
        }
        
        private void Update() {
            if (!isCollected && player && player.Body.CellPosition == CellPosition) {
                Collect();
            }
        }

        private void Collect() {
            // Пиши код тут
        }
    }
}
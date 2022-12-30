using UnityEngine;

namespace Components.Entities.Players {
    public class Player : EntityBehaviour {

        [SerializeField] private GridAlignedBody body;

        [SerializeField] private SpriteRenderer spriteRenderer;

        public GridAlignedBody Body => body;

        public bool IsAlive { get; private set; } = true;

        public void Kill() {
            IsAlive = false;
            spriteRenderer.color = Color.red;
        }
    }
}
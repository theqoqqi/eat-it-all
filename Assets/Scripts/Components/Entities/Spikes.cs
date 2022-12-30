using System.Collections;
using Components.Entities.Players;
using UnityEngine;

namespace Components.Entities {
    public class Spikes : EntityBehaviour {
        
        private Player player;
        
        private bool isActivated;

        protected override void Awake() {
            base.Awake();
            player = FindObjectOfType<Player>();
        }

        private void Update() {
            if (!isActivated && player && player.Body.CellPosition == CellPosition) {
                Activate();
            }
        }

        private void Activate() {
            isActivated = true;

            player.Kill();

            StartCoroutine(RestartLevel());
        }

        private IEnumerator RestartLevel() {
            yield return new WaitForSeconds(1.5f);
            
            Game.Instance.RestartLevel();
        }
    }
}
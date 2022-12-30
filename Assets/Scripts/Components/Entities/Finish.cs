using System.Collections;
using System.Linq;
using Components.Entities.Players;
using UnityEngine;

namespace Components.Entities {
    public class Finish : EntityBehaviour {

        [SerializeField] private Animator animator;

        [SerializeField] private int requiredScore;
        
        private Player player;

        private bool HasRequiredScore => Level.Score >= requiredScore;

        private bool isActivated;

        protected override void Awake() {
            base.Awake();
            player = FindObjectOfType<Player>();

            if (requiredScore == 0) {
                requiredScore = FindObjectsOfType<Item>().Sum(item => item.pickupScore);
            }
        }
        
        private void Update() {
            if (!isActivated && HasRequiredScore && player && player.Body.CellPosition == CellPosition) {
                Activate();
            }
        }

        private void Activate() {
            isActivated = true;

            animator.Play("Activated");

            StartCoroutine(GoToNextLevel());
        }

        private IEnumerator GoToNextLevel() {
            yield return new WaitForSeconds(3f);
            
            Game.Instance.LoadNextLevel();
        }
    }
}
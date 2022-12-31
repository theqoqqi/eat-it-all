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
        
        // Пиши код тут
    }
}
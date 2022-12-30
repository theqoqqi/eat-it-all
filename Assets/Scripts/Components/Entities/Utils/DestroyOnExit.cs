using UnityEngine;

namespace Components.Entities.Utils {
    public class DestroyOnExit : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Destroy(animator.gameObject, stateInfo.length);
        }
    }
}
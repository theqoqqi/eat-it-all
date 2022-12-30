using UnityEngine;

namespace Components.Entities {
    public class Elevator : EntityBehaviour {

        private void Start() {
            Destroy(GetComponent<SpriteRenderer>());
        }
    }
}
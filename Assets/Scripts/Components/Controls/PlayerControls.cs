using Components.Entities.Players;
using Components.Worlds;
using Core.Util;
using UnityEngine;

namespace Components.Controls {
    public class PlayerControls : MonoBehaviour {

        [SerializeField] private Player player;

        [SerializeField] private World world;

        private void Update() {
            if (player.Body.IsMoving || player.Body.ShouldFall || !player.IsAlive) {
                return;
            }

            var h = GetHorizontalDirection();
            var v = GetVerticalDirection();

            if (h.HasValue && player.CanWalk(h.Value)) {
                player.MoveTo(h.Value);
            }
            else if (h.HasValue && player.CanPush(h.Value)) {
                player.PushTo(h.Value);
            }
            else if (v == Direction.Down && player.CanStomp()) {
                player.PushTo(v.Value);
            }
            else if (v.HasValue && player.CanFly(v.Value)) {
                player.MoveTo(v.Value);
            }
        }

        private Direction? GetHorizontalDirection() {
            var left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            var right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

            if (left == right) {
                return null;
            }

            return left ? Direction.Left : Direction.Right;
        }

        private Direction? GetVerticalDirection() {
            var up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            var down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

            if (up == down) {
                return null;
            }

            return up ? Direction.Up : Direction.Down;
        }
    }
}
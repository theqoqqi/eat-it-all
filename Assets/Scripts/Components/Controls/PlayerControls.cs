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

            if (h.HasValue && CanWalk(h.Value)) {
                player.Body.StartMoveTo(h.Value);
            }
            else if (h.HasValue && CanPush(h.Value)) {
                var targetCellPosition = player.Body.GetCellPositionAt(h.Value);
                var pushedBody = world.GetBodyAtCell(targetCellPosition);
                var pushSpeed = pushedBody.MoveSpeed;

                player.Body.StartMoveTo(h.Value, pushSpeed);
                pushedBody.StartMoveTo(h.Value, pushSpeed);
                pushedBody.FallOnce();
            }
            else if (v == Direction.Down && CanStomp()) {
                var targetCellPosition = player.Body.GetCellPositionAt(v.Value);
                var pushedBody = world.GetBodyAtCell(targetCellPosition);
                var pushSpeed = pushedBody.FallSpeed;

                player.Body.StartMoveTo(v.Value, pushSpeed);
                pushedBody.StartMoveTo(v.Value, pushSpeed);
                pushedBody.FallOnce();
            }
            else if (v.HasValue && CanFly(v.Value)) {
                player.Body.StartMoveTo(v.Value);
            }
        }

        private bool CanWalk(Direction direction) {
            var targetCellPosition = player.Body.GetCellPositionAt(direction);

            return world.IsCellPassable(targetCellPosition);
        }

        private bool CanPush(Direction direction) {
            var targetCellPosition = player.Body.GetCellPositionAt(direction);
            var targetBody = world.GetBodyAtCell(targetCellPosition);
            var finalBodyPosition = player.Body.CellPosition + direction.AsCellPosition() * 2;
            var isFinalPositionPassable = world.IsCellEmpty(finalBodyPosition);

            return targetBody
                   && !targetBody.IsMoving
                   && targetBody.IsPushable
                   && isFinalPositionPassable;
        }

        private bool CanStomp() {
            var finalBodyPosition = player.Body.CellPosition + Direction.Down.AsCellPosition() * 2;
            
            return CanPush(Direction.Down) && !world.HasElevatorAt(finalBodyPosition);
        }

        private bool CanFly(Direction direction) {
            var targetCellPosition = player.Body.GetCellPositionAt(direction);
            var hasElevatorAtCurrent = world.HasElevatorAt(player.Body.CellPosition);
            var hasElevatorAtTarget = world.HasElevatorAt(targetCellPosition);
            var isTargetCellPassable = world.IsCellPassable(targetCellPosition);

            if (direction == Direction.Down) {
                return (hasElevatorAtCurrent || hasElevatorAtTarget) && isTargetCellPassable;
            }
            
            return hasElevatorAtCurrent && hasElevatorAtTarget && isTargetCellPassable;
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
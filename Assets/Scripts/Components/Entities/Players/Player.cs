using Core.Util;
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

        public void MoveTo(Direction direction) {
            Body.StartMoveTo(direction);
        }

        public void PushTo(Direction direction) {
            var targetCellPosition = Body.GetCellPositionAt(direction);
            var pushedBody = World.GetBodyAtCell(targetCellPosition);
            var pushSpeed = direction == Direction.Down ? pushedBody.FallSpeed : pushedBody.MoveSpeed;

            Body.StartMoveTo(direction, pushSpeed);
            pushedBody.StartMoveTo(direction, pushSpeed);
            pushedBody.FallOnce();
        }

        public bool CanWalk(Direction direction) {
            var targetCellPosition = Body.GetCellPositionAt(direction);

            return World.IsCellPassable(targetCellPosition);
        }

        public bool CanPush(Direction direction) {
            var targetCellPosition = Body.GetCellPositionAt(direction);
            var targetBody = World.GetBodyAtCell(targetCellPosition);
            var finalBodyPosition = Body.CellPosition + direction.AsCellPosition() * 2;
            var isFinalPositionPassable = World.IsCellEmpty(finalBodyPosition);

            return targetBody
                   && !targetBody.IsMoving
                   && targetBody.IsPushable
                   && isFinalPositionPassable;
        }

        public bool CanStomp() {
            var finalBodyPosition = Body.CellPosition + Direction.Down.AsCellPosition() * 2;
            
            return CanPush(Direction.Down) && !World.HasElevatorAt(finalBodyPosition);
        }

        public bool CanFly(Direction direction) {
            var targetCellPosition = Body.GetCellPositionAt(direction);
            var hasElevatorAtCurrent = World.HasElevatorAt(Body.CellPosition);
            var hasElevatorAtTarget = World.HasElevatorAt(targetCellPosition);
            var isTargetCellPassable = World.IsCellPassable(targetCellPosition);

            return hasElevatorAtCurrent && hasElevatorAtTarget && isTargetCellPassable;
        }
    }
}
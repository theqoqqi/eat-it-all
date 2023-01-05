using Components.Worlds;
using Core.Util;
using UnityEngine;

namespace Components.Entities {
    public class GridAlignedBody : EntityBehaviour {

        [SerializeField] private bool isPushable;

        public bool IsPushable => isPushable;

        [SerializeField] private float moveSpeed = 2.0f;

        public float MoveSpeed => moveSpeed;

        [SerializeField] private float fallSpeed = 2.0f;

        public float FallSpeed => fallSpeed;

        [SerializeField] private bool fallAlways;

        [SerializeField] private bool isSelfElevating;

        public bool IsSelfElevating => isSelfElevating;

        [SerializeField] private bool isPassable;

        public bool IsPassable => isPassable;

        [SerializeField] private bool passThroughBodies;

        private World world;

        private float currentMoveSpeed;

        private bool fallingEnabled;

        private Vector3 targetPosition;
        
        public Vector3Int TargetCellPosition => targetPosition.WorldToCell();

        private Vector3 Position {
            get => transform.position;
            set => transform.position = value;
        }

        public bool IsMoving => targetPosition != Position;
        
        public bool ShouldFall => fallingEnabled
                                  && world.IsCellPassable(GetCellPositionAt(Direction.Down), passThroughBodies)
                                  && !world.HasElevatorAt(CellPosition);
        
        public bool ShouldElevate => IsSelfElevating
                                     && world.IsCellPassable(GetCellPositionAt(Direction.Up), passThroughBodies)
                                     && world.HasElevatorAt(CellPosition)
                                     && world.HasElevatorAt(GetCellPositionAt(Direction.Up));

        protected override void Awake() {
            targetPosition = Position;
            fallingEnabled = fallAlways;
            world = FindObjectOfType<World>();
        }
        
        private void Update() {
            StepTowardsTargetPosition();
            
            if (!IsMoving && ShouldFall) {
                StartMoveTo(Direction.Down, fallSpeed);
            }

            if (!IsMoving && ShouldElevate) {
                StartMoveTo(Direction.Up);
            }
        }

        private void OnTargetPositionReached() {
            if (!ShouldFall && !fallAlways) {
                fallingEnabled = false;
            }
        }

        public void StartMoveTo(Direction direction) {
            StartMoveTo(direction, moveSpeed);
        }

        public void StartMoveTo(Direction direction, float moveSpeed) {
            StopMovement();

            currentMoveSpeed = moveSpeed;
            targetPosition = Position + direction.AsPosition();
        }

        public void FallOnce() {
            fallingEnabled = true;
        }

        private void StopMovement() {
            var snappedPosition = SnapPosition(Position);

            Position = snappedPosition;
            targetPosition = snappedPosition;
        }

        private static Vector3 SnapPosition(Vector3 v) {
            return v.WorldToCell().CellToWorld();
        }

        public Vector3Int GetCellPositionAt(Direction direction) {
            return CellPosition + direction.AsCellPosition();
        }

        private void StepTowardsTargetPosition() {
            var direction = targetPosition - Position;
            var speed = this.currentMoveSpeed * Time.deltaTime;

            if (direction.sqrMagnitude <= speed * speed) {
                Position = targetPosition;
                OnTargetPositionReached();
            }
            else {
                var velocity = Vector2.ClampMagnitude(direction, speed);
                Position += (Vector3) velocity;
            }
        }
    }
}
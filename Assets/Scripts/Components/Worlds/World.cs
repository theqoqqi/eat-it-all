using Components.Entities;
using UnityEngine;

namespace Components.Worlds {
    public class World : MonoBehaviour {

        [SerializeField] private TerrainTilemap terrainTilemap;

        [SerializeField] private EntityGrid entityGrid;

        public bool IsCellPassable(Vector3Int cellPosition, bool passThroughBodies) {
            return passThroughBodies
                    ? IsCellPassable(cellPosition)
                    : IsCellEmpty(cellPosition);
        }

        public bool IsCellPassable(Vector3Int cellPosition) {
            return terrainTilemap.IsCellEmpty(cellPosition)
                   && entityGrid.IsCellPassable(cellPosition)
                   && !entityGrid.IsCellReserved(cellPosition);
        }

        public bool IsCellEmpty(Vector3Int cellPosition) {
            return terrainTilemap.IsCellEmpty(cellPosition)
                   && entityGrid.IsCellEmpty(cellPosition)
                   && !entityGrid.IsCellReserved(cellPosition);
        }

        public GridAlignedBody GetBodyAtCell(Vector3Int cellPosition) {
            return entityGrid.GetBodyAtCell(cellPosition);
        }

        public bool HasElevatorAt(Vector3Int cellPosition) {
            return entityGrid.HasElevatorAt(cellPosition);
        }
    }
}
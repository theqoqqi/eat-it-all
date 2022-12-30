using UnityEngine;
using UnityEngine.Tilemaps;

namespace Components.Worlds {
    public class TerrainTilemap : MonoBehaviour {
        
        [SerializeField] private Tilemap tilemap;

        public bool IsCellEmpty(Vector3Int cellPosition) {
            return !tilemap.HasTile(cellPosition * 2);
        }
    }
}
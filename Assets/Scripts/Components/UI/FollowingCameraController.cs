using System;
using UnityEngine;

namespace Components.UI {
    public class FollowingCameraController : MonoBehaviour {

        [SerializeField] private new Camera camera;

        [SerializeField] private GameObject target;

        [SerializeField] private Vector2 padding;

        private int minVisibleSquareSize = 8;
        private int squareSizeInPixels = 32;
        private int pixelSize;
        
        private Vector2 minBounds;
        private Vector2 maxBounds;
        private float zOffset;
        private Vector3 position;

        private void Awake() {
            var viewportWidth = camera.orthographicSize;
            var viewportHeight = viewportWidth / Screen.width * Screen.height;
            var minScreenSize = Math.Min(Screen.width, Screen.height);

            minBounds = new Vector2(-viewportWidth / 2 + padding.x, -viewportHeight / 2 + padding.y);
            maxBounds = new Vector2(viewportWidth / 2 - padding.x, viewportHeight / 2 - padding.y);
            zOffset = camera.transform.position.z;

            var minAreaSizeInPixels = squareSizeInPixels * minVisibleSquareSize;
            pixelSize = (int) Mathf.Floor((float) minScreenSize / minAreaSizeInPixels);
            var pixelRatio = minScreenSize / ((float) minAreaSizeInPixels * pixelSize);
            
            camera.orthographicSize = pixelRatio * minVisibleSquareSize / 2f;
        }

        private void LateUpdate() {
            var targetPosition = target.transform.position;
            var currentMinBounds = targetPosition + (Vector3) minBounds;
            var currentMaxBounds = targetPosition + (Vector3) maxBounds;

            position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
            position.x = Mathf.Clamp(position.x, currentMinBounds.x, currentMaxBounds.x);
            position.y = Mathf.Clamp(position.y, currentMinBounds.y, currentMaxBounds.y);
            position.z = zOffset;

            var roundBy = squareSizeInPixels * pixelSize;
            var roundedPosition = new Vector3(
                    Mathf.Floor(position.x * roundBy) / roundBy,
                    Mathf.Floor(position.y * roundBy) / roundBy,
                    position.z
            );

            transform.position = roundedPosition;
        }
    }
}
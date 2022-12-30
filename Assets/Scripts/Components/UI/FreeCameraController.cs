using UnityEngine;

namespace Components.UI {
    public class FreeCameraController : MonoBehaviour {

        [SerializeField] private new Camera camera;

        [SerializeField] private float dragSpeed = 1;

        [SerializeField] private float zoomSpeed = 1;

        [SerializeField] private float minSize = 5;

        [SerializeField] private float maxSize = 50;

        private bool isDragging;

        private Vector2 mouseClickPos;

        private Vector3 Position {
            get => transform.position;
            set => transform.position = value;
        }

        private float Size {
            get => camera.orthographicSize;
            set => camera.orthographicSize = value;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse2) && !isDragging) {
                mouseClickPos = camera.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }

            if (isDragging) {
                var mouseCurrentPos = (Vector2) camera.ScreenToWorldPoint(Input.mousePosition);
                var distance = mouseCurrentPos - mouseClickPos;

                transform.position += new Vector3(-distance.x, -distance.y, 0) * dragSpeed;
                mouseClickPos = camera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetKeyUp(KeyCode.Mouse2)) {
                mouseClickPos = default;
                isDragging = false;
            }

            var mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);

            ZoomAt(mouseWorldPosition, Size * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
        }

        private void ZoomAt(Vector3 zoomTowards, float amount) {
            ZoomAtPositionToSize(zoomTowards, Size - amount);
        }

        private void ZoomAtPositionToSize(Vector3 zoomTowards, float newSize) {
            newSize = Mathf.Clamp(newSize, minSize, maxSize);
            var zoomedBy = Size - newSize;
            var multiplier = 1.0f / Size * zoomedBy;

            Position += (zoomTowards - Position) * multiplier;
            Size = newSize;
        }
    }
}
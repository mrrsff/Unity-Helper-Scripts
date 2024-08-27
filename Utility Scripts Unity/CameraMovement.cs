using UnityEngine;

namespace AlphaGame.HelperModules
{
    public class CameraMovement : MonoBehaviour
    {
        private float cameraOffset = 1f;
        public float moveSpeed = 20;
        public float lookSpeed = 10;
        public int shiftMultiplier = 2;
        public float zoomSpeed = 1;
        public BoxCollider boundary;
        public Camera _camera;
        private void LateUpdate() {
            if(Input.GetKeyDown(KeyCode.LeftShift)){
                moveSpeed *= shiftMultiplier;
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift)){
                moveSpeed /= shiftMultiplier;
            }
            // Get input for camera movement and zoom
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float zoom = Input.GetAxis("Mouse ScrollWheel");

            // Move camera
            Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
            float _cameraAngle = _camera.transform.eulerAngles.y;
            movement = Quaternion.Euler(0, _cameraAngle, 0) * movement;
            transform.position = ClampPosition(transform.position + movement);

            // Zoom in and out
            if(zoom != 0)
            {
                float zMovement = zoom * zoomSpeed * 100f * Time.deltaTime;
                Vector3 _newPosition = _camera.transform.position + _camera.transform.forward * zMovement;
                if(boundary.bounds.Contains(_newPosition))
                {
                    _camera.transform.position = _newPosition;
                }
            }
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                // Rotate camera
                Vector3 rotation = new Vector3(-mouseY, mouseX, 0) * lookSpeed * 100f * Time.deltaTime;
                _camera.transform.RotateAround(transform.position, Vector3.up, rotation.y);
                _camera.transform.RotateAround(transform.position, _camera.transform.right, rotation.x);
            }
        }
        private Vector3 ClampPosition(Vector3 position){
            float x = Mathf.Clamp(position.x, boundary.bounds.min.x, boundary.bounds.max.x);
            float y = Mathf.Clamp(position.y, boundary.bounds.min.y, boundary.bounds.max.y);
            float z = Mathf.Clamp(position.z, boundary.bounds.min.z, boundary.bounds.max.z);
            return new Vector3(x, y, z);
        }
        private Vector3 ClampPosition(float x, float y, float z){
            float x1 = Mathf.Clamp(x, boundary.bounds.min.x, boundary.bounds.max.x);
            float y1 = Mathf.Clamp(y, boundary.bounds.min.y, boundary.bounds.max.y);
            float z1 = Mathf.Clamp(z, boundary.bounds.min.z, boundary.bounds.max.z);
            return new Vector3(x1, y1, z1);
        }
    }
}
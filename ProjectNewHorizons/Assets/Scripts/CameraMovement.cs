using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector2 movementScalar = new(16, 8);
    [SerializeField] private Vector2 _borderClamp = new(2000,1080);
    private Vector3 _cameraPosition;
    private Vector3 _startTouchInWorldSpace;
    private void Start()
    {
        _cameraPosition = transform.position;
    }
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // do things with touch.position
            Debug.Log(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchInWorldSpace = GetWorldPosition(touch.position);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                var _currentWorldPosition = GetWorldPosition(touch.position);
                var _worldDifference = _currentWorldPosition - _startTouchInWorldSpace;

                _cameraPosition = transform.position;

                var _newCameraPosition = _cameraPosition + new Vector3(-_worldDifference.x, 0, -_worldDifference.z);

                _newCameraPosition.x = Mathf.Clamp(
                    _newCameraPosition.x,
                    -_borderClamp.x / movementScalar.x / 2f,
                    _borderClamp.x / movementScalar.x / 2f
                );

                _newCameraPosition.z = Mathf.Clamp(
                    _newCameraPosition.z,
                    -_borderClamp.y / movementScalar.y / 2f - 20,
                    _borderClamp.y / movementScalar.y / 2f - 20
                );

                transform.position = _newCameraPosition;
            }


            //logging
            Debug.Log($"finger {touch.fingerId} tapped {touch.tapCount} times");
            Debug.Log($"time since last touch value change = {touch.deltaTime}");
            Debug.Log($"position since last change in pixel cords = {touch.deltaPosition}");
            Debug.Log($"the phase of the touch = {touch.phase}");
        }
    }

    private Vector3 GetWorldPosition(Vector2 screenPoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit))
        {
            return hit.point;
        }

        Debug.LogWarning("Raycast missed!");
        return _startTouchInWorldSpace;
    }
}

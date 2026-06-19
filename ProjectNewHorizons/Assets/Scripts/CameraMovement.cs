using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //[SerializeField] private float movementScalar = 4;
    [SerializeField] private Vector2 movementScalar = new(8, 8);
    private Vector3 _cameraPosition;
    private void Start()
    {
        _cameraPosition = transform.position;
    }
    private void Update()
    {
//#if    UNITY_ANDROID || UNITY_IOS
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

            // do things with touch.position
            Debug.Log(touch.position);
            //1600 or 1920
            var camPos = _cameraPosition + new Vector3((touch.position.x - 2000 / 2) / movementScalar.x, 0, (touch.position.y - 1080 / 2) / movementScalar.y);
            transform.position = camPos;
            }
//#elif UNITY_STANDALONE
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            // do things with Input.mousePosition
        }
//#endif
    }

    // !!! USE THIS CODE BECAUSE IT'S OBVIOUSLY BETTER !!!
    // 
    // public void SelectPosition(InputAction.CallbackContext context)
    // {
    //     Vector2 selectedPosition = context.ReadValue<Vector2>();
    //     
    //     // do things with selectedPosition
    // }
}

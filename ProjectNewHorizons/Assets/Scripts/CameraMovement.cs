using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementScalar = 4;
    private void Update()
    {
//#if    UNITY_ANDROID || UNITY_IOS
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

            // do things with touch.position
            Debug.Log(touch);
            var camPos = new Vector3((touch.position.x - 1600 / 2) / movementScalar, 0, -10);
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

using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private void Update()
    {
//#if    UNITY_ANDROID || UNITY_IOS
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

            // do things with touch.position
            Debug.Log(touch);
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

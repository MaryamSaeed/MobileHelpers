using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// this class is a standalone class that handles the touch gestures (swipe , pinch)
/// </summary>
public class TouchGesturesHandler : MonoBehaviour
{
    [Tooltip("factor of swipe speed")]
    public float SwipeSpeed;
    [Tooltip(" minimum swipe value ")]
    public float SwipeThreshold;
    [Tooltip(" Clamping angle of pitch ")]
    public float ClampingAngle;
    [Tooltip("factor of pinch speed")]
    public float PinchZoomSpeed;
    [Tooltip(" Maximum allowable zoom")]
    public float Maxzoom = 100;
    [Tooltip(" Minimun allowable zoom")]
    public float Minzoom = 20;
    // Update is called once per frame
    private void Update()
    {
        switch (Input.touchCount)
        {
            case 0:
                if (Application.platform != RuntimePlatform.IPhonePlayer|| Input.touchSupported == false)
                {
                    MouseHandlers();
                }
                break;
            case 1:
                //swipe when only one touch 
                RotateOnSwipeDirection();
                break;
            case 2:
                //pinch when two touches are detected
                ZoomOnPinch();
                break;
        }
    }
    /// <summary>
    /// this function decides in which direction the camera will rotate
    /// when a swipe is detected.
    /// </summary>
    private void RotateOnSwipeDirection()
    {
        Touch swipe = Input.GetTouch(0);
        // Find the position in the previous frame
        Vector2 swipeprevpos = swipe.position - swipe.deltaPosition;
        //get the distance vector
        Vector2 swipedistance = swipe.position - swipeprevpos;
        if(Mathf.Abs(swipedistance.x) > Mathf.Abs(swipedistance.y)&& Mathf.Abs(swipedistance.x) >= SwipeThreshold)
        {
            //apply yaw rotation
            RotateOnSwipeAlongXaxis(swipe);
        }
        else if(Mathf.Abs(swipedistance.y)> Mathf.Abs(swipedistance.x) && Mathf.Abs(swipedistance.y) >= SwipeThreshold)
        {
            //apply pitch rotation
            RotateOnSwipeAlongYaxis(swipe);
        }
    }
    /// <summary>
    /// this function applies yaw rotation when a horizontal swipe is detected
    /// along the screen's X-axis.
    /// </summary>
    /// <param name="swipe">a refrence to the detcted swipe</param>
    private void RotateOnSwipeAlongXaxis(Touch swipe)
    {
        //rotate camera in the same direction of swipe around
        var yaw = - swipe.deltaPosition.x * SwipeSpeed * Time.deltaTime;
        Camera.main.transform.Rotate(Vector3.up, yaw);
        Vector3 rot = Camera.main.transform.eulerAngles;
        rot.z = 0;
        Camera.main.transform.eulerAngles = rot;
    }
    /// <summary>
    /// this function applies pitch rotation when a vertical swipe is detected
    /// along the screen's Y-axis.
    /// </summary>
    /// <param name="swipe"></param>
    private void RotateOnSwipeAlongYaxis(Touch swipe)
    {
        //rotate camera in the same direction of swipe around
        var pitch = swipe.deltaPosition.y * SwipeSpeed * Time.deltaTime;
        Camera.main.transform.Rotate(Vector3.right, pitch);
        Camera.main.transform.rotation = ClampRotationAroundXAxis(Camera.main.transform.rotation);
        Vector3 rot = Camera.main.transform.eulerAngles;
        rot.z = 0;
        Camera.main.transform.eulerAngles = rot;
    }
    /// <summary>
    /// this function is used the clamp the rotation between max and min values 
    /// </summary>
    /// <param name="q">the quatrenion u wish to clamp</param>
    /// <returns> the resulting quatrenion after clamping</returns>
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleX = 2 * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -ClampingAngle, ClampingAngle);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }
    /// <summary>
    /// this function detects the pinch gesture and alters the FOV
    /// of the Camera according to the pinch amount.
    /// </summary>
    private void ZoomOnPinch()
    {
        Touch touch1st = Input.GetTouch(0);
        Touch touch2nd = Input.GetTouch(1);
        // Find the position in the previous frame
        Vector2 prevPos1stTouch = touch1st.position - touch1st.deltaPosition;
        Vector2 prevPos2ndTouch = touch2nd.position - touch2nd.deltaPosition;
        // Find the magnitude of the distanc between the touches in each frame.
        float prevDistanceBtwtouches = (prevPos1stTouch - prevPos2ndTouch).magnitude;
        float currDistanceBtwtouches = (touch1st.position - touch2nd.position).magnitude;
        //amount of pinch 
        float rateOfChangeInDistance = prevDistanceBtwtouches - currDistanceBtwtouches;
        //clamp the pich value between upperbound and lower bound
        float zoomvalue = Camera.main.fieldOfView + rateOfChangeInDistance * PinchZoomSpeed *Time.deltaTime;
        Camera.main.fieldOfView = Mathf.Clamp(zoomvalue,Minzoom,Maxzoom);
    }
    /// <summary>
    /// this function does all the bove for editor and desktop
    /// using mouse and scroll wheel
    /// </summary>
    private void MouseHandlers()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            if (Mathf.Abs(Input.GetAxis("Mouse X")) >= Mathf.Abs(Input.GetAxis("Mouse Y")) && Mathf.Abs(Input.GetAxis("Mouse X")) >= SwipeThreshold)
            {
                var yaw = -Input.GetAxis("Mouse X") * Time.deltaTime;
                Camera.main.transform.Rotate(Vector3.up, yaw);
                Vector3 rot = Camera.main.transform.rotation.eulerAngles;
                rot.z = 0;
                Camera.main.transform.eulerAngles = rot;
            }
            else if (Mathf.Abs(Input.GetAxis("Mouse Y")) > Mathf.Abs(Input.GetAxis("Mouse X")) && Mathf.Abs(Input.GetAxis("Mouse Y")) >= SwipeThreshold)
            {

                var pitch = Input.GetAxis("Mouse Y") * Time.deltaTime;
                Camera.main.transform.Rotate(Vector3.right, pitch);
                Camera.main.transform.rotation = ClampRotationAroundXAxis(Camera.main.transform.rotation);
                Vector3 rot = Camera.main.transform.eulerAngles;
                rot.z = 0;
                Camera.main.transform.eulerAngles = rot;
            }
        }
        float Scrlwhel = Input.GetAxis("Mouse ScrollWheel");
        if (Scrlwhel != 0)
        {
            float zoomvalue = Camera.main.fieldOfView - (Scrlwhel*4);
            Camera.main.fieldOfView = Mathf.Clamp(zoomvalue, Minzoom,Maxzoom);
        }
    }

}

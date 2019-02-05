using System.Collections;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// this class does all the manipulation needed when transitioning the camera between view points
/// and checkpoints in an animated fashion.
/// </summary>
public class CameraManipulator : MonoBehaviour
{
    [Tooltip("the value that indecatses how fast the animation should be")]
    public  float TransitionSpeed;
    [Tooltip("assigns the animation curve used in animating camera move")]
    public  AnimationCurve TweeningCurve;
    [Tooltip("what to be done when transition starts")]
    public  UnityEvent OnTransitionStarted;
    [Tooltip("what to be done when transition Ends")]
    public  UnityEvent OnTransitionEnded;
    private Coroutine Transition; 
    private bool Inprogress = false;
    
    /// <summary>
    ///Manipulate the Camera's position and orientation
    /// </summary>
    /// <param name="targetview">target views transform</param>
    public  void CameraMoveAndRotateTo(Transform targetview)
    {
        if (!Inprogress)
        {
            Inprogress = !Inprogress;
            Transition= StartCoroutine(AnimateCameraManipulation(targetview, TweeningCurve));
        }
        else
        {
            //to do handle in progress case 
            StopCoroutine(Transition);
            Transition = StartCoroutine(AnimateCameraManipulation(targetview, TweeningCurve));
        }
    }
   /// <summary>
   /// Manipulates the camera's position only while keeping
   /// its original orientation
   /// </summary>
   /// <param name="newpos">the camera's target position</param>
    public  void CameraMoveTo(Vector3 newpos)
    {
        if (!Inprogress)
        {
            Inprogress = !Inprogress;
            Transition = StartCoroutine(AnimateCameraTranslation(newpos, TweeningCurve));
        }
        else
        {
            StopCoroutine(Transition);
            Transition = StartCoroutine(AnimateCameraTranslation(newpos, TweeningCurve));
        }
    }
    /// <summary>
    /// animate the camera's position transition
    /// </summary>
    /// <param name="targetpos">target position</param>
    /// <param name="curve">the animation curve</param>
    /// <returns></returns>
    private  IEnumerator AnimateCameraTranslation(Vector3 targetpos, AnimationCurve curve)
    {
        float curvetimer = 0.0f;
        float curveamount = 0.0f;
        var camparent = Camera.main.transform.parent;
        while (curvetimer < 1)
        {
            curveamount = curve.Evaluate(curvetimer);
            camparent.position = Vector3.Lerp(camparent.position,targetpos,curveamount);
            curvetimer += Time.deltaTime * TransitionSpeed;
            yield return null;
        }
        Inprogress = !Inprogress;
    }
    /// <summary>
    /// animate the camera's position and orientation transition
    /// </summary>
    /// <param name="targetview">the camera'a target postion and orientation</param>
    /// <param name="curve">the animation curve</param>
    /// <returns></returns>
    private   IEnumerator AnimateCameraManipulation(Transform targetview, AnimationCurve curve)
    {
        OnTransitionStarted.Invoke();
        float curvetimer = 0.0f;
        float curveamount = 0.0f;
        var camparent = Camera.main.transform.parent;
        while (curvetimer < 1)
        {
            curveamount = curve.Evaluate(curvetimer);
            camparent.position = Vector3.Lerp(camparent.position, targetview.position, curveamount);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, targetview.rotation, curveamount);
            curvetimer += Time.deltaTime * TransitionSpeed;
            yield return null;
        }
        OnTransitionEnded.Invoke();
        Inprogress = !Inprogress;
    }
}

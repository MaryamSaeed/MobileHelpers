using UnityEngine.Events;
using UnityEngine;
/// <summary>
/// this class is responsible for the fading effect on the camera when going up or doen
/// </summary>
public class CrossFadePanel : MonoBehaviour
{
    //event connected with the fade animation eventso that we switch position upon low opacity
    public static UnityEvent SwitchPosition = new UnityEvent();
    public static GameObject BlackImage;
    private static Animator CrossFadeAnim;//Animator fading effect
	// Use this for initialization
	void Start ()
    {
        CrossFadeAnim = GetComponent<Animator>();
        BlackImage = transform.GetChild(0).gameObject;
        BlackImage.SetActive(false);
	}
    /// <summary>
    /// this function will be called from the animation event to invoke the switch position event
    /// </summary>
    public void FireAnimationEvent()
    {
        SwitchPosition.Invoke();
    }
    //function that triggers the animation when going up or down
    public static void PlayCrossFadeAnim()
    {
        BlackImage.SetActive(true);
        CrossFadeAnim.SetTrigger("play");
    }
}

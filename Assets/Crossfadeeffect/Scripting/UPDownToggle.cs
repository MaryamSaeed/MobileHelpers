using UnityEngine;
public class UPDownToggle :InteractiveObject
{
    [Tooltip("the target transform")]
    public Transform Target;
    private bool Subscribe = false; // subscribtion of animation event
    protected override void ApplyAction()
    {
        if (!Subscribe)
        {
            CrossFadePanel.SwitchPosition.AddListener(SwitchtoTarget);
            Subscribe = !Subscribe;
        }
        CrossFadePanel.PlayCrossFadeAnim();
    }
    /// <summary>
    /// switches from the current transform to the target while animating
    /// </summary>
    private void SwitchtoTarget()
    {
        Camera.main.transform.parent.position = Target.position;
        Camera.main.transform.rotation = Target.rotation;
        Camera.main.fieldOfView = 60;
        if (Subscribe)
        {
            CrossFadePanel.SwitchPosition.RemoveListener(SwitchtoTarget);
            Subscribe = !Subscribe;
        }
    }
}

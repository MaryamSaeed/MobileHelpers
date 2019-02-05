using UnityEngine;
/// <summary>
/// this class is responsible for translating the camera to a specific target position 
/// identefied by the target.
/// </summary>
public class CheckPoint : InteractiveObject
{
    [Tooltip("the transform that indecates the target position")]
    public Vector3 Target;
    [Tooltip("the hight at which the camera above the checkpoint")]
    public float CamHight;
    [Tooltip("Refrence to the active Camera Manipulator class")]
    protected  CameraManipulator CamMan;

    //CheckPoint initialization
    public void Start()
    {
        CamMan = FindObjectOfType<CameraManipulator>();
        Target = transform.position;
        Target.y = transform.position.y + CamHight;

    }
    /// <summary>
    /// translate the camera to the target postion upon clicking this check point
    /// </summary>
    protected override void ApplyAction()
    {
        CamMan.CameraMoveTo(Target);
    }
}


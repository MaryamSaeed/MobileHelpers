using UnityEngine;
/// <summary>
/// this class performs the External View points routines
/// </summary>
public class ViewPoint : CheckPoint
{
    private CameraManipulator CamManip;
    protected override void ApplyAction()
    {
        if (CamManip == null)
            CamManip = FindObjectOfType<CameraManipulator>();
        CamManip.CameraMoveAndRotateTo(gameObject.transform);
    }
}

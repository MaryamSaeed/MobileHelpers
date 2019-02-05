using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// this class describes the behavior of interactive objects in the scene
/// </summary>
public abstract class InteractiveObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool PressedDown = false;// a flag thet indecates that this object has been pressed
    public UnityEvent OnPressed = new UnityEvent();// what to be done when this object is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        PressedDown = true;
        Invoke("Unpress", 0.2f);//unpress the button after a certain time interval 
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (PressedDown && !eventData.dragging)
        {
            OnPressed.Invoke();
            ApplyAction();
        }
        Unpress();
    }
    /// <summary>
    /// unpressing the button inorder to defferentiate between the selection touch , swipe and pinch
    /// </summary>
    private void Unpress()
    {
        PressedDown = false;
    }
    /// <summary>
    /// apply action will be overrided by the chiled class
    /// </summary>
    protected abstract void ApplyAction();
}

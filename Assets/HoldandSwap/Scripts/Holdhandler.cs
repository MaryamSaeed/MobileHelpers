using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class Holdhandler : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public UnityAction<GameObject> GotSelected;
    private SwapHandler swhandler;
    private bool SwapMode = false;
    private int holdtime = 1;//seconds
    private float dwntime = 0;
    private float uptime = 0;
    // Start is called before the first frame update
    private void Awake()
    {

    }
    private void Start()
    {
        if (swhandler == null)
        {
            swhandler = FindObjectOfType<SwapHandler>();
            swhandler.ActivateSwapMode.AddListener(OnSwapActivated);
            swhandler.DeActivateSwapMode.AddListener(OnSwapDeActivated);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dwntime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        uptime = Time.time;
        var deltati = uptime - dwntime;
        if (deltati < holdtime)
        {
            if (SwapMode)
            {
                GotSelected(gameObject);
            }
            else
            {

            }
        }
        else
        {
            Debug.Log("hold detected");
            if(!SwapMode)
            GotSelected(gameObject);
        }
        dwntime = 0;
        uptime = 0;
    }
    private void OnSwapActivated()
    {
        SwapMode = true;
    }
    private void OnSwapDeActivated()
    {
        SwapMode = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class SwapHandler : MonoBehaviour
{
    public UnityEvent ActivateSwapMode = new UnityEvent();
    public UnityEvent DeActivateSwapMode = new UnityEvent();
    private GameObject replacedObject = null;
    private GameObject replacementObject = null;
 
    private Holdhandler[] holdables;
    // Start is called before the first frame update
    void Start()
    {
        holdables = FindObjectsOfType<Holdhandler>();
        for (int i = 0; i < holdables.Length; i++)
        {
            holdables[i].GotSelected = OnGotselected;
        }
    }
    
    private void OnGotselected(GameObject selected)
    {
        if (replacedObject == null)
        {
            ActivateSwapMode.Invoke();
            replacedObject = selected;
        }
        else if (replacementObject == null)
        {
            replacementObject = selected;
        }
        if (replacedObject && replacementObject)
        {
            SwapObjects();
            DeActivateSwapMode.Invoke();
            InitObjects();
        }
    }
    private void SwapObjects()
    {
        //loading temp
        var temppos = replacedObject.transform.position;
        var tempscale = replacedObject.transform.localScale;
        //assiening replacement values to replaced
        replacedObject.transform.position = replacementObject.transform.position;
        replacedObject.transform.localScale = replacementObject.transform.localScale;
        //assigning temp values to replacement
        replacementObject.transform.position = temppos;
        replacementObject.transform.localScale = tempscale;
    }
    private void InitObjects()
    {
        replacementObject = null;
        replacedObject = null;

    }
}

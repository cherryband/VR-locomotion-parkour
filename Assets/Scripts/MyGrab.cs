using UnityEngine;

public class MyGrab : MonoBehaviour
{
    public SelectionTaskMeasure selectionTaskMeasure;
    public OVRInput.Controller controller;
    //private float triggerValue;
    //private bool isInCollider;
    //private bool isSelected;
    //private GameObject selectedObj;

    /*
    void Update()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        if (isInCollider)
        {
            if (!isSelected && triggerValue > 0.95f)
            {
                isSelected = true;
                selectedObj.transform.parent.transform.parent = transform;
            }
            else if (isSelected && triggerValue < 0.95f)
            {
                isSelected = false;
                selectedObj.transform.parent.transform.parent = null;
            }
        }
    }
    */

    void OnTriggerEnter(Collider other)
    {
        float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        if (other.gameObject.CompareTag("selectionTaskStart"))
        {
            if (!selectionTaskMeasure.isCountdown)
            {
                selectionTaskMeasure.isTaskStart = true;
                selectionTaskMeasure.StartOneTask();
            }
        }
        else if (other.gameObject.CompareTag("done") && triggerValue < 0.15f)
        {
            selectionTaskMeasure.isTaskStart = false;
            selectionTaskMeasure.EndOneTask();
        }
    }

    /*
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = false;
            selectedObj = null;
        }
    }
    */
}

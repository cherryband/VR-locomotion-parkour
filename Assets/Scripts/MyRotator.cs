using UnityEngine;

public class MyRotator : MonoBehaviour
{
    public OVRInput.Controller leftController, rightController;
    public SelectionTaskMeasure selectionTaskMeasure;
    public GameObject hmd;
    [SerializeField] private bool isIndexTriggerDown;
    [SerializeField] private float leftTriggerValue;
    [SerializeField] private float rightTriggerValue;

    private float triggerValue;
    private bool isReady = false;
    private GameObject selectedObj;
    private Vector3 positionOffset;
    private Quaternion rotationOffset;
    private Vector3 startPos1;
    private Vector3 startPos2;
    private Vector3 startPosition;
    private Quaternion startRotation;

    // Update is called once per frame
    void Update()
    {
        if (!selectionTaskMeasure.isTaskStart)
        {
            isReady = false;
            return;
        }
        if (!isReady)
        {
            selectedObj = selectionTaskMeasure.objectT;
            startPosition = selectedObj.transform.position;
            startRotation = selectedObj.transform.rotation;
            rotationOffset = Quaternion.identity;
            isReady = true;
        }

        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, leftController);
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, rightController);

        if (leftTriggerValue > 0.95f && rightTriggerValue > 0.95f)
        {
            Vector3 pos1 = OVRInput.GetLocalControllerPosition(leftController);
            Vector3 pos2 = OVRInput.GetLocalControllerPosition(rightController);
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos1 = pos1;
                startPos2 = pos2;
                startPosition = selectedObj.transform.position;
            }
            positionOffset = ((startPos1 - pos1) + (startPos2 - pos2))/2;
            rotationOffset = Quaternion.FromToRotation(startPos2-startPos1, pos2-pos1);
            selectedObj.transform.position = startPosition - positionOffset;
            selectedObj.transform.rotation = rotationOffset * startRotation;
        }
        else if (leftTriggerValue < 0.95f && rightTriggerValue < 0.95f)
        {
            if (isIndexTriggerDown)
            {
                isIndexTriggerDown = false;
                startPosition -= positionOffset;
                startRotation = rotationOffset * startRotation;
            }
            positionOffset = Vector3.zero;
            rotationOffset = Quaternion.identity;
        }

    }
}

using UnityEngine;
using System;

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script.
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    public float translationGain = 240f;
    [Range(0, 2)] public float scaleGain = 0.7f;
    public GameObject hmd;
    public GameObject environmentRoot;
    public GameObject player;
    [SerializeField] private float leftTriggerValue;
    [SerializeField] private float rightTriggerValue;
    [SerializeField] private Vector3 startPos1;
    [SerializeField] private Vector3 startPos2;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float scaleOffset = 1f;
    [SerializeField] private bool isIndexTriggerDown;


    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public SelectionTaskMeasure selectionTaskMeasure;
    public string stage;
    private Vector3 initialWorldPosition;
    private Vector3 initialPlayerPosition;
    private float scaleDelta = 0f;
    private Vector3 positionOffset;
    private float rotationCos = 0f;
    private Space cameraCoordSpace;
    private float previousAngle = 0f;

    void Start()
    {
        initialWorldPosition = environmentRoot.transform.position;
        initialPlayerPosition = player.transform.position;
        startPosition = transform.position;
        cameraCoordSpace = Space.Self;
    }

    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.
        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController);
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);


        if (leftTriggerValue > 0.95f && rightTriggerValue > 0.95f)
        {
            Vector3 pos1 = OVRInput.GetLocalControllerPosition(leftController);
            Vector3 pos2 = OVRInput.GetLocalControllerPosition(rightController);
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos1 = pos1;
                startPos2 = pos2;
            }
            scaleDelta = (pos1 - pos2).magnitude - (startPos1 - startPos2).magnitude;
            positionOffset = (startPos1 - pos1) + (startPos2 - pos2);
            //rotationCos = Vector3.Dot(Vector3.Normalize(startPos2 - startPos1), Vector3.Normalize(pos2 - pos1));

            //if (positionOffset.magnitude > movementLockThreshold) scaleDelta = 0f;
            //else if (scaleDelta > scaleLockThreshold) positionOffset = Vector3.zero;
            Debug.DrawRay(startPos1, pos1, Color.red, 0.2f);
            Debug.DrawRay(startPos2, pos2, Color.red, 0.2f);
        }
        else if (leftTriggerValue < 0.95f && rightTriggerValue < 0.95f)
        {
            if (isIndexTriggerDown)
            {
                isIndexTriggerDown = false;
                scaleOffset *= 1 + scaleDelta*scaleGain;
                startPosition += positionOffset*translationGain/scaleOffset;
                Debug.Log("scaleOffset = " + scaleOffset.ToString());
            }
            positionOffset = Vector3.zero;
            scaleDelta = 0f;
            if (scaleOffset < 0.01f) scaleOffset = 0.01f;
            previousAngle = 0f;
        }
        float curScale = scaleOffset * (1 + scaleDelta*scaleGain);
        if (curScale < 0.01f) curScale = 0.01f;
        transform.position = startPosition*curScale + positionOffset*translationGain;

        environmentRoot.transform.localScale = Vector3.one * curScale;
        environmentRoot.transform.position = initialWorldPosition * curScale;

        player.transform.localEulerAngles = Vector3.Scale(hmd.transform.localEulerAngles, Vector3.up);
        player.transform.position = initialPlayerPosition + hmd.transform.position + Vector3.down*1.2f;

        /*
        if (isIndexTriggerDown)
        {
            double angleRad =  Math.Acos(rotationCos);
            float angle = (float) (angleRad * 180 / Math.PI);
            // Source - https://stackoverflow.com/a
            // Posted by Aziz, modified by community. See post 'Timeline' for change history
            // Retrieved 2026-01-28, License - CC BY-SA 2.5
            environmentRoot.transform.Rotate(0, angle-previousAngle, 0, cameraCoordSpace);
            previousAngle = angle;
        }
        */


        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                transform.position = parkourCounter.currentRespawnPos;
                scaleOffset = 1f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("objectInteractionTask"))
        {
            //selectionTaskMeasure.isTaskStart = true;
            selectionTaskMeasure.scoreText.text = "";
            selectionTaskMeasure.partSumErr = 0f;
            selectionTaskMeasure.partSumTime = 0f;
            selectionTaskMeasure.ShowStartPanel();
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("banner"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
    }
}

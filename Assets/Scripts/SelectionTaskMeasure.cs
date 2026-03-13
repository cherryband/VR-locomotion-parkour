using System.Collections;
using UnityEngine;
using TMPro;
public class SelectionTaskMeasure : MonoBehaviour
{
    public GameObject targetT;
    public GameObject targetTPrefab;
    Vector3 targetTStartingPos;
    public GameObject objectT;
    public GameObject objectTPrefab;
    Vector3 objectTStartingPos;

    public GameObject task1StartPanel, task2StartPanel, task3StartPanel;
    public GameObject task1DonePanel, task2DonePanel, task3DonePanel;
    public TMP_Text startPanel1Text, startPanel2Text, startPanel3Text;
    public TMP_Text scoreText;
    public int completeCount;
    public bool isTaskStart;
    public bool isTaskEnd;
    public bool isCountdown;
    public Vector3 manipulationError;
    public float taskTime;
    public GameObject task1UI, task2UI, task3UI;
    public ParkourCounter parkourCounter;
    public DataRecording dataRecording;
    public int part;
    public float partSumTime;
    public float partSumErr;

    private TMP_Text startPanelText;
    private GameObject taskUI, taskStartPanel, donePanel;


    // Start is called before the first frame update
    void Start()
    {
        parkourCounter = GetComponent<ParkourCounter>();
        dataRecording = GetComponent<DataRecording>();
        part = 1;
        scoreText.text = "Part" + part.ToString();
        task1DonePanel.SetActive(false);
        task2DonePanel.SetActive(false);
        task3DonePanel.SetActive(false);
        task1StartPanel.SetActive(false);
        task2StartPanel.SetActive(false);
        task3StartPanel.SetActive(false);
        UpdateCurrentTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTaskStart)
        {
            // recording time
            taskTime += Time.deltaTime;
        }

        if (isCountdown)
        {
            taskTime += Time.deltaTime;
            startPanelText.text = (3.0 - taskTime).ToString("F1");
        }
    }

    public void ShowStartPanel()
    {
        taskStartPanel.SetActive(true);
        Debug.Log("ShowStartPanel()");
    }

    private void UpdateCurrentTarget()
    {
        if (part == 1)
        {
            taskUI = task1UI;
            startPanelText = startPanel1Text;
            taskStartPanel = task1StartPanel;
            donePanel = task1DonePanel;
        }
        else if (part == 2)
        {
            taskUI = task2UI;
            startPanelText = startPanel2Text;
            taskStartPanel = task2StartPanel;
            donePanel = task2DonePanel;
        }
        else
        {
            taskUI = task3UI;
            startPanelText = startPanel3Text;
            taskStartPanel = task3StartPanel;
            donePanel = task3DonePanel;
        }
        Debug.Log("taskPart = " + part.ToString());
    }

    public void StartOneTask()
    {
        UpdateCurrentTarget();

        taskTime = 0f;
        taskStartPanel.SetActive(false);
        donePanel.SetActive(true);
        objectTStartingPos = taskUI.transform.position + taskUI.transform.forward * 0.5f + taskUI.transform.up * 0.75f;
        targetTStartingPos = taskUI.transform.position + taskUI.transform.forward * 0.75f + taskUI.transform.up * 1.2f;
        objectT = Instantiate
        (
            objectTPrefab,
            objectTStartingPos,
            new Quaternion
            (
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f)
            ),
            taskUI.transform
        );
        targetT = Instantiate
        (
            targetTPrefab,
            targetTStartingPos,
            new Quaternion
            (
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f)
            ),
            taskUI.transform
        );
    }

    public void EndOneTask()
    {
        donePanel.SetActive(false);

        // release
        isTaskEnd = true;
        isTaskStart = false;

        // distance error
        manipulationError = Vector3.zero;
        for (int i = 0; i < targetT.transform.childCount; i++)
        {
            manipulationError += targetT.transform.GetChild(i).transform.position - objectT.transform.GetChild(i).transform.position;
        }
        scoreText.text = scoreText.text + "Time: " + taskTime.ToString("F1") + ", offset: " + manipulationError.magnitude.ToString("F2") + "\n";
        partSumErr += manipulationError.magnitude;
        partSumTime += taskTime;
        dataRecording.AddOneData(parkourCounter.locomotionTech.stage.ToString(), completeCount, taskTime, manipulationError);

        // Debug.Log("Time: " + taskTime.ToString("F1") + "\nPrecision: " + manipulationError.magnitude.ToString("F1"));
        Destroy(objectT);
        Destroy(targetT);
        StartCoroutine(Countdown(3f));
    }

    IEnumerator Countdown(float t)
    {
        taskTime = 0f;
        taskStartPanel.SetActive(true);
        isCountdown = true;
        completeCount += 1;

        if (completeCount > 4)
        {
            taskStartPanel.SetActive(false);
            scoreText.text = "Done Part" + part.ToString();
            part += 1;
            completeCount = 0;
            UpdateCurrentTarget();
        }
        else
        {
            yield return new WaitForSeconds(t);
            isCountdown = false;
            startPanelText.text = "start";
        }
        isCountdown = false;
        yield return 0;
    }
}

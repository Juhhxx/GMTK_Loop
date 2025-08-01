using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRecorder : MonoBehaviour
{
    [SerializeField] private GameObject _recordedSubject;
    [SerializeField] private GameObject _shadowPrefab;
    [SerializeField] private int        _maxShadows;
    [SerializeField] private float      _recordingRate;
    

    private List<List<PlayerRecorderData>> _recordedDatas;
    private Timer   _updateTimer;
    private int     _shadowsRunning;
    private bool    _isRecording;
    private List<GameObject> _shadows;

    private void Awake()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        _recordedSubject = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
    private void Start()
    {
        _recordedDatas = new List<List<PlayerRecorderData>>();
        _shadows = new List<GameObject>();

        _updateTimer = new Timer(_recordingRate);
        _updateTimer.OnTimerDone += TimedUpdate;

        StartRecording();
    }
    private void Update()
    {
        _updateTimer.CountTimer();

        // for (int i = 0; i < _recordedDatas.Count; i++)
        //     Debug.Log($"Recorded lenght {i} : {_recordedDatas[i].Count}");

        Debug.Log($"Recorded lenght : {_recordedDatas.Count}");

        if (Input.GetKeyDown(KeyCode.P)) StartPlaying();
    }
    private void TimedUpdate()
    {
        if (_isRecording) Record();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void StartPlaying()
    {
        CleanShadows();

        for (int i = 0; i < _shadowsRunning; i++)
        {
            GameObject newS = Instantiate(_shadowPrefab);

            ShadowMovement moveS = newS.GetComponent<ShadowMovement>();

            moveS.StartMovement(_recordedDatas[i]);
            moveS.SetID(i);

            _shadows.Add(newS);
        }

        StartRecording();
    }

    private void CleanShadows()
    {
        if (_shadows.Count == 0) return;

        foreach (GameObject shadow in _shadows)
        {
            Destroy(shadow);
        }

        _shadows.Clear();
    }

    private void StartRecording()
    {
        CreateRecordingSlot();

        _isRecording = true;

        Debug.Log($"shadowsRunning : {_shadowsRunning}");
    }
    private void CreateRecordingSlot()
    {
        // Create new Queue to save positions
        _recordedDatas.Insert(0, new List<PlayerRecorderData>());

        Debug.Log($"Recordings saved : {_recordedDatas.Count}");

        // If max number of shadows hasn't been reached then add 1 to max shadows
        // If else, remove the one saved for the longest
        if (_recordedDatas.Count <= _maxShadows) _shadowsRunning++;
        else _recordedDatas.RemoveAt(3);

        Debug.Log($"Recordings saved 2 : {_recordedDatas.Count}");
    }
    private void Record()
    {
        Debug.Log("RECORDING");

        PlayerRecorderData data =
        new PlayerRecorderData(_recordedSubject.transform.position, Time.time);

        _recordedDatas[0].Add(data);
    }
}

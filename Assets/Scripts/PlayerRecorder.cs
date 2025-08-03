using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRecorder : MonoBehaviourDDOL<PlayerRecorder>
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
        SingletonCheck(this);

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        _recordedSubject = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
    private void Start()
    {
        _recordedDatas = new List<List<PlayerRecorderData>>();
        _shadows = new List<GameObject>();

        _updateTimer = new Timer(_recordingRate);
        _updateTimer.OnTimerDone += TimedUpdate;
    }
    private void Update()
    {
        _updateTimer.CountTimer();

        // for (int i = 0; i < _recordedDatas.Count; i++)
        //     Debug.Log($"Recorded lenght {i} : {_recordedDatas[i].Count}");

        Debug.Log($"Recorded lenght : {_recordedDatas.Count}");

        if (Input.GetKeyDown(KeyCode.P)) StartPlaying();

        if (Input.GetButtonDown("Interact") && _isRecording) Record();
    }
    private void TimedUpdate()
    {
        if (_isRecording) Record();
    }

    public void SetMaxShadows(int max) => _maxShadows = max;

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
        Debug.Log($"shadowsRunning 1 : {_shadowsRunning}");
        CreateRecordingSlot();

        _isRecording = true;

        Debug.Log($"shadowsRunning 2 : {_shadowsRunning}");
    }
    public void ResetRecordings()
    {
        _isRecording = false;
        _recordedDatas.Clear();
        _shadowsRunning = 0;
        StartRecording();
    }
    private void CreateRecordingSlot()
    {
        // Create new Queue to save positions
        _recordedDatas.Insert(0, new List<PlayerRecorderData>());

        Debug.Log($"Recordings saved : {_recordedDatas.Count}");

        // If max number of shadows hasn't been reached then add 1 to max shadows
        // If else, remove the one saved for the longest
        if (_recordedDatas.Count <= _maxShadows) _shadowsRunning++;
        else _recordedDatas.Remove(_recordedDatas.Last());

        Debug.Log($"Recordings saved 2 : {_recordedDatas.Count}");
    }
    private void Record()
    {
        Debug.Log("RECORDING");

        PlayerRecorderData data =
        new PlayerRecorderData(_recordedSubject.transform.position, Time.time,
        Input.GetButtonDown("Interact"));

        _recordedDatas[0].Add(data);
    }
}

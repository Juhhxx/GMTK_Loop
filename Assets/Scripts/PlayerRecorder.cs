using System.Collections.Generic;
using DG.Tweening;
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

        for (int i = 0; i < _recordedDatas.Count; i++)
            Debug.Log($"Recorded lenght {i} : {_recordedDatas[i].Count}");

        if (Input.GetKeyDown(KeyCode.P)) StartPlaying();
    }
    private void TimedUpdate()
    {
        if (_isRecording) Record();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void StartPlaying()
    {
        StartRecording();

        CleanShadows();

        for (int i = 0; i < _shadowsRunning - 1 ; i++)
        {
            GameObject newS = Instantiate(_shadowPrefab);

            ShadowMovement moveS = newS.GetComponent<ShadowMovement>();

            moveS.StartMovement(_recordedDatas[i + 1]);
            moveS.SetID(i);

            _shadows.Add(newS);
        }
    }

    private void CleanShadows()
    {
        if (_shadows.Count == 0) return;

        foreach (GameObject shadow in _shadows)
        {
            shadow.transform.DOKill();
            Destroy(shadow);
        }

        _shadows.Clear();
    }

    private void StartRecording()
    {
        // Create new Queue to save positions
        _recordedDatas.Insert(0, new List<PlayerRecorderData>());

        _isRecording = true;

        // If max number of shadows hasn't been reached then add 1 to max shadows
        if (_shadowsRunning <= _maxShadows) _shadowsRunning++;
        // If else, remove the one saved for the longest
        else _recordedDatas.RemoveAt(2);

        Debug.Log($"shadowsRunning : {_shadowsRunning}");
    }
    private void Record()
    {
        Debug.Log("RECORDING");

        PlayerRecorderData data =
        new PlayerRecorderData(_recordedSubject.transform.position, Time.time);

        _recordedDatas[0].Add(data);
    }
}

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerRecorder : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _recordedSubject;
    [SerializeField] private Rigidbody2D _player;
    
    private Queue<PlayerRecorderData> _dataQueue;
    private Vector2 _startPositionRecording;
    private bool _playing = false;
    private Timer _timer;

    private void Start()
    {
        _dataQueue = new Queue<PlayerRecorderData>();
        _timer = new Timer(0.05f);
        _timer.OnTimerDone += () => _playing = true;
    }
    private void FixedUpdate()
    {
        Record();
        _timer.CountTimer();

        if (_playing) Play();
    }

    private void Record()
    {
        Debug.Log("RECORDING");
        _dataQueue.Enqueue(new PlayerRecorderData(_recordedSubject.transform.position));
    }

    private void Play()
    {
        Debug.Log("PLAYING");
        _player.transform.position = _dataQueue.Dequeue().Position;
    }
}

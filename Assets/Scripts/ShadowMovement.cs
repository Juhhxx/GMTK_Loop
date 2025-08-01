using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShadowMovement : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;
    private GameObject _target;
    private Queue<PlayerRecorderData> _movementData;
    private PlayerRecorderData _lastData;
    private int _id;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        GetComponentInChildren<TextMeshPro>().text = $"{_id}";
    }
    private void OnDestroy()
    {
        transform.DOKill();

        Destroy(_target);
    }

    public void SetID(int id) => _id = id;

    public void StartMovement(List<PlayerRecorderData> data)
    {
        _movementData = OptimizePath(data);
        // _movementData = new Queue<PlayerRecorderData>(data);

        // Debug.Log($"lenght : {data.Count} and {_movementData.Count}");

        _lastData = _movementData.Dequeue();

        transform.position = _lastData.Position;

        DoMovement();

        _target = Instantiate(_targetPrefab, data.Last().Position, Quaternion.identity);
    }

    private Queue<PlayerRecorderData> OptimizePath(List<PlayerRecorderData> data)
    {
        List<PlayerRecorderData> path = new List<PlayerRecorderData>();
        int dataSize = data.Count;

        for (int i = 0; i < dataSize; i++)
        {
            // Ignore first and last point
            if (i == 0 || i == dataSize - 1)
            {
                path.Add(data[i]);
                continue;
            }

            // Ignore points between two equal points
            if (data[i - 1].Position == data[i].Position &&
                data[i + 1].Position == data[i].Position) continue;

            // // Ignore points between two equal x values
            // if (data[i - 1].Position.x == data[i].Position.x &&
            //     data[i + 1].Position.x == data[i].Position.x) continue;

            // // Ignore points between two equal y values
            // if (data[i - 1].Position.y == data[i].Position.y &&
            //     data[i + 1].Position.y == data[i].Position.y) continue;

            path.Add(data[i]);
        }

        return new Queue<PlayerRecorderData>(path);
    }

    private void DoMovement()
    {
        if (_movementData.Count == 0)
        {
            _anim?.SetFloat("SpeedX", 0);
            return;
        }

        PlayerRecorderData data = _movementData.Dequeue();

        Vector2 endPoint = data.Position;

        float timeDuration = data.Time - _lastData.Time;
        float velocityX = (endPoint.x - transform.position.x) / timeDuration;
        float velocityY = (endPoint.y - transform.position.y) / timeDuration;

        _anim?.SetFloat("SpeedX", Mathf.Abs(velocityX));

        _lastData = data;

        if (endPoint.x - transform.position.x > 0) transform.rotation = Quaternion.identity;
        else if (endPoint.x - transform.position.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);

        transform.DOMove(endPoint, timeDuration).SetEase(Ease.Linear).OnComplete(DoMovement);
    }
}

using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShadowMovement : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private bool _spawnTarget;
    [SerializeField] private bool _showId;
    [SerializeField] private string _interactiveDebug;

    private GameObject _target;
    private Queue<PlayerRecorderData> _movementData;
    private PlayerRecorderData _lastData;
    private int _id;
    private Animator _anim;
    private Interactive _currentInteractive;
    private bool _isGrounded;
    [SerializeField] private LayerMask _groundLayer;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();

        if (_showId) tmp.text = $"{_id}";
        else tmp.gameObject.SetActive(false);
    }
    private void Update()
    {
        CheckIfGrounded();
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

        if (_spawnTarget)
            _target = Instantiate(_targetPrefab, data.Last().Position, Quaternion.identity);
    }
    private Queue<PlayerRecorderData> OptimizePath(List<PlayerRecorderData> data)
    {
        List<PlayerRecorderData> path = new List<PlayerRecorderData>();
        int dataSize = data.Count;

        for (int i = 0; i < dataSize; i++)
        {
            // Ignore first and last point
            if (i == 0 || i == dataSize - 1 || data[i].Interacted)
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

        if (data.Interacted) DoInteraction();

        Vector2 endPoint = data.Position;

        float timeDuration = data.Time - _lastData.Time;
        float velocityX = (endPoint.x - transform.position.x) / timeDuration;
        float velocityY = (endPoint.y - transform.position.y) / timeDuration;

        _anim?.SetFloat("SpeedX", Mathf.Abs(velocityX));
        _anim?.SetFloat("SpeedY", velocityY);
        _anim?.SetBool("IsGrounded", _isGrounded);

        _lastData = data;

        if (endPoint.x - transform.position.x > 0) transform.rotation = Quaternion.identity;
        else if (endPoint.x - transform.position.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);

        transform.DOMove(endPoint, timeDuration).SetEase(Ease.Linear).OnComplete(DoMovement);

        if (_movementData.Count == 0) _anim?.SetTrigger("Die");
    }

    private void CheckIfGrounded()
    {
        _isGrounded = Physics2D.Raycast(transform.position,
        Vector3.down, 0.2f, _groundLayer);
    }

    // Interaction
    private void DoInteraction()
    {
        if (_currentInteractive != null)
        {
            _currentInteractive.Interaction();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactive interactive = collision.GetComponent<Interactive>();

        if (interactive != null && interactive != _currentInteractive)
        {
            _currentInteractive = interactive;
            _interactiveDebug = interactive.gameObject.name;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _currentInteractive = null;
        _interactiveDebug = "None";
    }
}

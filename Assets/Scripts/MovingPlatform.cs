using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [OnValueChanged("SetPlatform")]
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _speed;
    [SerializeField] private float _stopTime;
    [SerializeField] private bool _startOn = true;
    [SerializeField] private bool _loop = true;


    public enum MovementType { Horizontal, Vertical, Custom }
    [OnValueChanged("SetUpWaypoints")]
    [SerializeField] private MovementType _movementType;

    [OnValueChanged("SetUpWaypoints")]
    [HideIf("_movementType", MovementType.Custom)]
    [SerializeField] private float _distance = 5.0f;

    [OnValueChanged("SetUpWaypoints")]
    [HideIf("_movementType", MovementType.Custom)]
    [SerializeField] private int _numberOfWaypoints = 3;

    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private bool _showPath = false;

    YieldInstruction _wfsStay;

    private void Start()
    {
        SetPlatform();

        _wfsStay = new WaitForSeconds(_stopTime);

        if (_startOn) StartMovingPlatform(false);
    }

    private void SetPlatform()
    {
        if (_platform == null) return;

        _platform.transform.position = _waypoints[0].position;
    }

    public void StartMovingPlatform(bool inReverse)
    {
        StopAllCoroutines();
        StartCoroutine(MovePlatform(inReverse));
    }

    private IEnumerator MovePlatform(bool inReverse)
    {
        Queue<Transform> path = new Queue<Transform>(_waypoints);

        if (inReverse) path = new Queue<Transform>(path.Reverse());

        Vector2 startPos = path.Dequeue().position;

        while (path.Count > 0)
        {
            Vector2 endPos = path.Dequeue().position;
            Vector2 newPos = startPos;

            float i = 0;

            while (newPos != endPos)
            {
                newPos = Vector2.Lerp(startPos, endPos, i);

                _platform.transform.position = newPos;

                i += _speed * Time.deltaTime;

                yield return null;
            }

            startPos = endPos;

            yield return _wfsStay;
        }

        if (_loop) StartMovingPlatform(!inReverse);
    }

    // Waypoint Creation
    private void CreateWaypoint(int id, Vector2 pos)
    {
        GameObject waypoint = new GameObject($"Waypoint_{id}");

        waypoint.transform.parent = gameObject.transform;

        waypoint.transform.localPosition = pos;

        _waypoints.Add(waypoint.transform);
    }
    private void DestroyWaypoints()
    {
        if (_waypoints.Count == 0) return;

        foreach (Transform w in _waypoints)
        {
            DestroyImmediate(w.gameObject);
        }

        _waypoints.Clear();
    }
    private void SetUpWaypoints()
    {
        if (_movementType != MovementType.Custom)
        {
            DestroyWaypoints();

            _waypoints = new List<Transform>();

            Vector2 pos = Vector2.zero;

            Vector2 dir = _movementType == MovementType.Vertical ? Vector2.up
                                                                : Vector2.right;

            for (int i = 0; i < _numberOfWaypoints; i++)
            {
                CreateWaypoint(i, pos);

                pos += dir * _distance;
            }
        }
    }
    [Button(enabledMode: EButtonEnableMode.Always)]
    private void AddWaypoint()
    {
        if (_waypoints.Count == 0)
        {
            CreateWaypoint(0, Vector2.zero);
            return;
        }
        else if (_waypoints.Count == 1)
        {
            Vector3 dir = _movementType == MovementType.Vertical ? Vector2.up
                                                                : Vector2.right;

            CreateWaypoint(1, _waypoints[0].localPosition + (dir * _distance));

            return;                                   
        }
        
        switch (_movementType)
        {
            case MovementType.Horizontal:
                _numberOfWaypoints++;
                CreateWaypoint(_waypoints.Count,
                _waypoints.Last().localPosition + (Vector3.right * _distance));
                break;

            case MovementType.Vertical:
                _numberOfWaypoints++;
                CreateWaypoint(_waypoints.Count,
                _waypoints.Last().localPosition + (Vector3.up * _distance));
                break;

            case MovementType.Custom:
                Vector3 dir = _waypoints[_waypoints.Count - 1].localPosition -
                              _waypoints[_waypoints.Count - 2].localPosition;

                dir = Vector3.Normalize(dir);

                CreateWaypoint(_waypoints.Count,
                _waypoints.Last().localPosition + (dir * _distance));
                break;
        }
        
    }
    [Button(enabledMode: EButtonEnableMode.Always)]
    private void RemoveWaypoint()
    {
        Transform waypoint = _waypoints.Last();

        _waypoints.Remove(waypoint);
        DestroyImmediate(waypoint.gameObject);

        if (_movementType != MovementType.Custom) _numberOfWaypoints--;
    }

    // Waypoint Draw
    private void OnDrawGizmos()
    {
        if (_showPath)
        {
            for (int i = 0; i < _waypoints.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_waypoints[i].position, 0.25f);

                if (i != _waypoints.Count - 1)
                {
                    Gizmos.DrawLine(_waypoints[i].position,
                                    _waypoints[i + 1].position);
                }
            }
        }
    }
}

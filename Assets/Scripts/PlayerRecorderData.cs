using System;
using UnityEngine;
public struct PlayerRecorderData
{
    private Vector2 _position;
    public Vector2 Position => _position;

    private float _time;
    public float Time => _time;

    public PlayerRecorderData(Vector2 position, float time)
    {
        _position = position;
        _time = time;
    }
}
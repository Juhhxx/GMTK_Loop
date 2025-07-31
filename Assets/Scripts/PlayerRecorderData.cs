using System;
using UnityEngine;
public struct PlayerRecorderData
{
    private Vector2 _position;
    public Vector2 Position => _position;

    public PlayerRecorderData(Vector2 position)
    {
        _position = position;
    }
}
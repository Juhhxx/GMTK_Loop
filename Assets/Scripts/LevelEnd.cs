using System;
using UnityEngine;
using UnityEngine.Events;

public class LevelEnd : MonoBehaviour
{
    public UnityEvent OnPlayerReachEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PLAYER REACHED END!!!!");
        OnPlayerReachEnd?.Invoke();
    }
}

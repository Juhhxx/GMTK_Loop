using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEvent _onStartButton;

    public void OnStartClick()
    {
        _onStartButton?.Invoke();
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}

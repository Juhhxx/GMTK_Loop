using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField][Expandable] private List<LevelProfile> _levels;
    [SerializeField] private PlayerRecorder _recorder;

    private LevelProfile _currentLevel;

    private void LoadLevel()
    {

    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(_currentLevel.LevelScene);
        // SceneManager.sceneLoaded
    }
}

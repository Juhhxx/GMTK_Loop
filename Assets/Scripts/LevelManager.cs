using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourDDOL<LevelManager>
{
    [SerializeField][Expandable] private List<LevelProfile> _levels;
    [SerializeField] private PlayerRecorder _recorder;

    private PlayerMovement _player;
    [SerializeField] private LevelEnd _levelEnd;
    private LevelProfile _currentLevel;
    private Queue<LevelProfile> _levelQueue;

    private void Awake()
    {
        SingletonCheck(this);
    }

    private void Start()
    {
        _levelQueue = new Queue<LevelProfile>(_levels);

        SetUpEvents();

        SceneManager.sceneLoaded += (Scene s, LoadSceneMode m) => SetUpEvents();
        SceneManager.sceneUnloaded += (Scene s) => TurnOffEvents();

    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void GoToNextLevel()
    {
        LevelProfile level = _levelQueue.Dequeue();
        Debug.Log($"GOING TO LEVEL {level}");
        _ = LoadLevel(level);
    }
    private async Task LoadLevel(LevelProfile level)
    {
        _currentLevel = level;

        Debug.Log($"LOADING LEVEL {level}");

        await SceneManager.LoadSceneAsync(level.LevelScene);

        Debug.Log($"LOADED LEVEL {level}");

        _recorder.SetMaxShadows(level.MaxShadowsAllowed);
        _recorder.ResetRecordings();
    }
    public async void RestartLevel()
    {
        await SceneManager.LoadSceneAsync(_currentLevel.LevelScene);
        _recorder.StartPlaying();
    }
    private void SetUpEvents()
    {
        _levelEnd = FindAnyObjectByType<LevelEnd>();
        _player = FindAnyObjectByType<PlayerMovement>();

        _levelEnd.OnPlayerReachEnd.AddListener(GoToNextLevel);
        _player.OnPlayerDeath.AddListener(RestartLevel);
    }
    private void TurnOffEvents()
    {
        _levelEnd.OnPlayerReachEnd.RemoveListener(GoToNextLevel);
        _player.OnPlayerDeath.RemoveListener(RestartLevel);
    }
}

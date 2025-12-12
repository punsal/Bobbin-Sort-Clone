using Core.Event.Abstract;
using Core.Save;
using Gameplay.Data;
using Gameplay.Level;
using Gameplay.Level.Observables;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private LevelIndexObservable levelIndexObservable;

    [Header("Events")]
    // Events to raise
    [SerializeField] private AScriptableEvent levelStartEvent;
    [SerializeField] private AScriptableEvent levelWinEvent;
    [SerializeField] private AScriptableEvent levelLoseEvent;
    // Events to subscribe
    [SerializeField] private AScriptableEvent nextLevelEvent;
    [SerializeField] private AScriptableEvent retryLevelEvent;
    
    private LevelController _currentLevel;
    
    private GameSaveData _gameSaveData;
    private int _currentLevelIndex;

    private void Awake()
    {
        _gameSaveData = SaveManager.Load<GameSaveData>();
        _currentLevelIndex = Mathf.Max(0, _gameSaveData.LastLevelIndex);
    }

    private void OnEnable()
    {
        if (nextLevelEvent != null) nextLevelEvent.OnRaised += OnNextLevel;
        if (retryLevelEvent != null) retryLevelEvent.OnRaised += OnRetryLevel;
    }

    private void OnDisable()
    {
        DisposeCurrentLevel();
        
        if (nextLevelEvent != null) nextLevelEvent.OnRaised -= OnNextLevel;
        if (retryLevelEvent != null) retryLevelEvent.OnRaised -= OnRetryLevel;
    }

    private void Start()
    {
        StartLevel(_currentLevelIndex);
    }

    private void StartLevel(int levelIndex)
    {
        DisposeCurrentLevel();
        LoadLevel(levelIndex);
        SetLevelIndex(levelIndex);
        RaiseLevelStart();
    }

    private void DisposeCurrentLevel()
    {
        if (_currentLevel == null)
        {
            return;
        }
        
        _currentLevel.LevelCompleted -= OnLevelCompleted;
        _currentLevel.LevelFailed -= OnLevelFailed;
        _currentLevel.Dispose();
    }

    private void LoadLevel(int levelIndex)
    {
        _currentLevel = levelLoader.LoadLevel(levelIndex);

        if (_currentLevel == null)
        {
            Debug.LogError($"Level {levelIndex} failed to load.");
            return;
        }

        _currentLevel.LevelCompleted += OnLevelCompleted;
        _currentLevel.LevelFailed += OnLevelFailed;
    }

    private void SetLevelIndex(int levelIndex)
    {
        if (levelIndexObservable == null)
        {
            Debug.LogError("LevelIndexObservable is null");
            return;
        }
        
        levelIndexObservable.Set(levelIndex);
    }

    private void RaiseLevelStart()
    {
        if (levelStartEvent == null)
        {
            Debug.LogError("LevelStartEvent is null");
            return;
        }
        levelStartEvent.Raise();
    }

    private void OnLevelCompleted()
    {
        if (levelWinEvent == null)
        {
            Debug.LogError("LevelWinEvent is null");
            return;
        }
        
        levelWinEvent.Raise();
    }

    private void OnLevelFailed()
    {
        if (levelLoseEvent == null)
        {
            Debug.LogError("LevelLoseEvent is null");
            return;
        }
        
        levelLoseEvent.Raise();
    }
    
    private void OnNextLevel()
    {
        _currentLevelIndex++;
        SaveCurrentLevelIndex();
        
        StartLevel(_currentLevelIndex);
    }
    
    private void OnRetryLevel()
    {
        SaveCurrentLevelIndex();
        
        StartLevel(_currentLevelIndex);
    }

    private void SaveCurrentLevelIndex()
    {
        _gameSaveData.LastLevelIndex = _currentLevelIndex;
        SaveManager.Save(_gameSaveData);
    }
}
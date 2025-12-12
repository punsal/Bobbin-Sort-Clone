using System;
using System.Collections.Generic;
using Gameplay.Container;
using Gameplay.Input;
using Gameplay.Item.Data;
using Gameplay.Item.Types;
using Gameplay.Level.Config;
using Gameplay.Level.Type;
using UnityEngine;

namespace Gameplay.Level
{
    public class LevelController : MonoBehaviour, IDisposable
    {
        [Header("Dependencies")]
        [SerializeField] private InputController inputController;
        
        [Header("Configuration")]
        [SerializeField] private List<SortContainer> sortContainers;
    
        private SortContainer _lastSelectedSortContainer;
        private List<SortItemData> _lastSelectedSortItemDataList;
        
        private LevelState _levelState;
        
        private event Action levelCompleted;
        public event Action LevelCompleted
        {
            add => levelCompleted += value;
            remove => levelCompleted -= value;
        }
        
        private event Action levelFailed;
        public event Action LevelFailed
        {
            add => levelFailed += value;
            remove => levelFailed -= value;
        }

        public void Initialize(LevelDefinition definition)
        {
            var count = Mathf.Min(sortContainers.Count, definition.containers.Length);

            // initialize containers with given colors
            for (var i = 0; i < count; i++)
            {
                sortContainers[i].InitializeFromDefinition(definition.containers[i].colorsBottomToTop);
            }

            // initialize remaining containers with empty colors
            for (var i = count; i < sortContainers.Count; i++)
            {
                sortContainers[i].InitializeFromDefinition(Array.Empty<SortItemColor>());
            }

            Load();
            Run();
        }
        
        public void Load()
        {
            _levelState = LevelState.Loading;
            if (inputController == null)
            {
                Debug.LogError("InputController is null");
                _levelState = LevelState.Error;
                return;
            }
        
            _lastSelectedSortContainer = null;
            _lastSelectedSortItemDataList = new List<SortItemData>();
        }

        public void Run()
        {
            if (_levelState == LevelState.Loading)
            {
                _levelState = LevelState.Running;
                return;
            }

            if (_levelState == LevelState.Error)
            {
                Debug.LogError("Level failed to load.");
                return;
            }
        }

        public void Dispose()
        {
            levelCompleted = null;
            levelFailed = null;
        }

        private void Update()
        {
            if (_levelState != LevelState.Running)
            {
                return;
            }

            if (!inputController.DidTap<SortContainer>(out var container))
            {
                return;
            }
            
            OnContainerTapped(container);
                
            if (_lastSelectedSortContainer != null)
            {
                return;
            }

            EvaluateLevel();
        }
        
        private void EvaluateLevel()
        {
            if (IsLevelCompleted())
            {
                _levelState = LevelState.Completed;
                levelCompleted?.Invoke();
                return;
            }

            if (HasMoves())
            {
                return;
            }
            
            _levelState = LevelState.Failed;
            levelFailed?.Invoke();
        }

        private void OnContainerTapped(SortContainer sortContainer)
        {
            if (_lastSelectedSortContainer == null)
            {
                SelectContainer(sortContainer);
                return;
            }
        
            if (sortContainer == _lastSelectedSortContainer)
            {
                DeselectContainer(sortContainer);
                return;
            }

            ApplyContainerChanges(sortContainer);
        }

        private void SelectContainer(SortContainer sortContainer)
        {
            if (sortContainer.IsDone())
            {
                return;
            }

            if (sortContainer.IsEmpty())
            {
                return;
            }

            if (!sortContainer.TryRemoveLastSortItemData(out var removedSortItemDataList))
            {
                Debug.LogError("Failed to remove last sort item data from container.");
                return;
            }

            _lastSelectedSortContainer = sortContainer;
            _lastSelectedSortItemDataList = removedSortItemDataList;
            _lastSelectedSortContainer.Select(_lastSelectedSortItemDataList);
        }

        private void DeselectContainer(SortContainer sortContainer)
        {
            sortContainer.Deselect(_lastSelectedSortItemDataList);
            _lastSelectedSortContainer = null;
            _lastSelectedSortItemDataList.Clear();
        }
    
        private void ApplyContainerChanges(SortContainer sortContainer)
        {
            if (sortContainer.IsDone())
            {
                Debug.Log("Container is done.");
                return;
            }

            if (sortContainer.IsFull())
            {
                Debug.Log("Container is full.");
                return;
            }
            
            var lastItemData = _lastSelectedSortItemDataList[^1];
            if (!sortContainer.CanReceive(lastItemData))
            {
                Debug.Log($"Container cannot receive last item data. Mismatched color: {lastItemData.ColorObservable.Value} and {sortContainer.PeekTop().ColorObservable.Value}");
                return;
            }

            if (!sortContainer.TryAddSortItemData(_lastSelectedSortItemDataList, out var failedSortItemDataList))
            {
                foreach (var failedSortItemData in failedSortItemDataList)
                {
                    _lastSelectedSortItemDataList.Remove(failedSortItemData);
                }
                _lastSelectedSortContainer.Deselect(failedSortItemDataList);
                _lastSelectedSortItemDataList.Clear();
                _lastSelectedSortContainer = null;
                return;
            }
        
            _lastSelectedSortItemDataList.Clear();
            _lastSelectedSortContainer.Deselect(_lastSelectedSortItemDataList);
            _lastSelectedSortContainer = null;
        }

        private bool IsLevelCompleted()
        {
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var sortContainer in sortContainers)
            {
                if (sortContainer.IsEmpty())
                {
                    continue;
                }

                if (!sortContainer.IsDone())
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool HasMoves()
        {
            for (var i = 0; i < sortContainers.Count; i++)
            {
                var from = sortContainers[i];

                if (from == null || from.IsEmpty() || from.IsDone())
                {
                    continue;
                }

                if (!from.TryPeekSortItemDataList(out var group))
                {
                    continue;
                }

                var topItem = group[0];

                for (var j = 0; j < sortContainers.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var target = sortContainers[j];

                    if (target == null)
                    {
                        continue;
                    }

                    if (target.CanReceive(topItem))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
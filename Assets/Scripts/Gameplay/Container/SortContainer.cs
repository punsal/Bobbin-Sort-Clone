using System.Collections.Generic;
using Gameplay.Item;
using Gameplay.Item.Data;
using Gameplay.Item.Extensions;
using Gameplay.Item.Types;
using UnityEngine;

namespace Gameplay.Container
{
    public class SortContainer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer selectionVisual;
        
        [SerializeField] private SortItem[] sortItems;
        private readonly List<SortItemData> _sortItemDataList = new();
        
        public void InitializeFromDefinition(SortItemColor[] colorsBottomToTop)
        {
            _sortItemDataList.Clear();
            
            if (sortItems.Length == 0)
            {
                Debug.LogError("No sort item found in container");
            }

            var capacity = sortItems.Length;

            for (var i = 0; i < capacity; i++)
            {
                var sortItem = sortItems[i];
                var data = sortItem.GetData();

                if (i < colorsBottomToTop.Length)
                {
                    data.StateObservable.Set(SortItemState.Sorted);
                    data.ColorObservable.Set(colorsBottomToTop[i]);
                }
                else
                {
                    data.StateObservable.Set(SortItemState.Empty);
                }
                
                sortItem.Initialize(data);
                _sortItemDataList.Add(data);
            }
        }

        public void Select(List<SortItemData> selectedData)
        {
            var size = selectedData.Count;
            for (var i = size - 1; i >= 0; i--)
            {
                var data = selectedData[i];
                data.StateObservable.Set(SortItemState.Empty);
            }
            
            var color = selectedData[0].ColorObservable.Value;
            selectionVisual.color = color.ToColor();
            
            Debug.Log($"Selected {selectedData.Count} sort items and set current color to {color}");
        }
        
        public void Deselect(List<SortItemData> deselectedDataList)
        {
            selectionVisual.color = Color.white;
            if (deselectedDataList.Count == 0)
            {
                Debug.Log("Deselected all data, nothing to do.");
                return;
            }

            if (!TryGetEmptySortItemData(out var emptyDataList))
            {
                throw new System.Exception("Failed to get empty sort item data.");
            }
            
            var size = Mathf.Min(deselectedDataList.Count, emptyDataList.Count);
            for (var i = size - 1; i >= 0; i--)
            {
                var emptyData = emptyDataList[i];
                var deselectedData = deselectedDataList[i];
                emptyData.StateObservable.Set(SortItemState.Sorted);
                emptyData.ColorObservable.Set(deselectedData.ColorObservable.Value);
                Debug.Log($"Deselected sort item {deselectedData.ColorObservable.Value}");
            }
        }

        public bool IsEmpty()
        {
            var size = _sortItemDataList.Count;
            for (var i = 0; i < size; i++)
            {
                var data = _sortItemDataList[i];
                if (data.StateObservable.Value != SortItemState.Empty)
                {
                    return false;
                }
            }
        
            return true;
        }
    
        public bool IsFull()
        {
            var size = _sortItemDataList.Count;
            var sortedCount = 0;
            for (var i = 0; i < size; i++)
            {
                var data = _sortItemDataList[i];
                if (data.StateObservable.Value == SortItemState.Sorted)
                {
                    sortedCount++;
                }
            }
        
            return sortedCount == size;
        }

        public bool IsDone()
        {
            var size = _sortItemDataList.Count;
            var lastColor = _sortItemDataList[^1].ColorObservable.Value;
            for (var i = size - 1; i >= 0; i--)
            {
                var data = _sortItemDataList[i];
                if (data.StateObservable.Value != SortItemState.Sorted)
                {
                    return false;
                }
                if (data.ColorObservable.Value != lastColor)
                {
                    return false;
                }
            }

            return true;
        }
        
        private int FindLastSortedIndex()
        {
            for (var i = _sortItemDataList.Count - 1; i >= 0; i--)
            {
                var item = _sortItemDataList[i];
                if (item != null && item.StateObservable.Value == SortItemState.Sorted)
                {
                    return i;
                }
            }

            return -1;
        }
        
        public SortItemData PeekTop()
        {
            var index = FindLastSortedIndex();
            return index >= 0 ? _sortItemDataList[index] : null;
        }

        private bool TryGetEmptySortItemData(out List<SortItemData> result)
        {
            result = new List<SortItemData>();
            if (IsFull())
            {
                Debug.Log("Container is full.");
                return false;
            }

            var size = _sortItemDataList.Count;
            for (var i = size - 1; i >= 0; i--)
            {
                var data = _sortItemDataList[i];
                if (data.StateObservable.Value == SortItemState.Empty)
                {
                    result.Add(data);
                }    
            }
        
            result.Reverse();
            Debug.Log($"Found {result.Count} empty sort item/s");
            return true;
        }
        
        public bool CanReceive(SortItemData data)
        {
            if (IsFull())
            {
                return false;
            }

            var lastIndex = FindLastSortedIndex();
            if (lastIndex < 0)
            {
                return true;
            }

            var top = _sortItemDataList[lastIndex];
            return top.ColorObservable.Value == data.ColorObservable.Value;
        }
    
        public bool TryAddSortItemData(List<SortItemData> dataList, out List<SortItemData> failedDataList)
        {
            failedDataList = new List<SortItemData>(dataList);
            var newSize = dataList.Count;
            if (newSize == 0)
            {
                Debug.LogError("No data to add.");
                return false;
            }
        
            if (!TryGetEmptySortItemData(out var emptyDataList))
            {
                Debug.LogError("Container is full.");
                return false;
            }
        
            var emptySize = emptyDataList.Count;
            var addSize = Mathf.Min(newSize, emptySize);
            for (var i = 0; i < addSize; i++)
            {
                var emptyData = emptyDataList[i];
                var newData = dataList[i];
                emptyData.StateObservable.Set(SortItemState.Sorted);
                emptyData.ColorObservable.Set(newData.ColorObservable.Value);

                failedDataList.Remove(newData);
            }

            if (failedDataList.Count > 0)
            {
                Debug.Log($"Failed to add all data. Remaining data count: {failedDataList.Count}.");
                return false;
            }
        
            Debug.Log($"Added {addSize} data to container.");
            return true;
        }
    
        public bool TryRemoveLastSortItemData(out List<SortItemData> removedDataList)
        {
            removedDataList = new List<SortItemData>();
            if (IsDone())
            {
                Debug.Log("Container is done.");
                return false;
            }

            if (IsEmpty())
            {
                Debug.Log("Container is empty.");
                return false;
            }

            var size = _sortItemDataList.Count;
            var lastSortItemDataColor = _sortItemDataList[^1].ColorObservable.Value;
            var didFindLastSorted = false;
            for (var i = size - 1; i >= 0; i--)
            {
                var lastItemData = _sortItemDataList[i];
                if (lastItemData.StateObservable.Value == SortItemState.Sorted && !didFindLastSorted)
                {
                    didFindLastSorted = true;
                    lastSortItemDataColor = lastItemData.ColorObservable.Value;
                    removedDataList.Add(lastItemData);
                    Debug.Log($"Removed last sort item data with color {lastSortItemDataColor}");
                    continue;
                }

                if (!didFindLastSorted)
                {
                    continue;
                }

                if (lastItemData.ColorObservable.Value != lastSortItemDataColor)
                {
                    break;
                }
            
                Debug.Log($"Removed sort item data with color of {lastSortItemDataColor}");
                removedDataList.Add(lastItemData);
            }

            if (removedDataList.Count == 0)
            {
                Debug.Log("Failed to remove sort item data from container.");
                return false;
            }
        
            Debug.Log($"Removed {removedDataList.Count} sort item data of color {lastSortItemDataColor} from container.");
            return true;
        }
        
        public bool TryPeekSortItemDataList(out IReadOnlyList<SortItemData> readonlySortItemList)
        {
            readonlySortItemList = null;

            var lastSortedIndex = FindLastSortedIndex();
            if (lastSortedIndex < 0)
            {
                return false;
            }

            var result = new List<SortItemData>();

            var top = _sortItemDataList[lastSortedIndex];
            result.Add(top);

            for (var i = lastSortedIndex - 1; i >= 0; i--)
            {
                var current = _sortItemDataList[i];

                if (current == null)
                {
                    break;
                }

                if (current.StateObservable.Value != SortItemState.Sorted)
                {
                    break;
                }
                
                if (current.ColorObservable.Value != top.ColorObservable.Value)
                {
                    break;
                }

                result.Add(current);
            }

            readonlySortItemList = result;
            return true;
        }
    }
}
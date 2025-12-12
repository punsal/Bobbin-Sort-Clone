using System;
using Gameplay.Item.Data;
using Gameplay.Item.Extensions;
using Gameplay.Item.Types;
using UnityEngine;

namespace Gameplay.Item
{
    public class SortItem : MonoBehaviour, IDisposable
    {
        [Header("Configuration")]
        [SerializeField] private SortItemState state;
        [SerializeField] private SortItemColor color;
        
        [Header("Dependencies")]
        [SerializeField] private SpriteRenderer visual;

        private SortItemData _data;

        private void SubscribeToChanges()
        {
            _data.StateObservable.OnChanged += OnStateChanged;
            _data.ColorObservable.OnChanged += OnColorChanged;
        }

        private void UnsubscribeFromChanges()
        {
            _data.StateObservable.OnChanged -= OnStateChanged;
            _data.ColorObservable.OnChanged -= OnColorChanged;
        }
        
        private void OnDestroy()
        {
            Dispose();
        }

        public void Initialize(SortItemData data)
        {
            _data = data;
            
            SubscribeToChanges();
            
            state = data.StateObservable.Value;
            color = data.ColorObservable.Value;
            
            Render();
        }

        public void Dispose()
        {
            UnsubscribeFromChanges();
            _data.Dispose();
        }

        private void Render()
        {
            OnColorChanged(color);
            OnStateChanged(state);
        }
        
        public SortItemData GetData()
        {
            if (_data != null)
            {
                return _data;
            }
            
            _data = new SortItemData(state, color);
            return _data;
        }

        private void OnStateChanged(SortItemState newState)
        {
            visual.enabled = newState switch
            {
                SortItemState.Empty => false,
                SortItemState.Sorted => true,
                _ => throw new ArgumentOutOfRangeException(nameof(newState), newState, null)
            };
        }

        private void OnColorChanged(SortItemColor newColor)
        {
            visual.color = newColor.ToColor();
        }
    }
}
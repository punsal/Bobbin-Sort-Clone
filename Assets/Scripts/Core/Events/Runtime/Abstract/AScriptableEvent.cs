using System;
using Core.Event.Interface;
using UnityEngine;

namespace Core.Event.Abstract
{
    public abstract class AScriptableEvent : ScriptableObject, IEvent
    {
        private event Action onRaised;
        public event Action OnRaised
        {
            add => onRaised += value;
            remove => onRaised -= value;
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Raise()
        {
            onRaised?.Invoke();    
        }

        public void Dispose()
        {
            onRaised = null;
        }
    }
    
    public abstract class AScriptableEvent<TArgs> : ScriptableObject, IEvent<TArgs>
    {
        private event Action<TArgs> onRaised;
        public event Action<TArgs> OnRaised
        {
            add => onRaised += value;
            remove => onRaised -= value;
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Raise(TArgs args)
        {
            onRaised?.Invoke(args);
        }
        
        public void Dispose()
        {
            onRaised = null;
        }
    }

    public abstract class AScriptableEvent<TArgs1, TArgs2> : ScriptableObject, IEvent<TArgs1, TArgs2>
    {
        private event Action<TArgs1, TArgs2> onRaised;
        public event Action<TArgs1, TArgs2> OnRaised
        {
            add => onRaised += value;
            remove => onRaised -= value;
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Raise(TArgs1 args1, TArgs2 args2)
        {
            onRaised?.Invoke(args1, args2);
        }
        
        public void Dispose()
        {
            onRaised = null;
        }
    }
}
using System;
using UnityEngine;

namespace Core.Observables.Abstract
{
    /// <summary>
    /// Serves as an abstract base class for scriptable objects that implement observable behavior,
    /// allowing subscription to change notifications and tracking changes to a value.
    /// This class is designed for Unity's ScriptableObject framework, enabling integration with Unity's asset-based workflows.
    /// </summary>
    /// <typeparam name="T">The type of the value being observed.</typeparam>
    public abstract class ScriptableObservableBase<T> : ScriptableObject, Interface.IObservable<T>
    {
        public T Value { get; protected set; }
        
        protected event Action<T> Changed;
        public event Action<T> OnChanged
        {
            add => Changed += value;
            remove => Changed -= value;
        }
        
        public void Set(T value, bool notify = true)
        {
            InternalSet(value);
            if (notify) Changed?.Invoke(value);
        }
        
        protected abstract void InternalSet(T value);

        public virtual void Dispose()
        {
            Changed = null;
        }
    }
}
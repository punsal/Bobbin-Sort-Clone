using System;

namespace Core.Observables.Abstract
{
    /// <summary>
    /// Provides an abstract base class for implementing observable entities that can notify subscribers
    /// when their value changes. This class supports initialization with an initial value, value updates,
    /// and subscription to change notifications.
    /// </summary>
    /// <typeparam name="T">The type of the value being observed.</typeparam>
    public abstract class ObservableBase<T> : Interface.IObservable<T>
    {
        public T Value { get; protected set;}

        protected event Action<T> Changed;
        public event Action<T> OnChanged
        {
            add => Changed += value;
            remove => Changed -= value;
        }

        protected ObservableBase(T initialValue)
        {
            Value = initialValue;
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
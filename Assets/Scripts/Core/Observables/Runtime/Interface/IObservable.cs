using System;

namespace Core.Observables.Interface
{
    /// <summary>
    /// Interface representing an observable entity that supports tracking changes to its value
    /// and notifying subscribers when the value changes.
    /// </summary>
    /// <typeparam name="T">The type of the value being observed.</typeparam>
    public interface IObservable<T> : IDisposable
    {
        T Value { get; }
        event Action<T> OnChanged;
        void Set(T value, bool notify = true);
    }
}
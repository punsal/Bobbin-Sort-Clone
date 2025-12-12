using Core.Observables.Abstract;

namespace Core.Observables
{
    /// <summary>
    /// A concrete implementation of <see cref="ScriptableObservableBase{T}"/> that provides
    /// observable behavior for a value in Unity's ScriptableObject context. This class
    /// enables value tracking and change notifications for subscribers, leveraging Unity's
    /// asset-based workflows.
    /// </summary>
    /// <typeparam name="T">The type of the value being observed.</typeparam>
    public class ScriptableObservable<T> : ScriptableObservableBase<T>
    {
        protected override void InternalSet(T value)
        {
            Value = value;
        }
    }
}
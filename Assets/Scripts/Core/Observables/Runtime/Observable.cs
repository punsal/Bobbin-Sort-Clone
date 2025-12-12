using Core.Observables.Abstract;

namespace Core.Observables
{
    /// <summary>
    /// A concrete implementation of an observable entity that extends the functionality
    /// of <see cref="ObservableBase{T}"/>. This class allows for tracking and updating
    /// the value of a specified type while providing notification mechanisms for subscribers
    /// when the value changes.
    /// </summary>
    /// <typeparam name="T">The type of the value being observed.</typeparam>
    public class Observable<T> : ObservableBase<T>
    {
        public Observable(T initialValue) : base(initialValue) { }
        
        protected override void InternalSet(T value) => Value = value;
    }
}
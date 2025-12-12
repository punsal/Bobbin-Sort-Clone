using System;

namespace Core.Event.Interface
{
    public interface IEvent : IDisposable
    {
        event Action OnRaised;
        void Raise();
    }

    public interface IEvent<TArgs> : IDisposable
    {
        event Action<TArgs> OnRaised;
        void Raise(TArgs args);
    }
    
    public interface IEvent<TArgs1, TArgs2> : IDisposable
    {
        event Action<TArgs1, TArgs2> OnRaised;
        void Raise(TArgs1 args1, TArgs2 args2);
    }
}
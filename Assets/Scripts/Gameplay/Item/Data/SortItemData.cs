using System;
using Gameplay.Item.Observables;
using Gameplay.Item.Types;

namespace Gameplay.Item.Data
{
    public class SortItemData : IDisposable
    {
        public SortItemStateObservable StateObservable { get; private set; }
        public SortItemColorObservable ColorObservable { get; private set; }

        public SortItemData(SortItemState initialStateType, SortItemColor initialColor)
        {
            StateObservable = new SortItemStateObservable(initialStateType);
            ColorObservable = new SortItemColorObservable(initialColor);
        }
        
        public void Dispose()
        {
            StateObservable.Dispose();
            ColorObservable.Dispose();
        }
    }
}
using Core.Observables;
using Gameplay.Item.Types;

namespace Gameplay.Item.Observables
{
    public class SortItemStateObservable : Observable<SortItemState>
    {
        public SortItemStateObservable(SortItemState initialValue) : base(initialValue) { }
    }
}
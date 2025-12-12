using Core.Observables;
using Gameplay.Item.Types;

namespace Gameplay.Item.Observables
{
    public class SortItemColorObservable : Observable<SortItemColor>
    {
        public SortItemColorObservable(SortItemColor initialValue) : base(initialValue) { }
    }
}
using System;
using System.Reactive;

namespace Sextant.Abstraction
{
    public interface IMasterViewModel : IPageViewModel, IViewLifeCycle
    {
        IObservable<Unit> On<T>();

        IObservable<Unit> Parent();
    }
}
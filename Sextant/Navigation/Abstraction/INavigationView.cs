using System;
using System.Reactive;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Defines a view that be add to a navigation or modal stack.
    /// </summary>
    public interface INavigationView : IView
    {
        /// <summary>
        /// An observable notifying that a page was pushed to the navigation stack.
        /// </summary>
        IObservable<IPageViewModel> PagePushed { get; }
    }
}
using System;
using System.Reactive;
using Xamarin.Forms;

namespace Sextant.Abstraction
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a detail view for a <see cref="T:Xamarin.Forms.MasterDetailPage" />.
    /// </summary>
    /// <seealso cref="T:Sextant.Abstraction.INavigationView" />
    public interface IDetailView : IView, IViewLifeCycle
    {
        /// <summary>
        /// Gets a value that determines if the view is appearing.
        /// </summary>
        IObservable<Unit> ViewDisappearing { get; }
        
        /// <summary>
        /// Gets a value that determines if the view is disappearing.
        /// </summary>
        IObservable<Unit> ViewAppearing { get; }
    }
}
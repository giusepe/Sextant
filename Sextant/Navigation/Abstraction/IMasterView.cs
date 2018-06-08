using System;
using System.Reactive;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Defines a master view for
    /// </summary>
    public interface IMasterView
    {
        /// <summary>
        /// Gets the back pressed.
        /// </summary>
        IObservable<Unit> BackPressed { get; }

        /// <summary>
        /// Gets or sets the detail view.
        /// </summary>
        IDetailView DetailView { get; set; }
        
        /// <summary>
        /// Gets a value that determines if the view is appearing.
        /// </summary>
        IObservable<Unit> ViewDisappearing { get; }
        
        /// <summary>
        /// Gets a value that determines if the view is disappearing.
        /// </summary>
        IObservable<Unit> ViewAppearing { get; }
        
        /// <summary>
        /// Gets the observable stream that says the detail view is presented.
        /// </summary>
        IObservable<bool> Presented { get; }
    }
}
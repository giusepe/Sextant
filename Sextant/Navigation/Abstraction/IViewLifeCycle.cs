using System;
using System.Reactive;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Interface that defines page life cycle methods.
    /// </summary>
    public interface IViewLifeCycle
    {
        /// <summary>
        /// An observable notifying that a view is appearing.
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> ViewAppearing();

        /// <summary>
        /// An observable notifying that a view is disappearing.
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> ViewDisappearing();

        /// <summary>
        /// A method that initializes the view model.
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> Init();

        /// <summary>
        /// A method that initializes the view model with the specified <see cref="TParameter"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <typeparam name="TParameter"></typeparam>
        /// <returns></returns>
        IObservable<Unit> Init<TParameter>(TParameter parameter);
    }
}
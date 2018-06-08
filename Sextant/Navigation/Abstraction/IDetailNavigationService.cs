using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using System.Text;
using Sextant.Abstraction;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Interface that defines a master detail navigation service.
    /// </summary>
    public interface IDetailNavigationService : IViewStackService
    {
        /// <summary>
        /// Gets the master view.
        /// </summary>
        IMasterView MasterView { get; }
        
        /// <summary>
        /// Gets the detail view.
        /// </summary>
        IDetailView DetailView { get; }

        /// <summary>
        /// Gets a value determining whether the detail view is presented.
        /// </summary>
        /// <returns></returns>
        IObservable<bool> Presented();

        /// <summary>
        /// Sets the detail view.
        /// </summary>
        /// <param name="detailView"></param>
        /// <returns></returns>
        IObservable<Unit> SetDetail(IDetailView detailView);
    }
}

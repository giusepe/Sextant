using System;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Genesis.Logging;
using Sextant.Abstraction;

namespace Sextant.Navigation
{
    public class DetailNavigationService : IDetailNavigationService
    {
        private readonly ILogger _logger;
        private readonly BehaviorSubject<IImmutableList<IPageViewModel>> _modalStack;
        private readonly BehaviorSubject<IImmutableList<IPageViewModel>> _pageStack;
        private readonly IMasterView _masterView;
        private IDetailView _detailView;

        /// <inheritdoc />
        public IObservable<IImmutableList<IPageViewModel>> ModalStack => _modalStack;

        /// <inheritdoc />
        public IObservable<IImmutableList<IPageViewModel>> PageStack => _pageStack;

        /// <inheritdoc />
        public IView View => _detailView;
        
        public DetailNavigationService(IMasterView masterView)
        {
            _masterView = masterView;
        }

        /// <inheritdoc />
        public IObservable<Unit> PopModal(bool animate = true) => _detailView.PopModal();

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true) => _detailView.PopPage(animate);

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IPageViewModel modal, string contract = null) =>
            _detailView.PushModal(modal, contract);

        /// <inheritdoc />
        public IObservable<Unit> PushPage(IPageViewModel page, string contract = null, bool resetStack = false,
            bool animate = true) => _detailView.PushPage(page, contract, resetStack, animate);

        /// <inheritdoc />
        public IObservable<IPageViewModel> TopModal()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IObservable<IPageViewModel> TopPage()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMasterView MasterView => _masterView;

        /// <inheritdoc />
        public IDetailView DetailView => _masterView.DetailView;

        /// <inheritdoc />
        public IObservable<bool> Presented() => _masterView.Presented;

        /// <inheritdoc />
        public IObservable<Unit> SetDetail(IDetailView detailView)
        {
            //TODO: Reset Stack?
            _masterView.DetailView = detailView;
            return Observable.Return(Unit.Default);
        }
    }
}